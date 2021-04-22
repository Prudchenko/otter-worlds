using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration=5;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }
    //Patrol. If has a target in fire range, than switch to fire state.
    public void Execute()
    {
        Patrol();

        enemy.Move();
        if (enemy.Target != null&& enemy.InFireRange)
        {
            enemy.ChangeState(new RangedState());
        }
    }

    public void Exit()
    {
    }
    //Prevents enemy from going off from the platform
    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            enemy.ChangeDirection();
        }
    }
    //Becomes idle after some time
    private void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
