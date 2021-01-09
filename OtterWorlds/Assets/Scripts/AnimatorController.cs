using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator anim;
    public SpriteRenderer sprt;
    PlayerController PlayerC;
    // Start is called before the first frame update
    void Start()
    {
        PlayerC = GetComponent<PlayerController>();
        sprt = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }



    // Update is called once per frame



    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            MoveR();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveL();
        }
        else
            anim.SetBool("Walk", false);

        if (!Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if ( PlayerC.isGrounded == true && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("JumpFromOnewayPl");
        }
     
    }
    public void MoveR()
    {

        sprt.flipX = false;
        anim.SetBool("Walk", true);
    }
    public void MoveL()
    {

        sprt.flipX = true;
        anim.SetBool("Walk", true);
    }
    
    public void Jump()
    {

        anim.SetTrigger("Jump");
    }

}
