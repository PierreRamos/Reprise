using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_SpecialMeterAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void SetSpecialBarIsFull(Component sender, object value)
    {
        animator.SetBool("IsFull", (bool) value);
    }
}
