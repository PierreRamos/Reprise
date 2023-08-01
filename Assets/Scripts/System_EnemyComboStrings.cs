using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyComboStrings : MonoBehaviour
{
    [SerializeField]
    private List<AttackString> attackStringList = new List<AttackString>();

    [SerializeField]
    private GameEvent onRequestExecuteAttackString;

    public void RequestExecuteAttackString()
    {
        int index = Random.Range(0, attackStringList.Count);
        onRequestExecuteAttackString.Raise(this, attackStringList[0]); // for now
    }
}
