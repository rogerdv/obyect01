using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    public GameObject model;
    public GameObject bulletPref;

    Animator anim;
    Rigidbody body;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        if (input!=Vector2.zero) {
            anim.SetFloat("Speed", 1.0f);
            Vector3 dir = new Vector3(input.x, 0, input.y);
            model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.LookRotation(dir, Vector3.up),0.5f);
            Vector3 mov = new Vector3(input.x, 0, input.y) * Time.deltaTime;
            body.MovePosition(body.position + mov * 3.5f);
            //transform.Translate(mov * 3.5f);
        } else {
            anim.SetFloat("Speed", 0);
        }
        if (playerInput.actions["Attack"].WasPressedThisFrame()) {
            //Vector2 mPos = Mouse.current.position.ReadValue();
            //Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mPos.x, mPos.y, 20f));
            /*Ray ray = Camera.main.ScreenPointToRay(mPos);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                Vector3 worldPos = hit.point;
                model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.LookRotation(worldPos, Vector3.up), 0.5f);
            }*/
            
            var projectile = Instantiate(bulletPref);
            var offset = new Vector3(0, 1, 1);
            projectile.GetComponent<Projectile>().dir= model.transform.forward;
            
            projectile.transform.position = model.transform.TransformPoint(offset);//model.transform.position +model.transform.forward *1.5f;
            
        }
    }
}
