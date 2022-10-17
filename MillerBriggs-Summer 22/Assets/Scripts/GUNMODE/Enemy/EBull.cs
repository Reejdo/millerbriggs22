using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBull : Enemy
{
    [Header("Bull Variables")]

    public Transform[] bullFirePoints; 
    public Transform wayPoint_1, wayPoint_2;
    private Transform wayPointTarget;
    private Vector2 moveDir;
    private Vector2 lastMoveDir;
    private AudioManager myAudioManager;

    private float startMoveSpeed;  
    [SerializeField] private float chargeSpeed; 
    [SerializeField] private float timeToRev, timeBeingDazed;
    private float revTimer, dazedTimer, chargeTimer; 
    [SerializeField] private bool isRevving, isCharging, isDazed;
    public int wallLayerNumber = 10, playerLayerNumber = 7; 
    public EnemyRange myEnemyRange;

    protected override void Awake()
    {
        //base.Awake();
        //Start by moving to first waypoint
        wayPointTarget = wayPoint_1;
        startMoveSpeed = moveSpeed; 
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (myEnemyRange.playerInRange)
        {
            playerInRange = true; 
        }
        else
        {
            playerInRange = false; 
        }

        AnimationHandle(); 
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();

        if (!isRevving && !isCharging && !isDazed)
        {
            moveSpeed = startMoveSpeed; 

            Debug.Log("Not revving or charging or dazed"); 

            if (!playerInRange || CannotSeePlayer())
            {
                RotateTowardsTarget(wayPointTarget); 

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

            }
        }

        if (!isDazed)
        {
            rb.velocity = new Vector2(moveDir.x, moveDir.y) * moveSpeed;
        }
        else if (isDazed)
        {
            rb.velocity = Vector2.zero;
        }

    }


    protected override void Attack()
    {
        //base.Attack();
        if (playerInRange && !CannotSeePlayer())
        {
            //Not making any action currently, start revving
            if (!isRevving && !isCharging && !isDazed)
            {
                isRevving = true;
                rb.velocity = Vector2.zero;
                moveDir = Vector2.zero;
            }
        }

        if (isRevving && timeToRev > revTimer)
        {
            RotateTowardsTarget(target); 
            Debug.Log("Revving");
            revTimer += Time.deltaTime;
            rb.velocity = Vector2.zero;
            moveDir = Vector2.zero;
        }

        //Done revving, turn towards player and charge
        if (isRevving && revTimer > timeToRev)
        {
            Debug.Log("Done revving");

            isCharging = true;
            isRevving = false;
            revTimer = 0;  
            playerSnapshot = target;
            ChargeTowardsPlayer(); 
        }

        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            SpawnProjectiles();
        }


        else if (isDazed)
        {
            chargeTimer = 0; 

            dazedTimer += Time.deltaTime; 

            if (dazedTimer > timeBeingDazed)
            {
                dazedTimer = 0;
                isDazed = false; 
            }
        }

    }

    void RotateTowardsTarget(Transform myTarget)
    {
        Vector2 rotation = transform.position - myTarget.position;

        //Debug.Log("velocity: " + rb.velocity); 

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    void ChargeTowardsPlayer()
    {
        RotateTowardsTarget(playerSnapshot); 
        moveDir = (playerSnapshot.position - transform.position).normalized;

        moveSpeed = chargeSpeed; 

        //Spawn bullets to the left and right of the bull over the course of its charge
    }

    void SpawnProjectiles()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer > shootRate)
        { 
            for (int i = 0; i < bullFirePoints.Length; i++)
            {
                GameObject temp = myPool.GetPooledObject();
                BulletSetUp(temp, bullFirePoints[i]); 
            }
            shootTimer = 0;
        }
    }

    void BulletSetUp(GameObject temp, Transform firePoint)
    {
        temp.transform.position = firePoint.position;
        temp.transform.rotation = firePoint.rotation;
        Rigidbody2D tempRB = temp.GetComponent<Rigidbody2D>();
        tempRB.velocity = tempRB.transform.up * temp.GetComponent<EnemyBullet>().moveSpeed;
        temp.GetComponent<EnemyBullet>().ResetLifeTime();

        temp.gameObject.SetActive(true);
        SetObjectAudioManager(temp);
        
        Debug.Log("Spawned: " + temp.gameObject.name + temp.gameObject.activeInHierarchy);
    }


    void AnimationHandle()
    {
        enemyAnim.SetBool("isRevving", isRevving);
        enemyAnim.SetBool("isCharging", isCharging);
        enemyAnim.SetBool("isDazed", isDazed); 
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        var layerMask = collision.gameObject.layer; 

        if (chargeTimer > 0.05f)
        {
            if (layerMask == wallLayerNumber)
            {
                isCharging = false;
                isDazed = true;
            }

            else if (layerMask == playerLayerNumber)
            {
                //player get knocked back and take damage
                isCharging = false;
                isDazed = true;

                PGunMovement myPMove = collision.gameObject.GetComponentInParent<PGunMovement>();

                if (myPMove == null)
                {
                    Debug.LogError("PGunMove not found!!");
                }

                myPMove.PlayerKnockBack(gameObject.transform);

                //Player damage here
            }
        }
    }
}
