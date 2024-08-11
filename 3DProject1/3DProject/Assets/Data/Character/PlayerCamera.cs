using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public Camera cameraObject;
    public PlayerManager player;
    [SerializeField] Transform cameraPivotTransform;  // Pivot for vertical rotation

    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1f;
    [SerializeField] public float leftAndRightRotationSpeed = 100f;
    [SerializeField] public float upAndDownRotationSpeed = 100f;
    [SerializeField] float minimumPivot = -10f; // Lowest point you can look down
    [SerializeField] float maximumPivot = 20f;  // Highest point you can look up
    [SerializeField] float minLeftAndRightAngle = -90f; // Minimum horizontal angle
    [SerializeField] float maxLeftAndRightAngle = 90f;  // Maximum horizontal angle

    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; // for camera collisions
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;    // for camera collision
    private float targetCameraZPosition;     // for camera collision

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0f, 2.5f, -5f);  // Offset for third-person view

    private void Awake()
    {
        Debug.Log(PlayerPrefs.GetInt("masterSens"));

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
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(
            transform.position,
            player.transform.position + offset,
            ref cameraVelocity,
            cameraSmoothSpeed * Time.deltaTime
        );

        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        // Update rotation angles
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

        // Clamp the angles
        leftAndRightLookAngle = Mathf.Clamp(leftAndRightLookAngle, minLeftAndRightAngle, maxLeftAndRightAngle);
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        // Apply the rotations
        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // Handle horizontal rotation (left and right)
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;  // Apply horizontal rotation to the camera

        // Handle vertical rotation (up and down)
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;  // Apply vertical rotation to the pivot
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}