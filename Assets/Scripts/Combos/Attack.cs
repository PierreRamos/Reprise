using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack_", menuName = "Combos/Attack")]
public class Attack : ScriptableObject
{
    public float damage;

    public float timingDelay;

    public bool isUnblockable;
}
