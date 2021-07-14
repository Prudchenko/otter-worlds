using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPoint : MonoBehaviour
{

    public BoxCollider2D MyCollider { get; set; }
    // Start is called before the first frame update
    void Start()
    {

        MyCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Otter.Instance.Jump = false;
    }
}
