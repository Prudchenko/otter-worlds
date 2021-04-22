using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState
{
    private Enemy enemy;
    private float fireTimer;
    private float fireCooldown=5;
    private bool canFire=true;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Fire();
        //Switch to melee if Player is near

        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
        //Move to Player if he is too far
        else if (enemy.Target != null)
        {
            enemy.Move();
        }
        //Switch to idle if the target was lost
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
    }
    //Mehanic of reloading ranged weapon
    private void Fire()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireCooldown)
        {
            canFire = true;
            fireTimer = 0;
        }
        if (canFire)
        {
            canFire = false;
            enemy.MyAnimator.SetTrigger("fire");
        }
    }
}
