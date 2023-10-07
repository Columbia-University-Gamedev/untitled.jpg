using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Transform tr;

    private bool canGrip = false;
    private bool grounded = true;
    private GameObject grip;
    private GameObject heldBlock;
    public GameObject feet;
    public GameObject hand;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        tr = GetComponent<Transform>();
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlockGrip")) {
            canGrip = true;//using bool because using input with physics can cause issues
            grip = other.gameObject;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Changes to add throwable interface
        if (other.gameObject.CompareTag("BlockGrip"))
        {
            canGrip = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        grounded = feet.GetComponent<FootSensor>().sensed;
        //Player movment
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (canGrip) {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(grip.GetComponentInParent<Throwable>() != null)
                {
                    heldBlock = grip.gameObject.transform.parent.gameObject;
                }
                
            }
        }

        //If we are holding block 
        if (heldBlock != null) {
            if (Input.GetMouseButtonDown(0))    
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mousePos2D = new Vector3(mousePos.x, mousePos.y,0);
                heldBlock.GetComponent<Throwable>().Throw(mousePos2D);
                heldBlock= null; // Reset the heldBlock reference since it's no longer held
            }
            else {
                Vector3 handPos = hand.transform.position;
                heldBlock.transform.position = handPos;
            }
            
        }
    }
    void Jump()
    {
        // Apply an upward force to jump if in contact with ground
        if (grounded) {rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); }
        
    }

    
    
 
}
