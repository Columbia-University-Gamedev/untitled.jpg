using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject patrolPoint1;
    GameObject patrolPoint2;
    Vector2 point1;
    Vector2 point2;
    private float speed=0.5f;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Transform tr;
    public int enemyType;//0 is boss 1 is smaller
    public GameObject manager;
    GameObject top;
    // Start is called before the first frame update
    void Start()
    {
        Transform parentTransform = transform.parent;
        patrolPoint1 = parentTransform.GetChild(1).gameObject;
        patrolPoint2 = parentTransform.GetChild(2).gameObject;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        tr = GetComponent<Transform>();
        point1 = new Vector2(patrolPoint1.transform.position.x, patrolPoint1.transform.position.y);
        point2 = new Vector2(patrolPoint2.transform.position.x, patrolPoint2.transform.position.y);
        top = tr.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1.0f); // PingPong between 0 and 1
        Vector2 newPosition = Vector2.Lerp(point1, point2, t);
        tr.position = new Vector3(newPosition.x, newPosition.y, tr.position.z);
        if (top.GetComponent<EnemySensor>().sensed && enemyType == 1) {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitobject = collision.gameObject;
        if(hitobject.gameObject.tag == "Player" && enemyType==0)
        {
            manager.GetComponent<Manager>().Reset();
        
        }
    }
}
