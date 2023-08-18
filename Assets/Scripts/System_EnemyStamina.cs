using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyStamina : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float maxStamina;

    [SerializeField]
    private float staminaRecoveryRateBase;

    [SerializeField]
    private float staminaRecoveryDelay;

    [SerializeField]
    private GameEvent onEnemyVulnerable;

    [SerializeField]
    private GameEvent onStaminaUpdateUI;

    [SerializeField]
    private GameEvent DEBUG_onGameEnd;

    private float parryCounter;

    private float staminaValue;

    private float staminaRecoveryRate;

    private int perfectParryStreak;

    private bool canRecoverStamina = true;

    private bool isRunning_StartStaminaRecoveryTimer;

    private bool isRunning_PlayerStaggerCounter;

    private bool isRunning_PerfectParryStreakResetTimer;

    private Coroutine current_StartStaminaRecoveryTimer;

    private Coroutine current_PerfectParryStreakResetTimer;

    private void Start()
    {
        staminaValue = maxStamina;
        staminaRecoveryRate = staminaRecoveryRateBase;
    }

    private void Update()
    {
        //Stamina Recovery System
        if (!isCurrentlyAttacking && canRecoverStamina && staminaValue < maxStamina)
        {
            if (staminaValue + (staminaRecoveryRate * Time.deltaTime) > maxStamina)
            {
                staminaValue = maxStamina;
            }
            else
            {
                staminaValue += staminaRecoveryRate * Time.deltaTime;
            }
        }

        //Updates the stamina UI every frame
        onStaminaUpdateUI.Raise(this, staminaValue);
    }

    // Updates Stamina Value on certain calls or events (ex. on parry)
    public void UpdateStaminaValue(int value)
    {
        float percentageIncrease = perfectParryStreak * 0.1f;
        float staminaDamage = value + (value * percentageIncrease);
        if (staminaValue + staminaDamage < 0)
        {
            staminaValue = 0;

            onEnemyVulnerable.Raise(this, null);
            DEBUG_onGameEnd.Raise(this, null);
        }
        else if (staminaValue + value > maxStamina)
        {
            staminaValue = maxStamina;
        }
        else
        {
            if (staminaDamage < 0)
            {
                StartStaminaRecoveryTimer();
            }
            staminaValue += staminaDamage;
        }
    }

    //Adds to perfect parry streak when player perfect parries
    public void AddPerfectParryStreak()
    {
        perfectParryStreak++;
        if (isRunning_PerfectParryStreakResetTimer)
        {
            StopCoroutine(current_PerfectParryStreakResetTimer);
        }
        current_PerfectParryStreakResetTimer = StartCoroutine(PerfectParryStreakResetTimer());
    }

    //Resets parry streak when player misses a perfect parry or gets damaged
    public void ResetPerfectParryStreak()
    {
        perfectParryStreak = 0;
    }

    //Checks what should be the stamina regen rate based on enemy health
    public void UpdateStaminaRecoveryRate(Component sender, object data)
    {
        float health = (float)data;
        if (health < (maxHealth * 0.25))
        {
            staminaRecoveryRate = staminaRecoveryRateBase * 0.01f;
        }
        else if (health < (maxHealth * 0.50))
        {
            staminaRecoveryRate = staminaRecoveryRateBase * 0.33f;
        }
        else if (health < (maxHealth * 0.75))
        {
            staminaRecoveryRate = staminaRecoveryRateBase * 0.66f;
        }
        else
        {
            staminaRecoveryRate = staminaRecoveryRateBase;
        }
    }

    //Checks whether enemy is attacking and halts stamina regeneration

    private bool isCurrentlyAttacking;

    public void SetIsCurrentlyAttacking(bool state)
    {
        if (state == false)
        {
            StartStaminaRecoveryTimer();
        }
        isCurrentlyAttacking = state;
    }

    //Perfect parry streak timer before it resets
    IEnumerator PerfectParryStreakResetTimer()
    {
        isRunning_PerfectParryStreakResetTimer = true;
        yield return new WaitForSeconds(2f);
        ResetPerfectParryStreak();
        isRunning_PerfectParryStreakResetTimer = false;
    }

    //Starts the stamina recovery timer
    public void StartStaminaRecoveryTimer()
    {
        if (isRunning_StartStaminaRecoveryTimer)
        {
            StopCoroutine(current_StartStaminaRecoveryTimer);
        }
        current_StartStaminaRecoveryTimer = StartCoroutine(StaminaRecoveryTimer());
        canRecoverStamina = false;

        //Starts a delay counter before stamina can recover after stamina is decreased
        IEnumerator StaminaRecoveryTimer()
        {
            isRunning_StartStaminaRecoveryTimer = true;
            yield return new WaitForSeconds(staminaRecoveryDelay);
            if (!isRunning_PlayerStaggerCounter)
            {
                canRecoverStamina = true;
            }
            isRunning_StartStaminaRecoveryTimer = false;
        }
    }
}
