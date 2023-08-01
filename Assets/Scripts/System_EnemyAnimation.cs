using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerEnemyCast()
    {
        animator.SetTrigger("EnemyCast");
    }

    public void TriggerEnemyHit()
    {
        animator.SetTrigger("EnemyHit");
    }

    public void SetEnemyStun(Component sender, object data)
    {
        bool isStunned = (bool) data;
        animator.SetBool("EnemyStunned", isStunned);
    }
}
