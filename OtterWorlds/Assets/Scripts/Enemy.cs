using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private IEnemyState currentState;
    public GameObject Target { get; set; }
    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private float fireRange;
    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;


    //Checks if target is close enough to hit in melee
    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }

    //Checks if target is close enough to hit in range
    public bool InFireRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= fireRange;
            }
            return false;
        }
    }
    //Checks if an object has less than 1 health
    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }
    //Always start idle, facing right
    public override void Start()
    {
        base.Start();
        Otter.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
    }
   
    void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
    }

    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }

    //Change direction when target moved behind 
    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }

        }
    }
    //Changing current state to new state
    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }
    //Set animation and sprite to moving if not attacking
    public void Move()
    {
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x)|| (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                MyAnimator.SetFloat("speed", 1);
                transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
            }
            else if(currentState is PatrolState)
            {
                ChangeDirection();
            }
            else if (currentState is RangedState)
            {
                Target = null;
                ChangeState(new IdleState());
            }
        }
    }
    //Returns vector which informs where enemy is facing
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }
    //Calls for Character OnTriggerEnter2D function and current state OnTriggerEnter2D
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }
    //Behaviour when hit by a damage source
    public override IEnumerator TakeDamage()
    {
        health -= 10;
        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
    }
}
