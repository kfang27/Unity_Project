using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    PlayerManager playerManager;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    public bool b_Input;  // For roll and sprint
    public bool rb_Input; // For light attack
    public bool rt_Input; // For heavy attack

    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;

    public float rollInputTimer;
    public bool isInteracting;

    PlayerStats playerStats;
    PlayerControls inputActions;
    CameraHandler cameraHandler;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        cameraHandler = CameraHandler.singleton;
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputActions.PlayerActions.Roll.performed += i => b_Input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        if (GetComponent<PlayerStats>().isDead)
            return; // Prevent input if dead

        MoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        // Check if the spacebar is pressed
        //b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

        //if (b_Input)
        //{
        //    rollInputTimer += delta;

        //    if (rollInputTimer > 0.5f && moveAmount > 0)
        //    {
        //        sprintFlag = true;  // Start sprinting if spacebar is held for more than 0.5 seconds and there's movement
        //        rollFlag = false;
        //    }
        //}
        //else
        //{
        //    if (rollInputTimer > 0 && rollInputTimer < 0.5f)
        //    {
        //        sprintFlag = false;
        //        rollFlag = true;  // Trigger roll if spacebar is released quickly
        //    }

        //    rollInputTimer = 0;
        //    sprintFlag = false;  // Stop sprinting if the spacebar is released
        //}

        if (b_Input)
        {
            rollInputTimer += delta;
            //sprintFlag = true;

            if (playerStats.currentStamina <= 0)
            {
                b_Input = false;
                sprintFlag = false;
            }

            if (moveAmount > 0.5f && playerStats.currentStamina > 0)
            {
                sprintFlag = true;
            }
        }
        else
        {
            sprintFlag = false;

            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                //sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleAttackInput(float delta)
    {
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;

        // RB is for RIGHT HAND weapon
        if (rb_Input)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        // RT is for left hand
        if (rt_Input)
        {
            if (isInteracting)
                return;

            if (playerManager.canDoCombo)
                return;

            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }
}
