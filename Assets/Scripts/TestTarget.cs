using UnityEngine;

public class TestTarget : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        Debug.Log("I received a hit");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
