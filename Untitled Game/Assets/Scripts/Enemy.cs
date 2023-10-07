using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    BoxCollider2D bc;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitobject = collision.gameObject;
        if(hitobject.tag == "Block")
        {
            Destroy(gameObject);
        }
        if(hitobject.gameObject.tag == "Player")
        {
            hitobject.GetComponent<Player>().Die();
        }
    }
}
