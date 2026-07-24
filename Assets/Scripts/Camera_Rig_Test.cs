using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRig_Test : MonoBehaviour
{
    [Header("Objects")]
    public Transform target;
    public GameObject camObject;

    private Camera cam;
    private PlayerInput playerInput;

    [Header("Dead Zone")]
    public float deadZoneWidth = 4f;
    public float deadZoneHeight = 3f;

    [Header("Movement")]
    public float smoothSpeed = 8f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float zoomSmoothSpeed = 10f;

    public float minFOV = 30f;
    public float maxFOV = 60f;

    [Tooltip("How much the camera moves toward the player during zoom.")]
    public float zoomMovementStrength = 0.5f;

    private float targetFOV;


    private void Awake()
    {
        cam = camObject.GetComponent<Camera>();
        playerInput = target.GetComponent<PlayerInput>();

        targetFOV = cam.fieldOfView;
    }


    private void LateUpdate()
    {
        if (target == null)
            return;

        HandleZoom();
        HandleCameraMovement();
    }


    private void HandleZoom()
    {
        float scroll =
            playerInput.actions["Zoom"].ReadValue<float>();


        // Change the desired FOV
        if (scroll != 0f)
        {
            targetFOV -= scroll * zoomSpeed;

            targetFOV = Mathf.Clamp(
                targetFOV,
                minFOV,
                maxFOV
            );
        }


        // Smoothly change the FOV
        float previousFOV = cam.fieldOfView;

        cam.fieldOfView = Mathf.Lerp(
            cam.fieldOfView,
            targetFOV,
            zoomSmoothSpeed * Time.deltaTime
        );


        // How much the FOV changed this frame
        float fovDifference =
            previousFOV - cam.fieldOfView;


        if (Mathf.Approximately(fovDifference, 0f))
            return;


        // Horizontal direction from the camera to the player
        Vector3 directionToTarget =
            target.position - transform.position;

        directionToTarget.y = 0f;

        directionToTarget.Normalize();


        // Zoom in  -> move towards player
        // Zoom out -> move away from player
        transform.position +=
            directionToTarget *
            fovDifference *
            zoomMovementStrength;
    }


    private void HandleCameraMovement()
    {
        Vector3 desiredPosition = transform.position;


        float left =
            transform.position.x - deadZoneWidth * 0.5f;

        float right =
            transform.position.x + deadZoneWidth * 0.5f;

        float bottom =
            transform.position.z - deadZoneHeight * 0.5f;

        float top =
            transform.position.z + deadZoneHeight * 0.5f;


        if (target.position.x < left)
        {
            desiredPosition.x +=
                target.position.x - left;
        }

        if (target.position.x > right)
        {
            desiredPosition.x +=
                target.position.x - right;
        }

        if (target.position.z < bottom)
        {
            desiredPosition.z +=
                target.position.z - bottom;
        }

        if (target.position.z > top)
        {
            desiredPosition.z +=
                target.position.z - top;
        }


        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(
                deadZoneWidth,
                0.1f,
                deadZoneHeight
            )
        );
    }
}