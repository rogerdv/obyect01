using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRig : MonoBehaviour
{
    [Header("Objects")]
    public Transform target;
    public GameObject camObject;
    
    Camera cam;
    PlayerInput playerInput;

    [Header("Dead Zone")]
    public float deadZoneWidth = 4f;
    public float deadZoneHeight = 3f;

    [Header("Movement")]
    public float smoothSpeed = 8f;


    void Start()
    {
        //Get the PlayerInput from the player object
        playerInput = target.GetComponent<PlayerInput>();   
        //Get the camera component
        cam = camObject.GetComponent<Camera>();
    }

    void Update()
    {        
       
        
    }

    void LateUpdate()
    {
        if (target == null)
            return;
        float scroll = playerInput.actions["Zoom"].ReadValue<float>();
        //if (scroll == 0)
          //  return;
        cam.fieldOfView += scroll * -10 * Time.deltaTime;

        Vector3 desired = transform.position;

        float left = transform.position.x - deadZoneWidth * 0.5f;
        float right = transform.position.x + deadZoneWidth * 0.5f;

        float bottom = transform.position.z - deadZoneHeight * 0.5f;
        float top = transform.position.z + deadZoneHeight * 0.5f;

        if (target.position.x < left)
            desired.x += target.position.x - left;

        if (target.position.x > right)
            desired.x += target.position.x - right;

        if (target.position.z < bottom)
            desired.z += target.position.z - bottom;

        if (target.position.z > top)
            desired.z += target.position.z - top;

        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            smoothSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(deadZoneWidth, 0.1f, deadZoneHeight));
    }
}