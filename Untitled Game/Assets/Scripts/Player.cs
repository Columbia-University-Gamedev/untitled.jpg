using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float jumpForce = 10f;
    private float horizontalInput;
    
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Transform tr;

    private bool canGrip = false;
    private bool grounded = true;
    public static bool tutorial1 = false;
    public static bool tutorial2 = false;
    private bool isFacingRight = true;
    private GameObject grip;
    private GameObject heldBlock;
    public GameObject feet;
    public GameObject hand;
    public GameObject front;

    private float wallSlidingSpeed = 2f;
    private bool isWallGrabbing;
    private float wallJumpingDirection;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private Animator anim;
    //Wall jumping & flipping found in this nifty tutorial: https://www.youtube.com/watch?v=O6VX6Ro7EtA

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        
    }

    private bool IsWalled()
    {
        Collider2D col = Physics2D.OverlapCircle(front.transform.position, 0.2f, LayerMask.GetMask("Grabbable"));
        return col;
    }

    private bool IsTutorial1()
    {
        Collider2D col = Physics2D.OverlapCircle(front.transform.position, 0.2f, LayerMask.GetMask("Tutorial1"));
        return col;
    }

    private bool IsTutorial2()
    {
        Collider2D col = Physics2D.OverlapCircle(front.transform.position, 0.2f, LayerMask.GetMask("Tutorial2"));
        return col;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlockGrip"))
        {
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
        tutorial1 = IsTutorial1();
        tutorial2 = IsTutorial2();
        //Debug.Log(tutorial1+","+tutorial2);
        //Player movment
        horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;
        anim.SetFloat("HSpeed", movement.magnitude);
        anim.SetBool("IsGrabbing", heldBlock != null);
        anim.SetBool("IsGrounded", grounded);
        anim.SetBool("IsWallGrabbing", isWallGrabbing);
        anim.SetFloat("VVel", rb.velocity.y);
        
        

        transform.Translate(movement);
        //regular jump
        if (Input.GetKeyDown(KeyCode.Space) && !isWallGrabbing)
        {
            Jump();
        }
        //jumping off wall
        if (Input.GetKeyDown(KeyCode.Space) && isWallGrabbing) {
            WallJump();
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

        //If we are holding onto the wall, slowly slide down.
        //If you want holding to stop player, just set sliding speed to 0. 
        if (IsWalled() && !grounded && Input.GetKey(KeyCode.E))
        {
            isWallGrabbing = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallGrabbing = false;
        }
        Flip();
    }
    void Jump()
    {
        // Apply an upward force to jump if in contact with ground
        if (grounded) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
            anim.SetTrigger("Jump");
        }
        
    }

    private void WallJump()
    {
        // Apply an upward force in opposite direction of wall player is holding onto
        wallJumpingDirection = isFacingRight ? -1 : 1;

        // Remember to flip player
        if (transform.localScale.x != wallJumpingDirection)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        rb.AddForce(new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y), ForceMode2D.Impulse);

    }

    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Die()
    {
        print("Dead!");
    }
}
