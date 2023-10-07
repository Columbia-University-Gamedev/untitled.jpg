using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour,Throwable
{

    Rigidbody2D rb;
    public float weight = 5;//might change how this works, basically alters amount of force at throw
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer==6)
        {
            Destroy(other.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Throwable.Throw(Vector3 mp)
    {
        Debug.Log("Thrown");
        Vector3 throwDirection = (mp - transform.position).normalized;
        float throwForce = 100.0f / weight; // Adjust this value to control the throw force
        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        print(throwDirection * throwForce);
        
        
    }
}
