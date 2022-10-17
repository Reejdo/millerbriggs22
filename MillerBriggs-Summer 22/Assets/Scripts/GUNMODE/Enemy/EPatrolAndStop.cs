using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPatrolAndStop : Enemy
{
    public Transform wayPoint_1, wayPoint_2;
    private Transform wayPointTarget;
    private Vector2 moveDir;
    private Vector2 lastMoveDir; 
    [SerializeField] private float bulletSpreadRange;
    [SerializeField] private int numProjectiles;

    [SerializeField] private EnemyRange myRange;
    [SerializeField] private bool isShooting;


    protected override void Awake()
    {
        base.Awake();
        //Start by moving to first waypoint
        wayPointTarget = wayPoint_1;

        shootTimer = 0;
    }

    protected override void Move()
    {
        //base.Move();

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

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();

        if (playerInRange && !CannotSeePlayer())
        {

            rb.velocity = Vector2.zero;

            enemyAnim.SetBool("isMoving", false); 

        }
        else
        {
            //only begin to move after done shooting
            if (!isShooting && (!playerInRange || CannotSeePlayer()))
            {
                if (Vector2.Distance(transform.position, wayPoint_1.position) < 0.5)
                {
                    wayPointTarget = wayPoint_2;

                }

                if (Vector2.Distance(transform.position, wayPoint_2.position) < 0.5)
                {
                    wayPointTarget = wayPoint_1;
                }

                Vector2 dir = (wayPointTarget.position - transform.position).normalized;

                moveDir = dir;

                rb.velocity = new Vector2(moveDir.x, moveDir.y) * moveSpeed;

                enemyAnim.SetBool("isMoving", true);

                SetMoveDirection(); 

            }
        }
    }

    void IdleFollowPlayer()
    {
        Vector2 distToPlayer = new Vector2(transform.position.x - target.position.x, transform.position.y - target.position.y); 

        if (Mathf.Abs(distToPlayer.x) > Mathf.Abs(distToPlayer.y))
        {
            if (distToPlayer.x > 0.01)
            {
                enemyAnim.SetFloat("moveDirX", 1);
                enemyAnim.SetFloat("moveDirY", 0);
            }
            else if (distToPlayer.x < -0.01)
            {
                enemyAnim.SetFloat("moveDirX", -1);
                enemyAnim.SetFloat("moveDirY", 0);
            }
        }
        else
        {
            if (distToPlayer.y > 0.01)
            {
                enemyAnim.SetFloat("moveDirY", 1);
                enemyAnim.SetFloat("moveDirX", 0);
            }
            else if (distToPlayer.y < -0.01)
            {
                enemyAnim.SetFloat("moveDirY", -1);
                enemyAnim.SetFloat("moveDirX", 0);
            }
        }
    }

    void SetMoveDirection()
    {
        if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
        {
            if (moveDir.x > 0.01)
            {
                enemyAnim.SetFloat("moveDirX", 1);
                enemyAnim.SetFloat("moveDirY", 0);
            }
            else if (moveDir.x < -0.01)
            {
                enemyAnim.SetFloat("moveDirX", -1);
                enemyAnim.SetFloat("moveDirY", 0);
            }
        }
        else
        {
            if (moveDir.y > 0.01)
            {
                enemyAnim.SetFloat("moveDirY", 1);
                enemyAnim.SetFloat("moveDirX", 0);
            }
            else if (moveDir.y < -0.01)
            {
                enemyAnim.SetFloat("moveDirY", -1);
                enemyAnim.SetFloat("moveDirX", 0);
            }
        }
    }

    protected override void Attack()
    {
        //base.Attack;

        if (playerInRange && !CannotSeePlayer() && !isShooting)
        {
            //Originally was going to shoot at where player was upon entering range, 
            //but it was too easy to dodge, so just shoot at player's location
            playerSnapshot = target.transform;

            shootTimer = 0;

            //begin shoot animation when player enters range, don't stop until finished
            isShooting = true;
        }

        if (isShooting)
        {
            //Keep it facing the player while shooting
            IdleFollowPlayer();

            shootTimer += Time.deltaTime;

            if (shootTimer > shootRate)
            {

                RotateFirePointToPlayer();
                SpawnProjectiles(numProjectiles, bulletSpreadRange);
                shootTimer = 0;

                isShooting = false;
            }
        }
    }

    protected override void DisplayHPBar()
    {
        base.DisplayHPBar();
    }

}
