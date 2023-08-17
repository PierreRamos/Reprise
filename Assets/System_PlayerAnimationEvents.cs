using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameEvent onPlayerConsumableFinish;

    [SerializeField]
    private GameEvent onPlayerUseHealthPotionConfirm;

    [SerializeField]
    private GameEvent onPlayerReversalConfirm;

    [SerializeField]
    private GameEvent onPlayerShootConfirm;

    [SerializeField]
    private GameEvent onPlayerShootEmpoweredConfirm;

    public void PlayerConsumableFinish()
    {
        onPlayerConsumableFinish.Raise(this, null);
    }

    public void PlayerUseHealthPotionConfirm()
    {
        onPlayerUseHealthPotionConfirm.Raise(this, null);
    }

    public void PlayerShootBulletConfirm()
    {
        onPlayerShootConfirm.Raise(this, null);
    }

    public void PlayerShootEmpoweredConfirm()
    {
        onPlayerShootEmpoweredConfirm.Raise(this, null);
    }

    public void PlayerReversalConfirm()
    {
        onPlayerReversalConfirm.Raise(this, null);
    }
}
