﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private Transform bulletPos;
    [SerializeField]
    protected float movementSpeed;
    protected bool facingRight;
    [SerializeField]
    protected GameObject bulletPrefab;

    [SerializeField]
    protected int health;
    [SerializeField]
    private EdgeCollider2D meleeCollider;
    [SerializeField]
    private List<string> damageSources;
    public abstract bool IsDead { get; }

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }
    public Animator MyAnimator { get;private set; }
    public EdgeCollider2D MeleeCollider
    {
        get
        {
            return meleeCollider;
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public abstract IEnumerator TakeDamage();
    public abstract void Death();
    public void ChangeDirection()
    {
        facingRight = !facingRight;
        if (tag == "Otter")
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, (float)0.75, (float)0.75);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
    }
    //Instantiating a bullet
    public virtual void Fire(int value)
    {

        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(bulletPrefab, bulletPos.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            tmp.GetComponent<Bullet>().Initialize(Vector2.right);
        }
        else
        {

            GameObject tmp = (GameObject)Instantiate(bulletPrefab, bulletPos.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Bullet>().Initialize(Vector2.left);
        }
    }
    //Turn on melee collider
    public void MeleeAttack()
    {
        MeleeCollider.enabled = true;
    }

    //Checks for damage sources in vicinity
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
