using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameEvent onEnemyShootConfirm;

    [SerializeField]
    private GameEvent onEnemyEndAttackString;

    [SerializeField]
    private GameEvent onEnemyEndParry;

    public void EnemyShootConfirm(int shootPoint)
    {
        onEnemyShootConfirm.Raise(this, shootPoint);
    }

    public void EnemyEndParry()
    {
        onEnemyEndParry.Raise(this, null);
    }

    public void EnemyEndAttackString()
    {
        onEnemyEndAttackString.Raise(this, null);
    }
}
