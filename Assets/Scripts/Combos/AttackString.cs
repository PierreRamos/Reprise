using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "String_", menuName = "Combos/String")]
public class AttackString : ScriptableObject
{
    public List<Attack> attackString = new List<Attack>();
}
