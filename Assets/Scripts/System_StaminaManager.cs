using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_StaminaManager : MonoBehaviour
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
    private float staggerTime;

    [SerializeField]
    private GameEvent onPlayerStaggerEnable;

    [SerializeField]
    private GameEvent onPlayerStaggerDisable;

    [SerializeField]
    private GameEvent onStaminaUpdateUI;

    private float parryCounter;

    private float staminaValue;

    private float staminaRecoveryRate;

    private bool parryIsHeld;

    private bool isPerfectParry;

    private bool canRecoverStamina = true;

    private bool canStaggerPlayer = true;

    private bool isRunning_StartStaminaRecoveryTimer;

    private bool isRunning_PlayerStaggerCounter;

    private Coroutine current_StartStaminaRecoveryTimer;

    private void Start()
    {
        staminaValue = maxStamina;
        staminaRecoveryRate = staminaRecoveryRateBase;
    }

    private void Update()
    {
        //Stamina Recovery System
        if (parryIsHeld && canRecoverStamina && staminaValue < maxStamina)
        {
            if (
                staminaValue + (staminaRecoveryRate * Time.deltaTime) >
                maxStamina
            )
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

    public void UpdateParryCounter()
    {
        parryCounter++;
    }

    public void IsPerfectParry(bool value)
    {
        isPerfectParry = value;
    }

    // Updates Stamina Value on certain calls or events (ex. on parry)
    public void UpdateStaminaValue(int value)
    {
        if (staminaValue + value < 0 && isPerfectParry)
        {
            staminaValue = 1;
        }
        else if (staminaValue + value < 0 && !isPerfectParry)
        {
            staminaValue = 0;
            if (canStaggerPlayer)
            {
                onPlayerStaggerEnable.Raise(this, null);
                StartCoroutine(PlayerStaggerTimer());
            }
        }
        else if (staminaValue + value > maxStamina)
        {
            staminaValue = maxStamina;
        }
        else
        {
            if (value < 0)
            {
                if (isRunning_StartStaminaRecoveryTimer)
                {
                    StopCoroutine (current_StartStaminaRecoveryTimer);
                }
                current_StartStaminaRecoveryTimer =
                    StartCoroutine(StartStaminaRecoveryTimer());
                canRecoverStamina = false;
            }
            staminaValue += value;
        }
    }

    //Called when parry is held
    public void SetParryIsHeld(bool value)
    {
        parryIsHeld = value;
    } //

    //Checks what should be the stamina regen rate based on player Health
    public void UpdateStaminaRecoveryRate(Component sender, object data)
    {
        float health = (float) data;
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

    //Starts a delay counter before stamina can recover after stamina is decreased
    IEnumerator StartStaminaRecoveryTimer()
    {
        isRunning_StartStaminaRecoveryTimer = true;
        yield return new WaitForSeconds(staminaRecoveryDelay);
        if (!isRunning_PlayerStaggerCounter)
        {
            canRecoverStamina = true;
        }
        isRunning_StartStaminaRecoveryTimer = false;
    }

    IEnumerator PlayerStaggerTimer()
    {
        isRunning_PlayerStaggerCounter = true;
        canStaggerPlayer = false;
        canRecoverStamina = false;
        yield return new WaitForSeconds(staggerTime);
        staminaValue = maxStamina;
        onPlayerStaggerDisable.Raise(this, null);
        canRecoverStamina = true;
        canStaggerPlayer = true;
        isRunning_PlayerStaggerCounter = false;
    }
}
