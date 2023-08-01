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
    private GameEvent onEnemyCastShootBullet;

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
    private float castTime;

    //Increments if enemy is hit during idle time
    private int hitCount;

    //Temporary damage holder
    private float bulletDamage;

    //Holds current enemy health so can dictate whether is more aggressive relative to health
    private float enemyHealth;

    private string healthThreshold = "100";

    private bool canShoot;

    private bool isActivelyParrying;

    private bool isInIdle = true;

    private bool enemyIsStunned;

    private bool isExecutingAttackString;

    private Coroutine current_IdleWindowTimer;

    private Coroutine current_ExecuteAttackStringCoroutine;

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
        List<Attack> attackList = ((AttackString) data).attackString;
        current_ExecuteAttackStringCoroutine =
            StartCoroutine(ExecuteAttackStringCoroutine(attackList));
    }

    IEnumerator ExecuteAttackStringCoroutine(List<Attack> attackList)
    {
        isExecutingAttackString = true;
        canShoot = false;

        // float hitCastTimeAdjustment = (0.5f - 0.3f);
        PlayCastingAnimation();
        yield return new WaitForSeconds(0.25f);
        isActivelyParrying = false;
        for (int i = 0; i < attackList.Count; i++)
        {
            bulletDamage = attackList[i].damage * -1;
            float placeholder = 0;
            float timingDelay = attackList[i].timingDelay;
            float temp = timingDelay - 0.3f;
            if (temp < 0 && i != 0)
            {
                placeholder = temp;
            }

            //Prototype
            yield return new WaitForSeconds(timingDelay); //0 initially
            ShootBullet();
        }
        StartCoroutine(AfterAttackStringDelay());
        onEnemyFinishString.Raise(this, null);
        isExecutingAttackString = false;
    }

    //Method which starts the time window where enemy is idle and is not actively parrying
    private void IdleWindow()
    {
        isInIdle = true;
        onEnemyUpdateStatus.Raise(this, isInIdle);

        if (healthThreshold.Equals("100"))
        {
            current_IdleWindowTimer =
                StartCoroutine(IdleWindowTimer(Random
                    .Range(minIdleTime, maxIdleTime)));
        }
        else if (healthThreshold.Equals("75"))
        {
            current_IdleWindowTimer =
                StartCoroutine(IdleWindowTimer(Random
                    .Range(minIdleTime, maxIdleTime * 0.66f)));
        }
        else if (healthThreshold.Equals("50"))
        {
            current_IdleWindowTimer =
                StartCoroutine(IdleWindowTimer(Random
                    .Range(minIdleTime, maxIdleTime * 0.33f)));
        }
        else if (healthThreshold.Equals("25"))
        {
            current_IdleWindowTimer =
                StartCoroutine(IdleWindowTimer(Random
                    .Range(minIdleTime, maxIdleTime * 0.01f)));
        }
    } //

    //Method which starts the time window where enemy is idle but is actively parrying
    private void ActiveWindow()
    {
        hitCount = 0;
        isInIdle = false;
        onEnemyUpdateStatus.Raise(this, isInIdle);
        isActivelyParrying = true;
        if (healthThreshold.Equals("100"))
        {
            StartCoroutine(AfterActiveTimer(Random
                .Range(0, maxAfterActiveAttackDelay)));
        }
        if (healthThreshold.Equals("75"))
        {
            StartCoroutine(AfterActiveTimer(Random
                .Range(0, maxAfterActiveAttackDelay * 0.66f)));
        }
        if (healthThreshold.Equals("50"))
        {
            StartCoroutine(AfterActiveTimer(Random
                .Range(0, maxAfterActiveAttackDelay * 0.33f)));
        }
        if (healthThreshold.Equals("25"))
        {
            StartCoroutine(AfterActiveTimer(Random
                .Range(0, maxAfterActiveAttackDelay * 0.01f)));
        }
    } //

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
    } //

    //Is called everytime the enemy is hit by player bullet and has a chance to stop the idle time
    public void EnemyHitCheck()
    {
        if (isInIdle && !enemyIsStunned)
        {
            hitCount++;
            int chanceNumber = 80 * hitCount;
            int randomNumber = (int) Random.Range(0, 100f);
            if (chanceNumber > randomNumber)
            {
                StopCoroutine (current_IdleWindowTimer);
                ActiveWindow();
            }
        }
    } //

    //Stun enemy when called by OnPlayerEmpoweredBulletEnterEnemy
    public void StartStunEnemy(Component sender, object data)
    {
        float stunTime = (float) data;
        enemyIsStunned = true;
        onEnemyStunned.Raise(this, enemyIsStunned);
        StopAllCoroutines();
        isActivelyParrying = false;
        isExecutingAttackString = false;
        StartCoroutine(StunEnemyTimer(stunTime));
    }

    //Sets enemy health threshold
    public void SetHealthThreshold(Component sender, object data)
    {
        string threshold = (string) data;
        healthThreshold = threshold;
    }

    IEnumerator StunEnemyTimer(float time)
    {
        yield return new WaitForSeconds(time);
        enemyIsStunned = false;
        onEnemyStunned.Raise(this, enemyIsStunned);
        ActiveWindow();
    }

    //Delay after enemy finishes attack string
    IEnumerator AfterAttackStringDelay()
    {
        yield return new WaitForSeconds(0.3f);
        IdleWindow();
    }

    //Timer which tells how long idle window is
    IEnumerator IdleWindowTimer(float value)
    {
        yield return new WaitForSeconds(value);
        ActiveWindow();
    }

    //Starts a short timer which dictates when the enemy will start an attack string
    IEnumerator AfterActiveTimer(float value)
    {
        yield return new WaitForSeconds(value);
        canShoot = true;
    }

    private void PlayCastingAnimation()
    {
        onEnemyCastShootBullet.Raise(this, null);
    }

    private void ShootBullet()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        onEnemyBulletSpawn.Raise(this, bulletDamage);
    }
}
