using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameEvent onPlayerConsumableFinish;

    [SerializeField]
    private GameEvent onPlayerUseHealthPotionConfirm;

    public void PlayerConsumableFinish()
    {
        onPlayerConsumableFinish.Raise(this, null);
    }

    public void PlayerUseHealthPotionConfirm()
    {
        onPlayerUseHealthPotionConfirm.Raise(this, null);
    }
}
