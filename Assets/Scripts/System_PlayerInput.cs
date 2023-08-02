using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class System_PlayerInput : MonoBehaviour
{
    [SerializeField]
    private GameEvent onParryAttempt;

    [SerializeField]
    private GameEvent onParryHold;

    [SerializeField]
    private GameEvent onParryHoldCancel;

    [SerializeField]
    private GameEvent onPlayerMovementInput;

    [SerializeField]
    private GameEvent onPlayerActivelyMoving;

    [SerializeField]
    private GameEvent onPlayerShootInput;

    [SerializeField]
    private GameEvent onPlayerDashInput;

    [SerializeField]
    private GameEvent onPlayerUseConsumableInput;

    private float parryHoldTime;

    private float currentHorizontalMovement;

    //Assigned by value manager when stamina is depleted
    private bool isStaggered;

    //Penalty that player cannot block immediately after shooting
    private bool playerShootPenalty;

    private bool isPerformed_PlayerBlockInput;

    private bool isPerformed_PlayerMovementInput;

    private void Update()
    {
        //Checks if player is still moving (movement key is still held) and if is not staggered
        if (isPerformed_PlayerMovementInput && !disableInput)
        {
            onPlayerActivelyMoving.Raise(this, true);
            onPlayerMovementInput.Raise(this, currentHorizontalMovement);
        }
        else
        {
            onPlayerActivelyMoving.Raise(this, false);
        }

        // Check if parry is held and also checks if player is not staggered
        if (isPerformed_PlayerBlockInput && !disableInput)
        {
            onParryHold.Raise(this, null);
        }
        else
        {
            onParryHoldCancel.Raise(this, null);
        }
    }

    //Player shoot input (tap)
    public void PlayerShoot(InputAction.CallbackContext context)
    {
        if (context.performed && !disableInput)
        {
            onPlayerShootInput.Raise(this, null);
        }
    }

    //Player dash input (tap)
    public void PlayerDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isPerformed_PlayerBlockInput)
        {
            onPlayerDashInput.Raise(this, null);
        }
    }

    //Player use consumable (tap)
    public void PlayerUseConsumable(InputAction.CallbackContext context)
    {
        if (context.performed && !disableInput)
        {
            onPlayerUseConsumableInput.Raise(this, null);
        }
    }

    //Player block input (hold)
    public void PlayerBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPerformed_PlayerBlockInput = true;
            onParryAttempt.Raise(this, null);
        }
        else if (context.canceled)
        {
            isPerformed_PlayerBlockInput = false;
        }
    }

    //Player movement input (hold)
    public void PlayerMovement(InputAction.CallbackContext context)
    {
        currentHorizontalMovement = context.ReadValue<Vector2>().x;

        //
        if (context.performed)
        {
            isPerformed_PlayerMovementInput = true;
        }
        else if (context.canceled)
        {
            isPerformed_PlayerMovementInput = false;
            onPlayerMovementInput.Raise(this, 0f);
        }
    }

    //--SET STATUS--

    private bool disableInput;

    //Enables player input
    public void EnableInput()
    {
        disableInput = false;
    }

    //Disables player input and needs to be enabled
    public void DisableInput(bool isFullStop)
    {
        if (isFullStop)
        {
            disableInput = true;
            onPlayerMovementInput.Raise(this, 0f);
        }
        else
        {
            disableInput = true;
        }
    }

    //Disables player input but within a certain time
    private bool isRunning_DisableInputTimer;

    private Coroutine current_DisableInputTimer;

    public void DisableInputTimer(float time)
    {
        disableInput = true;
        if (isRunning_DisableInputTimer)
        {
            StopCoroutine(current_DisableInputTimer);
        }
        current_DisableInputTimer = StartCoroutine(Timer());

        IEnumerator Timer()
        {
            isRunning_DisableInputTimer = true;
            yield return new WaitForSeconds(time);
            disableInput = false;
            isRunning_DisableInputTimer = false;
        }
    }

    private bool disableMovement;

    //Enables player movement
    public void EnableMovement()
    {
        disableMovement = false;
    }

    //Disables player movement and needs to be enabled
    public void DisableMovement(bool isFullStop)
    {
        if (isFullStop)
        {
            disableMovement = true;
            onPlayerMovementInput.Raise(this, 0f);
        }
        else
        {
            disableMovement = true;
        }
    }

    //Disables player movement but within a certain time
    private bool isRunning_DisableMovementTimer;

    private Coroutine current_DisableMovementTimer;

    public void DisableMovementTimer(float time)
    {
        disableMovement = true;
        if (isRunning_DisableMovementTimer)
        {
            StopCoroutine(current_DisableMovementTimer);
        }
        current_DisableMovementTimer = StartCoroutine(Timer());

        IEnumerator Timer()
        {
            isRunning_DisableMovementTimer = true;
            yield return new WaitForSeconds(time);
            disableMovement = false;
            isRunning_DisableMovementTimer = false;
        }
    }
}
