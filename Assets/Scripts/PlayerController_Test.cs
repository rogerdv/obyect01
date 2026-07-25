using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Test : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject bulletPref;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;

    [Header("Aim")]
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Shooting")]
    [SerializeField]
    private Vector3 bulletSpawnOffset = new Vector3(0f, 1f, 1f);


    private PlayerInput playerInput;
    private Rigidbody body;

    private Vector2 moveInput;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        // Leer input de movimiento
        moveInput =
            playerInput.actions["Move"].ReadValue<Vector2>();

        // Apuntar hacia el cursor
        AimAtMouse();

        // Disparo
        if (playerInput.actions["Attack"].WasPressedThisFrame())
        {
            Shoot();
        }
    }


    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        Vector3 movement = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        // Evita que el movimiento diagonal sea más rápido
        movement = Vector3.ClampMagnitude(
            movement,
            1f
        );

        body.MovePosition(
            body.position +
            movement *
            moveSpeed *
            Time.fixedDeltaTime
        );
    }


    private void AimAtMouse()
    {
        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            Camera.main.ScreenPointToRay(
                mousePosition
            );

        // Plano horizontal a la altura del jugador
        Plane groundPlane =
            new Plane(
                Vector3.up,
                transform.position
            );

        if (groundPlane.Raycast(
            ray,
            out float distance
        ))
        {
            Vector3 mouseWorldPosition =
                ray.GetPoint(distance);

            Vector3 aimDirection =
                mouseWorldPosition -
                transform.position;

            // Ignorar el eje vertical
            aimDirection.y = 0f;

            // Evitar problemas si el cursor está
            // exactamente sobre el jugador
            if (aimDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(
                        aimDirection,
                        Vector3.up
                    );

                model.transform.rotation =
                    Quaternion.RotateTowards(
                        model.transform.rotation,
                        targetRotation,
                        rotationSpeed *
                        Time.deltaTime
                    );
            }
        }
    }


    private void Shoot()
    {
        GameObject projectile =
            Instantiate(bulletPref);

        // Posición inicial de la bala
        projectile.transform.position =
            model.transform.TransformPoint(
                bulletSpawnOffset
            );

        // Dirección del proyectil
        projectile.GetComponent<Projectile>().dir =
            model.transform.forward;
    }
}