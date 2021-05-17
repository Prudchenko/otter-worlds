using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration;
    public void Enter(Enemy enemy)
    {
        patrolDuration = UnityEngine.Random.Range(2, 10);
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
    //Choose Player as target if he shoot at enemy
    public void OnTriggerEnter(Collider2D other)
    {
        
        if (other.tag == "Bullet")
        {
            enemy.Target = Otter.Instance.gameObject;
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
