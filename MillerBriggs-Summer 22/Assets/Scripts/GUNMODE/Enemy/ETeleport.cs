using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETeleport : Enemy
{

    [Header("Teleport Variables")]
    [SerializeField] private float teleportRate = 2.0f, deCloakRate = 1f;
    private float moveTimer, deCloakTimer; 

    [SerializeField] private float tpWaitTimeToAttack = 1f; 
    [SerializeField] private int maxAttackNum = 1;
    private int currentAttackNum = 0;
    [SerializeField] private float timeBetweenAttacks = 0f;
    private float attackTimer;

    private Vector3 teleportLocation;
    [SerializeField] private float tpAnimTime = 2f;
    [SerializeField] private float playerRadius; 

    [SerializeField] private float minRoomX, maxRoomX, minRoomY, maxRoomY;

    public GameObject chaseEnemy;
    private bool spawnChaser;
    public string attackName; 

    protected override void Update()
    {
        base.Update();

        if (begunDeath && !spawnChaser)
        {
            //Debug.Log("Spawn chaser");
            spawnChaser = true;
            GameObject chaser = Instantiate(chaseEnemy, transform.position, Quaternion.identity);
            myRoomManager.AddEnemyCount();
            chaser.GetComponent<Enemy>().myRoomManager = myRoomManager; 

        }
    }

    protected override void Move()
    {
        //Give up the base Move Function
        //base.Move();

        if (!begunDeath)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        if (moveTimer == 0)
        {
            enemyAnim.SetBool("teleport", false);
            deCloakTimer += Time.deltaTime;
        }

        if (deCloakTimer > deCloakRate && moveTimer < teleportRate)
        {
            moveTimer += Time.deltaTime;

            enemyAnim.SetBool("teleport", true);
        }

        if (moveTimer > teleportRate)
        {
            teleportLocation = new Vector3(Random.Range(minRoomX, maxRoomX), Random.Range(minRoomY, maxRoomY));
            RaycastHit2D checkTpLocation = Physics2D.CircleCast(teleportLocation, playerRadius, new Vector2(1, 1), 0, enemyDetectCollision);

            //if checkTpLocation hits then find a new location
            while (checkTpLocation)
            {
                teleportLocation = new Vector3(Random.Range(minRoomX, maxRoomX), Random.Range(minRoomY, maxRoomY));
                checkTpLocation = Physics2D.CircleCast(teleportLocation, playerRadius, new Vector2(1, 1), 0, enemyDetectCollision);
            }

            transform.position = teleportLocation;

            moveTimer = 0;
            currentAttackNum = 0;
            deCloakTimer = 0;
            enemyAnim.SetBool("teleport", false);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void Attack()
    {
        //Only attack once teleported 
        //base.Attack();

        if (moveTimer > tpWaitTimeToAttack)
        {
            if (currentAttackNum < maxAttackNum)
            {
                //Don't wait additional time for first attack after waiting tpWaitTime
                if (currentAttackNum == 0)
                {
                    SpawnProjectile(); 
                    currentAttackNum++; 
                }

                attackTimer += Time.deltaTime;

                if (attackTimer > timeBetweenAttacks)
                {
                    SpawnProjectile(); 
                    currentAttackNum++;
                    attackTimer = 0;
                }
            }
        }

    }

    void SpawnProjectile()
    {
        GameObject temp = myPool.GetPooledObject();
        temp.transform.position = transform.position;
        temp.transform.rotation = Quaternion.identity;
        temp.GetComponent<EnemyBullet>().ResetLifeTime();
        temp.SetActive(true);

        SetObjectAudioManager(temp);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue; 
        Gizmos.DrawWireSphere(transform.position, playerRadius);
    }

}
