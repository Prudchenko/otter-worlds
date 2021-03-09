using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Otter : Character
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
    public bool Jump { get; set; }
    public bool OnGround { get; set; }
    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public override void Start()
    {
        base.Start();
        MyRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!TakingDamage && !IsDead)
        {
            HandleInput();
        }
    }

    void FixedUpdate()
    {
        if (!TakingDamage && !IsDead)
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
    }
    private void HandleMovement(float horizontal)
    {
        //Otter falls
        if (MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }
        //Idling/running
        if (!Attack && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }
        //Jumping
        if (Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }
    //Checks for input for special actions
    private void HandleInput()
    {
               //Attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MyAnimator.SetTrigger("attack");
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S))
        {
            MyAnimator.SetTrigger("jump");
        }
        //Jump from oneway platform
        if (OnGround == true && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FallTimer());
        }
        //Fire
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MyAnimator.SetTrigger("fire");
        }
    }

    //Flipping player when moving left-right
    private void Flip (float horizontal)
    {
        if((horizontal>0&& !facingRight||horizontal < 0 && facingRight)&& !this.MyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            ChangeDirection(); 
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
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
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
    public override void Fire(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            base.Fire(value);
        }
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;
        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
            MyAnimator.SetTrigger("die");
        }
        yield return null;
    }
}
