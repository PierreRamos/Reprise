using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float parryHoldSpeed;

    [SerializeField]
    private float dashForce;

    [SerializeField]
    private float parryMovementPenaltyTime;

    [SerializeField]
    private GameEvent onPlayerPositionRequest;

    [SerializeField]
    private GameEvent onPlayerDash;

    [SerializeField]
    private GameEvent onPlayerDashFinish;

    [SerializeField]
    private GameEvent onPlayerUpdateVelocity;

    private float horizontal;

    private bool isFacingRight = true;

    private bool isBeingPushedBack;

    private bool isDashing;

    private bool onParryHoldSpeed;

    private bool isRunning_PlayerPushBackMovementPenaltyTimer;

    private Coroutine current_PlayerPushBackMovementPenaltyTimer;

    private void Update()
    {
        if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        else if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }

        //Sends velocity x when player's x velocity is not equal to zero
        if (rb.velocity.x != 0)
        {
            onPlayerUpdateVelocity.Raise(this, rb.velocity.x);
        }

        //debug
        // if (isDashing)
        // {
        //     print(rb.velocity);
        // }
    }

    private void FixedUpdate()
    {
        if (!isBeingPushedBack && !isDashing)
        {
            if (onParryHoldSpeed)
            {
                rb.velocity = new Vector3(
                    horizontal * parryHoldSpeed * Time.deltaTime,
                    rb.velocity.y,
                    0f
                );
            }
            else
            {
                rb.velocity = new Vector3(horizontal * speed * Time.deltaTime, rb.velocity.y, 0f);
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        // transform.Rotate(0f, 180f, 0f);
    }

    public void GetHorizontalInput(Component sender, object data)
    {
        horizontal = (float)data;
    }

    public void TransferPlayerPosition(Component sender, object data)
    {
        onPlayerPositionRequest.Raise(this, transform.position);
    }

    public void OnParryHoldSpeed(bool value)
    {
        onParryHoldSpeed = value;
    }

    public void PlayerDash()
    {
        if ((horizontal > 0 || horizontal < 0) && !isDashing)
        {
            isDashing = true;
            rb.AddForce(new Vector3(horizontal * dashForce, 0f, 0f), ForceMode.Impulse);
            StartCoroutine(PlayerDashTimer());
            onPlayerDash.Raise(this, null);

            IEnumerator PlayerDashTimer()
            {
                yield return new WaitUntil(() => rb.velocity.x < 2f && rb.velocity.x > -2f);
                isDashing = false;
                onPlayerDashFinish.Raise(this, null);
            }
        }
    }

    //Pushes the player back by amount force
    public void StartPlayerPushBack(float pushBackForce)
    {
        if (isRunning_PlayerPushBackMovementPenaltyTimer)
        {
            StopCoroutine(current_PlayerPushBackMovementPenaltyTimer);
        }
        current_PlayerPushBackMovementPenaltyTimer = StartCoroutine(
            PlayerPushBackMovementPenaltyTimer(pushBackForce)
        );
    }

    //Pushes back player for certain amount of time and cannot move for a while
    IEnumerator PlayerPushBackMovementPenaltyTimer(float pushBackForce)
    {
        isRunning_PlayerPushBackMovementPenaltyTimer = true;
        isBeingPushedBack = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.right * pushBackForce, ForceMode.Impulse);
        yield return new WaitForSeconds(parryMovementPenaltyTime);
        isBeingPushedBack = false;
        isRunning_PlayerPushBackMovementPenaltyTimer = false;
    }
}
