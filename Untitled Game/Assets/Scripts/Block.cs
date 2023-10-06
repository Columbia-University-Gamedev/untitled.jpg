using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float weight = 5;//might change how this works, basically alters amount of force at throw
    // Start is called before the first frame update
    void Start()
    {
        
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
}
