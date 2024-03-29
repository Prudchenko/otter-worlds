﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void DeadEventHandler();

public class Otter : Character
{
    //Singleton pattern to access from other scripts
    private static Otter instance;

    public event DeadEventHandler Dead;

    private IUsable usable;
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
    private Transform[] headPoint;
    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private bool airControl;
    [SerializeField]
    public float jumpForce;

    private bool immortal = false;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float immortalTime;
    [SerializeField]
    private float climbSpeed;
    public Rigidbody2D MyRigidbody { get; set; }
    public bool OnLadder { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }
    public override bool IsDead
    {
        get
        {
            if (health <= 0)
            {
                OnDead();
            }
            return health <= 0;
        }
    }

    public override void Start()
    {
        base.Start();
        OnLadder = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            float vertical = Input.GetAxis("Vertical");
            //Checks if player is grounded
            OnGround = IsGrounded();
            //Moves player if possible
            HandleMovement(horizontal, vertical);
            //Flips Player if possible
            Flip(horizontal);
            //Checks which animation layer to play
            HandleLayers();

        }
    }

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }

    private void HandleMovement(float horizontal, float vertical)
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
        if (Jump && MyRigidbody.velocity.y == 0 && !OnLadder)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        if (OnLadder)
        {
            MyAnimator.speed = vertical != 0 ? Mathf.Abs(vertical) : Mathf.Abs(horizontal);
            MyRigidbody.velocity = new Vector2(horizontal * climbSpeed, vertical * climbSpeed);

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
        //Jump, dedicated to Den
        if ((Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.W)) && !Input.GetKey(KeyCode.S) && !OnLadder)
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
        //Interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            Use();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }
    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            health -= 10;
            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
            }
        }
     
    }

    public override void Death()
    {
        MyRigidbody.velocity = Vector2.zero;
        //health = 30;
        SceneManager.LoadScene(1);
    }

    private void Use()
    {
        if (usable != null)
        {
            usable.Use();
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Usable")
        {
            usable = other.GetComponent<IUsable>();
        }
        base.OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Usable")
        {
            usable = null;
        }
    }
}
