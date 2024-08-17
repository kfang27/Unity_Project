using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerStats playerStats;


    public bool canDoCombo;
    public bool isInvulnerable;


    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
    }

    void Update()
    {
        inputHandler.isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        isInvulnerable = anim.GetBool("isInvulnerable");

        inputHandler.rollFlag = false;

        // can be commented out if using first implementation in HandleRollInput function of InputHandler
        inputHandler.sprintFlag = false;

    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }

        playerStats.RegenerateStamina();
    }

    private void LateUpdate()
    {

        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;
    }
}