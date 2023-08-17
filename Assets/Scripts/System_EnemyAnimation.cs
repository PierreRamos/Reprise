using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool canParry;

    private bool projectileIsPresent;

    private bool isParryPrepped;

    private void Update()
    {
        if (projectileIsPresent && !isParryPrepped)
        {
            TriggerParryPrep();
        }
    }

    public void TriggerEnemyCast()
    {
        animator.SetTrigger("EnemyCast");
    }

    public void TriggerEnemyHit()
    {
        animator.SetTrigger("EnemyHit");
    }

    public void TriggerParryPrep()
    {
        if (canParry)
        {
            animator.ResetTrigger("Parry");
            animator.SetTrigger("ParryPrep");
            isParryPrepped = true;
        }
    }

    public void TriggerParry()
    {
        animator.ResetTrigger("ParryPrep");
        animator.SetTrigger("Parry");
        isParryPrepped = false;
    }

    public void TriggerAttackString()
    {
        animator.SetTrigger("StartString1");
    }

    public void SetEnemyStun(Component sender, object data)
    {
        bool isStunned = (bool)data;
        animator.SetBool("EnemyStunned", isStunned);
    }

    public void SetCanParry(bool parry)
    {
        canParry = parry;
    }

    public void SetProjectileIsPresent(bool isPresent)
    {
        projectileIsPresent = isPresent;
    }
}
