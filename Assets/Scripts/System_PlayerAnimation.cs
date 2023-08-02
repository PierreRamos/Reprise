using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float horizontalVelocity;

    private void Update()
    {
        animator.SetFloat("HorizontalVelocity", horizontalVelocity);
    }

    public void TriggerHitAnimation()
    {
        int randomNumber = Random.Range(1, 3);
        animator.SetTrigger($"PlayerHit{randomNumber}");
    }

    public void TriggerPlayerHealthPotion()
    {
        animator.SetTrigger("PlayerHealthPotion");
    }

    public void TriggerEmpoweredBulletCastAnimation()
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        animator.SetTrigger("PlayerEmpoweredCast");
    }

    public void TriggerEmpoweredBulletShootAnimation()
    {
        animator.updateMode = AnimatorUpdateMode.Normal;
        animator.SetTrigger("PlayerEmpoweredShoot");
    }

    public void TriggerDashStartAnimation()
    {
        animator.SetTrigger("PlayerDashStart");
    }

    public void TriggerPlayerCastShoot()
    {
        animator.SetTrigger("PlayerCastShoot");
    }

    public void TriggerPlayerShoot()
    {
        animator.SetTrigger("PlayerShoot");
    }

    public void TriggerPlayerBlocked()
    {
        int randomNumber = Random.Range(1, 3);
        animator.SetTrigger($"PlayerBlocked{randomNumber}");
    }

    public void TriggerPlayerDeflected()
    {
        int randomNumber = Random.Range(1, 3);
        animator.SetTrigger($"PlayerDeflected{randomNumber}");
    }

    public void SetPlayerIsBlocking(bool value)
    {
        animator.SetBool("PlayerIsBlocking", value);
    }

    public void SetPlayerIsMoving(Component sender, object data)
    {
        bool isMoving = (bool)data;
        animator.SetBool("PlayerIsMoving", isMoving);
    }

    public void UpdatePlayerHorizontalVelocity(Component sender, object data)
    {
        horizontalVelocity = (float)data;
    }
}
