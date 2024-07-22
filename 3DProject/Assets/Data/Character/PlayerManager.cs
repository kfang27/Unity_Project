using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager playerLocomotionManager;

    protected override void Awake()
    {
        // This will run the Awake function from CharacterManager
        base.Awake();

        // Can add more functionalities from here on out

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();

        // Check if we own the gameobject and if not, we can't control/edit it
        if (!IsOwner)
            return;

        // Handle movement every frame
        playerLocomotionManager.HandleAllMovement();
    }
}
