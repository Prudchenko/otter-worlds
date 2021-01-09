using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed ;
    public float jumpForce;
    private float inputHorizontal;

    [HideInInspector]
    public Rigidbody2D rb;
    private IUsable usable;

    [HideInInspector]
    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;

    private float inputVertical;

    [HideInInspector]
    public bool onLadder;
    public float climbSpeed;

    // Start is called before the first frame update
    void Start()
    {
        onLadder = false;
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        inputHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);
    }
    private void Update()
    {
        //For multiple jumping

        if (onLadder)
        {
            inputVertical = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(inputHorizontal * climbSpeed, inputVertical * climbSpeed);
            //rb.gravityScale = 0;
        }

        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        //Jumping from a oneway platform
        if(isGrounded == true && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FallTimer());
        }

        //Jumping multiple times
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 && !Input.GetKey(KeyCode.S))
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }

        //Jumping from the ground
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true && !Input.GetKey(KeyCode.S))
        {
            Debug.Log("Ground");
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Use();
        }
    }
    IEnumerator FallTimer()
    {
        gameObject.layer = 10;
        rb.gravityScale += 4;
        yield return new WaitForSeconds(0.15f);
        gameObject.layer = 8;
        rb.gravityScale -= 4;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Usable")
        {

            usable = other.GetComponent<IUsable>();
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Usable")
        {
            usable = null;
        }
    }

    private void Use()
    {
        if (usable != null)
        {
            usable.Use();
        }
    }
}
