using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 dir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 dir;
        transform.Translate(dir*10.0f*Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position)
    }

    private void OnTriggerEnter(Collider other)
    {        
        
        other.GetComponent<TestTarget>().TakeDamage(1);
        Destroy(gameObject);
    }
}
