using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_BulletCounterTime : MonoBehaviour
{
    [SerializeField]
    private GameEvent onConfirmPerfectParryHit;

    [SerializeField]
    private GameEvent onConfirmParryHit;

    [SerializeField]
    private GameEvent onUnblockableReversal;

    [SerializeField]
    private float parryWindowStart;

    [SerializeField]
    private float perfectParryWindowStart;

    private float currentTime;

    private bool isBlockable;

    private bool canPerfectParry;

    private bool playerIsDashing;

    public bool parryIsHeld { private get; set; }

    private void Start()
    {
        currentTime = 0;
    }

    private void Update()
    {
        currentTime += 1 * Time.deltaTime;
        if (currentTime > parryWindowStart)
        {
            isBlockable = true;
            if (currentTime > perfectParryWindowStart)
            {
                canPerfectParry = true;
            }
        }
        else
        {
            isBlockable = false;
        }
        if (parryIsHeld || playerIsDashing)
        {
            ConfirmParryHit(this, transform.position);
        }
    }

    //Gets called when player dashes and when it finishes
    public void PlayerIsDashing(bool isDashing)
    {
        playerIsDashing = isDashing;
    }

    //Gets called when player inputs parry
    public void ConfirmParryHit(Component sender, object data)
    {
        //If attack is blockable
        if (gameObject.CompareTag("Bullet"))
        {
            if (isBlockable && canPerfectParry)
            {
                onConfirmPerfectParryHit.Raise(this, transform.position);
            }
            else if (isBlockable)
            {
                onConfirmParryHit.Raise(this, transform.position);
            }
        }
        //If attack is unblockable
        else if (gameObject.CompareTag("UnblockableAttack"))
        {
            if (playerIsDashing == true && isBlockable)
            {
                onUnblockableReversal.Raise(this, null);
            }
        }
    }
}
