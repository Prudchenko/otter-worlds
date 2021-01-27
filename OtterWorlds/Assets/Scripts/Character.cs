using System.Collections;
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
    public bool Attack { get; set; }
    public Animator MyAnimator { get;private set; }
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
    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }
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
}
