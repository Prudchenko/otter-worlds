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
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
    }
   

    // Update is called once per frame
    void Update()
    {
        currentState.Execute();
        LookAtTarget();
    }
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
    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }
    public void Move()
    {
        if (!Attack)
        {

            MyAnimator.SetFloat("speed", 1);
            transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);

        }
    }
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnTriggerEnter(other);
    }
}
