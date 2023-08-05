using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameEvent onStartSetEnemyMaxHealth;

    [SerializeField]
    private GameEvent onEnemyHealthMeterUpdateUI;

    [SerializeField]
    private GameEvent onEnemyHealthValueChange;

    [SerializeField]
    private GameEvent onEnemyHealthThresholdChange;

    [SerializeField]
    private GameEvent DEBUG_onGameEnd;

    private float healthValue;

    private string healthThreshold = "";

    private void Start()
    {
        healthValue = maxHealth;
        onEnemyHealthValueChange.Raise(this, healthValue);
    }

    private void Update()
    {
        onEnemyHealthMeterUpdateUI.Raise(this, healthValue);
    }

    public void UpdateEnemyHealthValue(float value)
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
        onEnemyHealthValueChange.Raise(this, healthValue);
    }

    //Sends state whenever health reaches certain thresholds
    public void CheckHealthThreshold()
    {
        if (healthValue <= maxHealth * 0.25 && !healthThreshold.Equals("25"))
        {
            healthThreshold = "25";
            onEnemyHealthThresholdChange.Raise(this, healthThreshold);
        }
        else if (healthValue <= maxHealth * 0.50 && !healthThreshold.Equals("50"))
        {
            healthThreshold = "50";
            onEnemyHealthThresholdChange.Raise(this, healthThreshold);
        }
        else if (healthValue <= maxHealth * 0.75 && !healthThreshold.Equals("75"))
        {
            healthThreshold = "75";
            onEnemyHealthThresholdChange.Raise(this, healthThreshold);
        }
        else if (healthValue <= maxHealth && !healthThreshold.Equals("100"))
        {
            healthThreshold = "100";
            onEnemyHealthThresholdChange.Raise(this, healthThreshold);
        }
    }
}
