using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private float dashGhostEffectDelay;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject ghostEffect;

    private float horizontalVelocity;

    private float elapsedDashGhostEffectDelay;

    private bool ghostIsActive;

    private void Start()
    {
        elapsedDashGhostEffectDelay = dashGhostEffectDelay;
    }

    private void Update()
    {
        animator.SetFloat("HorizontalVelocity", horizontalVelocity);

        //Spawns ghost effect whenever player is dashing
        if (ghostIsActive)
        {
            if (elapsedDashGhostEffectDelay > 0)
            {
                elapsedDashGhostEffectDelay -= Time.deltaTime;
            }
            else
            {
                Sprite currentSprite = spriteRenderer.sprite;
                GameObject currentGhostEffect = Instantiate(
                    ghostEffect,
                    transform.position,
                    transform.rotation
                );
                currentGhostEffect.GetComponent<SpriteRenderer>().sprite = currentSprite;
                Destroy(currentGhostEffect, 0.5f);
                elapsedDashGhostEffectDelay = dashGhostEffectDelay;
            }
        }
    }

    //Trigger player hit animation except when player is stunned
    public void TriggerHitAnimation()
    {
        if (playerIsStunned == false)
        {
            animator.SetTrigger("PlayerHit");
        }
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
        animator.ResetTrigger("PlayerDashFinish");
        animator.SetTrigger("PlayerDashStart");
    }

    public void TriggerDashFinishAnimation()
    {
        animator.SetTrigger("PlayerDashFinish");
    }

    public void SetGhostIsActive(bool isActive)
    {
        ghostIsActive = isActive;
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
        animator.SetTrigger("PlayerBlocked");
    }

    public void TriggerPlayerDeflected()
    {
        animator.SetTrigger("PlayerDeflect");
    }

    public void TriggerPlayerReversal()
    {
        animator.SetTrigger("PlayerReversal");
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

    private bool playerIsStunned;

    public void TriggerPlayerStun()
    {
        animator.SetTrigger("PlayerStun");
    }

    public void SetPlayerIsStunned(bool value)
    {
        playerIsStunned = value;
        animator.SetBool("PlayerIsStunned", playerIsStunned);
    }

    public void UpdatePlayerHorizontalVelocity(Component sender, object data)
    {
        horizontalVelocity = (float)data;
    }
}
