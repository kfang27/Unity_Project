using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;

    // PlayerControls is the name of the C# class generated from the movement schematic
    PlayerControls playerControls = null;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool dodgeInput = false;


    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 cameraInput;
    public float cameraHorizontalInput;
    public float cameraVerticalInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // When scene changes, this will be executed
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // If player is loading into the world/tutorial scene (index 1), enable the player controls
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        // If it's the main menu or any other scene,
        // disable the controls to prevent character from moving in any other scene
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            
            //Movement is the name of the actions in the action maps
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // Unsubscribes from this event when object is destroyed
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
    }

    private void HandleMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        // Returns absolute number (This will make sure there won't be a negative number)
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
        if (player == null)
            return;

        // Passing 0 because we don't want strafing. Only use horizontal when strafing or locking onto target
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
    }

    private void HandleCameraMovementInput()
    {
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;
    }

    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }
}
