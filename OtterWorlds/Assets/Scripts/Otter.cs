﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Otter : MonoBehaviour
{
    //Singleton pattern to access from other scripts
    private static Otter instance;
    public static Otter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Otter>();
            }
            return instance;
        }
    }

    private Animator myAnimator;
    [SerializeField]
    private float movementSpeed;
    private bool facingRight;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private bool airControl;
    [SerializeField]
    private float jumpForce;
    public Rigidbody2D MyRigidbody { get; set; }
    public bool Attack { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        MyRigidbody = GetComponent<Rigidbody2D>();
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
        OnGround = IsGrounded();
        //Moves player if possible
        HandleMovement(horizontal);
        //Flips Player if possible
        Flip(horizontal);
        //Checks which animation layer to play
        HandleLayers();
    }
    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }
        if (!Attack && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }
        if (Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }
    //Checks for input for special actions
    private void HandleInput()
    {
        //Upon falling, landing animation is played
        if (MyRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            myAnimator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S))
        {
            myAnimator.SetTrigger("jump");
        }
        if (OnGround == true && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
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

    //Checks if otter's paws touch something
    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for(int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
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
        if (!OnGround)
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
        MyRigidbody.gravityScale += 4;
        yield return new WaitForSeconds(0.15f);
        gameObject.layer = 8;
        MyRigidbody.gravityScale -= 4;
    }
}
