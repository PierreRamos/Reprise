using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerConsumables : MonoBehaviour
{
    //General game event for consumable use
    [SerializeField]
    private GameEvent onPlayerUseConsumable;

    //General game event for consumables
    [SerializeField]
    private GameEvent onPlayerUseHealthPotion;

    [SerializeField]
    private GameEvent onPlayerInterruptedDuringConsumable;

    [SerializeField]
    private GameEvent updatePotionUI;

    [SerializeField]
    private int potionCount;

    private bool canUse_Consumable;

    private void Start()
    {
        canUse_Consumable = true;
        updatePotionUI.Raise(this, potionCount);
    }

    public void UseConsumable()
    {
        if (canUse_Consumable)
        {
            //Potion
            if (potionCount > 0)
            {
                UseHealthPotion();
                onPlayerUseConsumable.Raise(this, null);
            }
        }

        //local functions
        void UseHealthPotion()
        {
            canUse_Consumable = false;
            onPlayerUseHealthPotion.Raise(this, potionCount);
        }
    }

    public void CanUseConsumable()
    {
        canUse_Consumable = true;
    }

    public void InterruptConsumable()
    {
        if (canUse_Consumable == false)
        {
            canUse_Consumable = true;
            onPlayerInterruptedDuringConsumable.Raise(this, null);
        }
    }

    public void UsedConsumable(string consumable)
    {
        if (consumable.Equals("Potion"))
        {
            potionCount--;
            updatePotionUI.Raise(this, potionCount);
        }
    }
}
