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
    private GameObject unblockableAttack;

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

    private bool isInIdle = true;

    private bool enemyIsStunned;

    private bool isExecutingAttackString;

    private Coroutine current_IdleWindowTimer;

    private void Start()
    {
        IdleWindow();
    }

    private void Update()
    {
        if (canShoot && !enemyIsStunned)
        {
            if (!isExecutingAttackString)
            {
                onEnemyCanShoot.Raise(this, null);
            }
        }
    }

    public void ExecuteAttackString(Component sender, object data)
    {
        List<Attack> attackList = ((AttackString)data).attackString;
        StartCoroutine(ExecuteAttackStringCoroutine(attackList));
    }

    IEnumerator ExecuteAttackStringCoroutine(List<Attack> attackList)
    {
        isExecutingAttackString = true;
        canShoot = false;
        PlayCastingAnimation();
        yield return new WaitForSeconds(0.25f);
        isActivelyParrying = false;

        //For loop cycles through attacks in attack list
        for (int i = 0; i < attackList.Count; i++)
        {
            bulletDamage = attackList[i].damage * -1;
            float timingDelay = attackList[i].timingDelay;
            yield return new WaitForSeconds(timingDelay); //0 initially
            if (attackList[i].isUnblockable == true)
            {
                onEnemyCastUnblockableAttack.Raise(this, null);
                PlayCastingAnimation(); //temporary
                yield return new WaitForSeconds(0.5f);
                ShootUnblockableBullet();
            }
            else
            {
                ShootBullet();
            }
        }
        StartCoroutine(AfterAttackStringDelay());
        onEnemyFinishString.Raise(this, null);
        isExecutingAttackString = false;

        //Internal methods
        void PlayCastingAnimation()
        {
            onEnemyCastShootBullet.Raise(this, null);
        }

        void ShootBullet()
        {
            Instantiate(bullet, transform.position, transform.rotation);
            onEnemyBulletSpawn.Raise(this, bulletDamage);
        }

        void ShootUnblockableBullet()
        {
            onEnemyShootUnblockable.Raise(this, null);
            Instantiate(unblockableAttack, transform.position, transform.rotation);
            onEnemyBulletSpawn.Raise(this, bulletDamage);
        }

        //Delay after enemy finishes attack string
        IEnumerator AfterAttackStringDelay()
        {
            yield return new WaitForSeconds(0.3f);
            IdleWindow();
        }
    }

    //Method which starts the time window where enemy is idle and is not actively parrying
    private void IdleWindow()
    {
        isInIdle = true;
        onEnemyUpdateStatus.Raise(this, isInIdle);

        if (healthThreshold.Equals("100"))
        {
            current_IdleWindowTimer = StartCoroutine(
                IdleWindowTimer(Random.Range(minIdleTime, maxIdleTime))
            );
        }
        else if (healthThreshold.Equals("75"))
        {
            current_IdleWindowTimer = StartCoroutine(
                IdleWindowTimer(Random.Range(minIdleTime, maxIdleTime * 0.66f))
            );
        }
        else if (healthThreshold.Equals("50"))
        {
            current_IdleWindowTimer = StartCoroutine(
                IdleWindowTimer(Random.Range(minIdleTime, maxIdleTime * 0.33f))
            );
        }
        else if (healthThreshold.Equals("25"))
        {
            current_IdleWindowTimer = StartCoroutine(
                IdleWindowTimer(Random.Range(minIdleTime, maxIdleTime * 0.01f))
            );
        }

        //Internal methods
        //Timer which tells how long idle window is
        IEnumerator IdleWindowTimer(float value)
        {
            yield return new WaitForSeconds(value);
            ActiveWindow();
        }
    }

    //Method which starts the time window where enemy is idle but is actively parrying
    private void ActiveWindow()
    {
        hitCount = 0;
        isInIdle = false;
        onEnemyUpdateStatus.Raise(this, isInIdle);
        isActivelyParrying = true;
        if (healthThreshold.Equals("100"))
        {
            StartCoroutine(AfterActiveTimer(Random.Range(0, maxAfterActiveAttackDelay)));
        }
        if (healthThreshold.Equals("75"))
        {
            StartCoroutine(AfterActiveTimer(Random.Range(0, maxAfterActiveAttackDelay * 0.66f)));
        }
        if (healthThreshold.Equals("50"))
        {
            StartCoroutine(AfterActiveTimer(Random.Range(0, maxAfterActiveAttackDelay * 0.33f)));
        }
        if (healthThreshold.Equals("25"))
        {
            StartCoroutine(AfterActiveTimer(Random.Range(0, maxAfterActiveAttackDelay * 0.01f)));
        }

        //Internal methods
        //Starts a short timer which dictates when the enemy will start an attack string
        IEnumerator AfterActiveTimer(float value)
        {
            yield return new WaitForSeconds(value);
            canShoot = true;
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

    //Is called everytime the enemy is hit by player bullet and has a chance to stop the idle time
    public void EnemyHitCheck()
    {
        if (isInIdle && !enemyIsStunned)
        {
            hitCount++;
            int chanceNumber = 80 * hitCount;
            int randomNumber = (int)Random.Range(0, 100f);
            if (chanceNumber > randomNumber)
            {
                StopCoroutine(current_IdleWindowTimer);
                ActiveWindow();
            }
        }
    } //

    //Stun enemy when called by OnPlayerEmpoweredBulletEnterEnemy
    public void StartStunEnemy(Component sender, object data)
    {
        float stunTime = (float)data;
        enemyIsStunned = true;
        onEnemyStunned.Raise(this, enemyIsStunned);
        StopAllCoroutines();
        isActivelyParrying = false;
        isExecutingAttackString = false;
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
        isActivelyParrying = false;
        isExecutingAttackString = false;
    }

    //Sets enemy health threshold
    public void SetHealthThreshold(Component sender, object data)
    {
        string threshold = (string)data;
        healthThreshold = threshold;
    }
}
