using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour,IUsable
{
    [HideInInspector]
    public PlayerController plCo;
    [HideInInspector]
    public GameObject gameObj;
    // Start is called before the first frame update
    void Start()
    {
        gameObj = GameObject.Find("Otter");
        plCo = gameObj.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Use()
    {
        if (plCo.onLadder)
        {
            //Stop climbing
            UseLadder(false,5);
        }
        else
        {
            //Start climbing
            UseLadder(true,0);
            gameObj.layer = 10;
        }
    }
    private void UseLadder(bool OnLadder, int gravity)
    {
        plCo.onLadder = OnLadder;
        plCo.rb.gravityScale = gravity;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Otter")
        {
            UseLadder(false, 5);
            gameObj.layer = 8;
        }
    }
}
