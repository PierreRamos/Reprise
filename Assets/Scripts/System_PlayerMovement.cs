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
    private float parryHoldMovementSpeed;

    [SerializeField]
    private float dashForce;

    [SerializeField]
    private GameEvent onPlayerPositionRequest;

    [SerializeField]
    private GameEvent onPlayerDash;

    [SerializeField]
    private GameEvent onPlayerDashFinish;

    [SerializeField]
    private GameEvent onPlayerUpdateVelocity;

    private float horizontal;

    private bool onParryHoldSpeed;

    private void Update()
    {
        //Sends velocity x when player's x velocity is not equal to zero
        // if (rb.velocity.x != 0)
        // {
        //     onPlayerUpdateVelocity.Raise(this, rb.velocity.x);
        // }

        //Test
        onPlayerUpdateVelocity.Raise(this, rb.velocity.x);
    }

    private void FixedUpdate()
    {
        if (!isDashing && !isPushedBack)
        {
            if (onParryHoldSpeed)
            {
                rb.velocity = new Vector3(
                    horizontal * parryHoldMovementSpeed * Time.deltaTime,
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

    private bool isDashing;

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

    //Bug: trigger dash finish when dashing stops

    //Pushes the player back by amount force (Does not stop player movement)

    [SerializeField]
    private GameEvent onPlayerPushbackStart;

    [SerializeField]
    private GameEvent onPlayerPushbackFinish;

    private bool isPushedBack;

    private bool isRunning_PlayerPushBack;

    private Coroutine current_PlayerPushBack;

    public void PlayerPushBack(float pushBackForce)
    {
        isPushedBack = true;
        onPlayerPushbackStart.Raise(this, null);
        Vector3 temp = rb.velocity;
        temp.x = 0f;
        rb.velocity = temp;
        rb.AddForce(-transform.right * pushBackForce, ForceMode.Impulse);
        if (isRunning_PlayerPushBack)
        {
            StopCoroutine(current_PlayerPushBack);
        }
        current_PlayerPushBack = StartCoroutine(Condition());

        IEnumerator Condition()
        {
            isRunning_PlayerPushBack = true;
            yield return new WaitUntil(() => rb.velocity.x != 0f);
            yield return new WaitUntil(() => rb.velocity.x == 0f);
            isPushedBack = false;
            onPlayerPushbackFinish.Raise(this, null);
            isRunning_PlayerPushBack = false;
        }
    }
}
