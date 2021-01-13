using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    [SerializeField]
    private float movementSpeed;
    private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
        Flip(horizontal);
    }
    private void HandleMovement(float horizontal)
    {

        myRigidbody.velocity =  new Vector2(horizontal*movementSpeed,myRigidbody.velocity.y);
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }
    
    //Flipping player when moving left-right
    private void Flip (float horizontal)
    {
        if(horizontal>0&& !facingRight||horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
