using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPatrol : Enemy
{
    public Transform wayPoint_1, wayPoint_2;
    private Transform wayPointTarget;
    private Vector2 moveDir;

    [SerializeField] private Transform currentTarget;
    [SerializeField] private EnemyRange myRange;

    protected override void Awake()
    {
        base.Awake();
        //Start by moving to first waypoint
        wayPointTarget = wayPoint_1;
        currentTarget = wayPoint_1;
    }

    protected override void Move()
    {
        //base.Move();
    }

    protected override void Update()
    {
        base.Update(); 

        if (myRange.playerInRange)
        {
            playerInRange = true; 
        }
        else
        {
            playerInRange = false; 
        }
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();

        if (playerInRange && !CannotSeePlayer())
        {
            //Debug.Log("Target is Player");

            currentTarget = target; 

        }
        else
        {
            currentTarget = wayPointTarget; 

            //Debug.Log("");

            if (Vector2.Distance(transform.position, wayPoint_1.position) < 0.5)
            {
                wayPointTarget = wayPoint_2;
            }

            if (Vector2.Distance(transform.position, wayPoint_2.position) < 0.5)
            {
                wayPointTarget = wayPoint_1;
            }
        }

        Vector2 dir = (currentTarget.position - transform.position).normalized;

        moveDir = dir;

        rb.velocity = new Vector2(moveDir.x, moveDir.y) * moveSpeed;

    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, currentTarget.position); 
    }

    protected override void DisplayHPBar()
    {
        base.DisplayHPBar();
    }

}
