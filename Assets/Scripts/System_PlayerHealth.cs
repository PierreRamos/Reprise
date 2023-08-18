using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameEvent onHealthUpdateUI;

    [SerializeField]
    private GameEvent onHealthValueChange;

    [SerializeField]
    private GameEvent DEBUG_onGameEnd;

    private float healthValue;

    private void Start()
    {
        healthValue = maxHealth;
    }

    private void Update()
    {
        onHealthUpdateUI.Raise(this, healthValue);
    }

    public void UpdateHealthValue(float value)
    {
        if (healthValue + value > maxHealth)
        {
            healthValue = maxHealth;
        }
        else if (healthValue + value < 0)
        {
            healthValue = 0;
            DEBUG_onGameEnd.Raise(this, null);
        }
        else
        {
            healthValue += value;
        }
        onHealthValueChange.Raise(this, healthValue);
    }

    //Increase health value
    public void IncreaseHealthValue(float healthIncrease)
    {
        if (healthValue + healthIncrease > maxHealth)
        {
            healthValue = maxHealth;
        }
        else
        {
            healthValue += healthIncrease;
        }
        onHealthValueChange.Raise(this, healthValue);
    }
}
