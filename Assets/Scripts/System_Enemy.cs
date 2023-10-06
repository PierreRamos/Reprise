using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Enemy : MonoBehaviour
{
    [SerializeField]
    private float minIdleTime;

    [SerializeField]
    private float maxIdleTime;

    [SerializeField]
    private float maxAfterActiveAttackDelay;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform shootPoint1;

    [SerializeField]
    private Transform shootPoint2;

    [SerializeField]
    private GameObject unblockableAttack;

    [SerializeField]
    private GameEvent startString;

    [SerializeField]
    private GameEvent onEnemyCastShootBullet;

    [SerializeField]
    private GameEvent onEnemyCastUnblockableAttack;

    [SerializeField]
    private GameEvent onEnemyBulletSpawn;

    [SerializeField]
    private GameEvent onEnemyCanShoot;

    [SerializeField]
    private GameEvent onEnemyParryPlayerBullet;

    [SerializeField]
    private GameEvent onConfirmPlayerBulletHitEnemy;

    [SerializeField]
    private GameEvent onEnemyStunned;

    [SerializeField]
    private GameEvent onEnemyCanParry;

    [SerializeField]
    private GameEvent onEnemyCannotParry;

    [SerializeField]
    private GameEvent onEnemyUpdateStatus;

    [SerializeField]
    private GameEvent onEnemyFinishString;

    [SerializeField]
    private GameEvent onEnemyShootUnblockable;

    [SerializeField]
    private float castTime;

    //Increments if enemy is hit during idle time
    private int hitCount;

    //Temporary damage holder
    private float bulletDamage;

    //Holds current enemy health threshold so can dictate whether is more aggressive relative to health
    private string healthThreshold = "100";

    private bool canShoot;

    private bool isActivelyParrying;

    private bool enemyIsStunned;

    private bool isExecutingAttackString;

    private Coroutine current_IdleWindowTimer;

    private void Start()
    {
        IdleWindow();
    }

    private bool isCurrentlyParrying;

    private void Update()
    {
        if (canShoot && !enemyIsStunned)
        {
            if (!isExecutingAttackString && !isCurrentlyParrying)
            {
                canShoot = false;
                isExecutingAttackString = true;
                isActivelyParrying = false;
                startString.Raise(this, null);
                onEnemyCannotParry.Raise(this, null);
                // onEnemyCanShoot.Raise(this, null); //temporary
            }
        }
    }

    //Ends attack string, gets called via animation events
    public void EndAttackString()
    {
        if (isExecutingAttackString)
        {
            isExecutingAttackString = false;
        }
        IdleWindow();
    }

    //Shoots enemy bullet, gets called by via animation events
    public void EnemyShoot(Component sender, object shootPoint)
    {
        int point = (int)shootPoint;
        if (point == 1)
        {
            Instantiate(bullet, shootPoint1.position, transform.rotation);
        }
        else if (point == 2)
        {
            Instantiate(bullet, shootPoint2.position, transform.rotation);
        }
        onEnemyBulletSpawn.Raise(this, bulletDamage);
    }

    //Sets whether enemy is preparing to parry or not, condition whether can start attack string
    public void SetIsCurrentlyParrying(bool state)
    {
        isCurrentlyParrying = state;
    }

    //Method which starts the time window where enemy is idle and is not actively parrying

    private bool isRunning_IdleWindowTimer;

    private bool isIdle;

    private void IdleWindow()
    {
        isIdle = true;
        isActive = false;
        float time = 0;

        if (healthThreshold.Equals("100"))
        {
            time = Random.Range(minIdleTime, maxIdleTime);
        }
        else if (healthThreshold.Equals("75"))
        {
            time = Random.Range(minIdleTime * 0.9f, maxIdleTime * 0.9f);
        }
        else if (healthThreshold.Equals("50"))
        {
            time = Random.Range(minIdleTime * 0.8f, maxIdleTime * 0.8f);
        }
        else if (healthThreshold.Equals("25"))
        {
            time = Random.Range(minIdleTime * 0.7f, maxIdleTime * 0.7f);
        }

        current_IdleWindowTimer = StartCoroutine(IdleWindowTimer(time));

        //Internal methods
        //Timer which tells how long idle window is
        IEnumerator IdleWindowTimer(float value)
        {
            isRunning_IdleWindowTimer = true;
            yield return new WaitForSeconds(value);
            ActiveWindow();
            isRunning_IdleWindowTimer = false;
        }
    }

    //Method which starts the time window where enemy is idle but is actively parrying

    private Coroutine current_AfterActiveTimer;

    private bool isRunning_AfterActiveTimer;

    private bool isActive;

    private void ActiveWindow()
    {
        isIdle = false;
        isActive = true;
        float time = 0;
        if (healthThreshold.Equals("100"))
        {
            time = Random.Range(3, maxAfterActiveAttackDelay);
        }
        else if (healthThreshold.Equals("75"))
        {
            time = Random.Range(3 * 0.9f, maxAfterActiveAttackDelay * 0.9f);
        }
        else if (healthThreshold.Equals("50"))
        {
            time = Random.Range(3 * 0.8f, maxAfterActiveAttackDelay * 0.8f);
        }
        else if (healthThreshold.Equals("25"))
        {
            time = Random.Range(3 * 0.7f, maxAfterActiveAttackDelay * 0.7f);
        }

        onEnemyCanParry.Raise(this, null);
        isActivelyParrying = true;

        if (isRunning_AfterActiveTimer)
        {
            StopCoroutine(current_AfterActiveTimer);
        }
        current_AfterActiveTimer = StartCoroutine(AfterActiveTimer(time));

        //Internal methods
        //Starts a short timer which dictates when the enemy will start an attack string
        IEnumerator AfterActiveTimer(float value)
        {
            isRunning_AfterActiveTimer = true;
            yield return new WaitForSeconds(value);
            canShoot = true;
            isRunning_AfterActiveTimer = false;
        }
    }

    //Method which checks if enemy will parry player bullet or not; Gets called by OnPlayerBulletHitEnemy
    public void CheckIfEnemyWillParry(Component sender, object data)
    {
        if (isActivelyParrying)
        {
            onEnemyParryPlayerBullet.Raise(this, data);
        }
        else
        {
            onConfirmPlayerBulletHitEnemy.Raise(this, null);
        }
    }

    //Is called everytime the enemy is hit by player bullet
    public void EnemyHitCheck()
    {
        if (!enemyIsStunned && !isExecutingAttackString)
        {
            hitCount++;
            int chanceNumber = 75 * hitCount;
            int randomNumber = (int)Random.Range(0, 100f);
            if (chanceNumber > randomNumber)
            {
                if (isActive)
                {
                    StopCoroutine(current_AfterActiveTimer);
                    canShoot = true;
                }
                else if (isIdle)
                {
                    StopCoroutine(current_IdleWindowTimer);
                    ActiveWindow();
                }
                hitCount = 0;
            }
        }
    } //

    //Stun enemy when called by OnPlayerEmpoweredBulletEnterEnemy
    public void StartStunEnemy(Component sender, object data)
    {
        float stunTime = (float)data;
        enemyIsStunned = true;
        onEnemyStunned.Raise(this, enemyIsStunned);
        InterruptEnemy();
        StartCoroutine(StunEnemyTimer(stunTime));

        //Internal methods
        IEnumerator StunEnemyTimer(float time)
        {
            yield return new WaitForSeconds(time);
            enemyIsStunned = false;
            onEnemyStunned.Raise(this, enemyIsStunned);
            ActiveWindow();
        }
    }

    //Interrupts enemy and halts all coroutines
    public void InterruptEnemy()
    {
        StopAllCoroutines();
        onEnemyCannotParry.Raise(this, null);
        isActivelyParrying = false;
        isExecutingAttackString = false;
    }

    //Sets enemy health threshold
    public void SetHealthThreshold(Component sender, object data)
    {
        string threshold = (string)data;
        healthThreshold = threshold;
    }

    // //Temporarily turned off
    // public void ExecuteAttackString(Component sender, object data)
    // {
    //     List<Attack> attackList = ((AttackString)data).attackString;
    //     StartCoroutine(ExecuteAttackStringCoroutine(attackList));
    // }

    // IEnumerator ExecuteAttackStringCoroutine(List<Attack> attackList)
    // {
    //     isExecutingAttackString = true;
    //     canShoot = false;
    //     PlayCastingAnimation();
    //     yield return new WaitForSeconds(0.25f);
    //     onEnemyCannotParry.Raise(this, null);
    //     isActivelyParrying = false;

    //     //For loop cycles through attacks in attack list
    //     for (int i = 0; i < attackList.Count; i++)
    //     {
    //         bulletDamage = attackList[i].damage * -1;
    //         float timingDelay = attackList[i].timingDelay;
    //         yield return new WaitForSeconds(timingDelay); //0 initially
    //         if (attackList[i].isUnblockable == true)
    //         {
    //             onEnemyCastUnblockableAttack.Raise(this, null);
    //             PlayCastingAnimation(); //temporary
    //             yield return new WaitForSeconds(0.5f);
    //             ShootUnblockableBullet();
    //         }
    //         else
    //         {
    //             ShootBullet();
    //         }
    //     }
    //     StartCoroutine(AfterAttackStringDelay());
    //     onEnemyFinishString.Raise(this, null);
    //     isExecutingAttackString = false;

    //     //Internal methods
    //     void PlayCastingAnimation()
    //     {
    //         onEnemyCastShootBullet.Raise(this, null);
    //     }

    //     void ShootBullet()
    //     {
    //         Instantiate(bullet, transform.position, transform.rotation);
    //         onEnemyBulletSpawn.Raise(this, bulletDamage);
    //     }

    //     void ShootUnblockableBullet()
    //     {
    //         onEnemyShootUnblockable.Raise(this, null);
    //         Instantiate(unblockableAttack, transform.position, transform.rotation);
    //         onEnemyBulletSpawn.Raise(this, bulletDamage);
    //     }

    //     //Delay after enemy finishes attack string
    //     IEnumerator AfterAttackStringDelay()
    //     {
    //         yield return new WaitForSeconds(0.3f);
    //         IdleWindow();
    //     }
    // }
}
