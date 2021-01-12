﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private float movementSpeed;
    private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
    }
    private void HandleMovement(float horizontal)
    {

        myRigidbody.velocity =  new Vector2(horizontal*movementSpeed,myRigidbody.velocity.y);

    }
    private void Flip (float horizontal)
    {

    }
}