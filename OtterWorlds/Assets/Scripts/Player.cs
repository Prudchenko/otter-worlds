using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    [SerializeField]
    private float movementSpeed;
    private bool attack;
    private bool facingRight;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGrounded;
    private bool jump;
    [SerializeField]
    private bool airControl;
    [SerializeField]
    private float jumpForce;
    private bool jumpAttack;
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        //Checks if player is grounded
        isGrounded = IsGrounded();
        //Moves player if possible
        HandleMovement(horizontal);
        //Flips Player if possible
        Flip(horizontal);
        //Attack is possible
        HandleAttack();
        //Checks which animation layer to play
        HandleLayers();
        ResetValues();
    }
    private void HandleMovement(float horizontal)
    {
        //Moving mechanic and animation
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")&&(isGrounded||airControl))
        {
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        }
        //Jumping mechanic and animation
        if (isGrounded && jump)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
        }
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }
    private void HandleAttack()
    {
        //Attack animation
        if (attack&& isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("attack");
            myRigidbody.velocity = Vector2.zero;
        }
        if (jumpAttack && !isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("jumpAttack")){
            myAnimator.SetBool("jumpAttack", true);
        }
        if (!jumpAttack && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("jumpAttack"))
        {
            myAnimator.SetBool("jumpAttack", false);
        }
    }
    
    //Checks for input for special actions
    private void HandleInput()
    {
        //Upon falling, landing animation is played
        if (myRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            attack = true;
            jumpAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S))
        {
            jump = true;
        }
        if (isGrounded == true && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FallTimer());
        }
    }

    //Flipping player when moving left-right
    private void Flip (float horizontal)
    {
        if((horizontal>0&& !facingRight||horizontal < 0 && facingRight)&& !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    private void ResetValues()
    {
        attack = false;
        jump = false;
        jumpAttack = false;
    }

    //Checks if otter's paws touch something
    private bool IsGrounded()
    {
        if (myRigidbody.velocity.y <= 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for(int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        //Reseting jumping animation triggers
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);

                        return true;
                    }
                }
            }
        }
        return false;
    }
    private void HandleLayers()
    {
        //When Otter is not on ground, Air Layer is played
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
    IEnumerator FallTimer()
    {
        gameObject.layer = 10;
        myRigidbody.gravityScale += 4;
        yield return new WaitForSeconds(0.15f);
        gameObject.layer = 8;
        myRigidbody.gravityScale -= 4;
    }
}
