using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameEvent onEnemyShootConfirm;

    [SerializeField]
    private GameEvent onEnemyEndAttackString;

    public void EnemyShootConfirm(int shootPoint)
    {
        onEnemyShootConfirm.Raise(this, shootPoint);
    }

    public void EnemyEndAttackString()
    {
        onEnemyEndAttackString.Raise(this, null);
    }
}
