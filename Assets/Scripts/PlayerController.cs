using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    public GameObject model;

    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
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
            transform.Translate(mov * 2);
        } else {
            anim.SetFloat("Speed", 0);
        }
        
    }
}
