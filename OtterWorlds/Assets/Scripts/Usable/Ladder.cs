using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour,IUsable
{
    [HideInInspector]
    public GameObject gameObj;
    // Start is called before the first frame update
    void Start()
    {

        gameObj = GameObject.Find("Otter");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Use()
    {
        if (Otter.Instance.OnLadder)
        {
            //Stop climbing
            UseLadder(false, 5, 0,1,"land");
        }
        else
        {
            //Start climbing
            UseLadder(true, 0, 1,0,"reset");
            gameObj.layer = 10;
        }
    }
    private void UseLadder(bool OnLadder, int gravity, int layerWeight, int animSpeed, string trigger)
    {
        Otter.Instance.OnLadder = OnLadder;
        Otter.Instance.MyRigidbody.gravityScale = gravity;
        Otter.Instance.MyAnimator.SetLayerWeight(2, layerWeight);
        Otter.Instance.MyAnimator.speed = animSpeed;
        Otter.Instance.MyAnimator.SetTrigger(trigger);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Otter")
        {
            UseLadder(false, 5, 0,1,"land");
            gameObj.layer = 8;
        }
    }
}
