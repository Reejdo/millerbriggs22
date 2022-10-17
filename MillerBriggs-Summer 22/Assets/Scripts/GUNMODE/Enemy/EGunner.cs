using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGunner : Enemy
{
    [Header("SPECIFIC VARS")]

    public Sounds miniGun; 

    [SerializeField] private EnemyRange myRange, myCloseRange;
    [SerializeField] private bool isShooting, playerInClose;

    private Vector2 moveDir;
    private Vector2 lastMoveDir;
    private Vector2 paceLocation_1, paceLocation_2, wayPointTarget;
    [SerializeField] private float paceDistance;
    [SerializeField] private float paceSpeed = 1.5f;
    private bool beginPacing, canPace;

    protected override void Awake()
    {
        base.Awake();

        shootTimer = 0;
    }

    protected override void Update()
    {
        base.Update();
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

        if (myCloseRange.playerInRange)
        {
            playerInClose = true;
        }
        else
        {
            playerInClose = false;
        }
    }


    protected override void FixedUpdate()
    {
        if (!begunDeath)
        {

            if (playerInRange && !CannotSeePlayer() && !playerInClose && !isShooting)
            {

                //Debug.Log("Move"); 

                beginPacing = false;

                playerDirection = (target.position - transform.position).normalized;

                moveDir = playerDirection;

                rb.velocity = new Vector2(moveDir.x, moveDir.y) * moveSpeed;

                enemyAnim.SetBool("isMoving", true);

                SetMoveDirection();
            }
            else if (playerInClose && !CannotSeePlayer() && !isShooting)
            {
                enemyAnim.SetBool("isMoving", false);
                isShooting = true;
                beginPacing = false;
            }

            //start pacing with set spots from where the enemy is located
            else if (!playerInRange && !isShooting && !beginPacing)
            {
                beginPacing = true;
                DeterminePaceLocations();
            }

            //if enemy can't pace don't tyr to make them move
            if (beginPacing && canPace)
            {
                enemyAnim.SetBool("isMoving", true);
                SetPaceDirection();


                if (Vector2.Distance((Vector2)transform.position, paceLocation_1) < 0.5)
                {
                    wayPointTarget = paceLocation_2;
                }

                if (Vector2.Distance((Vector2)transform.position, paceLocation_2) < 0.5)
                {
                    wayPointTarget = paceLocation_1;
                }

                Vector2 dir = (wayPointTarget - (Vector2)transform.position).normalized;

                moveDir = dir;

                rb.velocity = new Vector2(moveDir.x, moveDir.y) * paceSpeed;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        //base.FixedUpdate();

    }


    protected override void Attack()
    {
        //base.Attack();

        if (playerInClose && !CannotSeePlayer() && !isShooting)
        {
            shootTimer = 0;

            //begin shoot animation when player enters range, don't stop until finished
            isShooting = true;
        }

        if (!playerInClose || CannotSeePlayer())
        {
            isShooting = false;
            enemyAnim.SetBool("isShooting", false);
            Debug.Log("Stop shooting"); 
        }

        if (isShooting)
        {

            rb.velocity = Vector2.zero;
            moveDir = Vector2.zero;
            enemyAnim.SetBool("isMoving", false);
            enemyAnim.SetBool("isShooting", true); 

            shootTimer += Time.deltaTime;

            if (shootTimer > shootRate)
            {
                GameObject temp = myPool.GetPooledObject();
                temp.transform.position = firePoint.position;
                temp.SetActive(true);

                temp.GetComponent<EnemyBullet>().RotateToPlayer(); 

                shootTimer = 0;

            }
        }
        else
        {
            shootTimer = 0;
        }

    }

    void DeterminePaceLocations()
    {
        RaycastHit2D rayHitUp = Physics2D.Raycast(transform.position, transform.up, paceDistance, enemyDetectCollision);
        RaycastHit2D rayHitDown = Physics2D.Raycast(transform.position, -transform.up, paceDistance, enemyDetectCollision);

        if (!rayHitUp)
        {
            paceLocation_1 = new Vector2(transform.position.x, transform.position.y + paceDistance);
            canPace = true;

            if (rayHitDown)
            {
                paceLocation_2 = new Vector2(transform.position.x, transform.position.y);
            }

            wayPointTarget = paceLocation_1;
        }

        if (!rayHitDown)
        {
            paceLocation_2 = new Vector2(transform.position.x, transform.position.y - paceDistance);
            canPace = true;
            wayPointTarget = paceLocation_1;

            if (rayHitUp)
            {
                paceLocation_1 = new Vector2(transform.position.x, transform.position.y);
            }
        }

        else if (rayHitDown && rayHitUp)
        {
            canPace = false;
        }
    }

    void SetMoveDirection()
    {
        if (target.transform.position.y > transform.position.y)
        {
            enemyAnim.SetFloat("moveDirY", 1);
        }
        else
        {
            enemyAnim.SetFloat("moveDirY", -1);
        }
    }

    void SetPaceDirection()
    {
        if (wayPointTarget.y > transform.position.y)
        {
            enemyAnim.SetFloat("moveDirY", 1);
        }
        else
        {
            enemyAnim.SetFloat("moveDirY", -1);
        }
    }

    protected override void DisplayHPBar()
    {
        base.DisplayHPBar();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Debug.DrawRay(transform.position, transform.up * paceDistance, Color.magenta);
        Debug.DrawRay(transform.position, -transform.up * paceDistance, Color.magenta);
    }

}
