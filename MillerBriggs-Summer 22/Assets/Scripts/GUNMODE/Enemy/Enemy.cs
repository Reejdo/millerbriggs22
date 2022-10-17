using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private protected float moveSpeed;
    [SerializeField] private float healthPoint;
    [SerializeField] private float maxHealthPoint;

    public Transform target;
    [SerializeField] private protected float simpleRange;

    [SerializeField] private SpriteRenderer sR;

    [SerializeField] protected float shootRate = 2f;
    protected float shootTimer;
    public GameObject projectile;
    public Transform firePoint; 

    [SerializeField] protected private bool playerInRange;
    public Animator exclaimAnim;
    [SerializeField] protected Animator enemyAnim;

    protected bool begunDeath;

    public Image hpImage; //Regular bar
    public Image hpEffectImage; //Bar when being hurt 
    public GameObject deathParticle; 

    protected Rigidbody2D rb;
    protected Vector2 moveDirection;
    protected Vector3 playerDirection;
    protected Transform playerSnapshot;

    public LayerMask enemyDetectCollision;

    [Header("ROOM MANAGER")]
    public RoomManager myRoomManager;

    [Header("ITEM DROPS")]
    public DropItem[] dropItems;

    private AudioManager myAudioManager;
    public Sounds myHurtSound, myDeathSound, dropItemSound;

    protected EnemyBulletPool myPool;
    public GameObject myBulletPool;
    //public EnemyRange myEnemyRange; 

    protected virtual void Awake()
    {
        myRoomManager.AddEnemyCount(); 
    }

    private void Start()
    {
        healthPoint = maxHealthPoint;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        GameObject temp = Instantiate(myBulletPool, transform.position, Quaternion.identity);
        myPool = temp.GetComponent<EnemyBulletPool>();
        myPool.PoolObjects(projectile); 
    }

    protected virtual void Update()
    {
        if (myAudioManager == null)
        {
            myAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>(); 
        }

        //FlipDirection(); 

        if (healthPoint <= 0 && !begunDeath)
        {
            myRoomManager.SubtractEnemyCount(); 
            begunDeath = true;
            StartCoroutine(Death()); 
        }

        if (!begunDeath)
        {
            Attack();
            Move();
        }

        DisplayHPBar(); //Remove when adding hurt method

    }

    protected virtual void Move()
    {
        if (Vector2.Distance(transform.position, target.position) < simpleRange)
        {
            playerInRange = true;
            playerDirection = (target.position - transform.position).normalized;

            moveDirection = playerDirection;

            //rb.velocity = Vector2.ClampMagnitude(rb.velocity + new Vector2(dir.x * moveSpeed * Time.deltaTime, dir.y * moveSpeed * Time.deltaTime), 20);
            //transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            playerInRange = false;
        }
    }


    protected virtual void FixedUpdate()
    {
        if (playerInRange && !begunDeath)
        {
            if (!CannotSeePlayer())
            {
                rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            }
            else
            {
                rb.velocity = new Vector2(0, 0); 
            }
        }
    }

    public bool CannotSeePlayer()
    {
        Vector2 sightLineDirection = target.position - gameObject.transform.position;

        float dist = Vector2.Distance(transform.position, target.position);

        return Physics2D.Raycast(transform.position, sightLineDirection, dist, enemyDetectCollision);



    }

    protected virtual void FlipDirection()
    {
        if (transform.position.x > target.position.x)
        {
            sR.flipX = true; 
        }
        else
        {
            sR.flipX = false; 
        }
    }

    protected virtual void Attack()
    {

        if (playerInRange)
        {
            //Debug.Log("Attack!!!!"); 

            shootTimer += Time.deltaTime;

            if (shootTimer > shootRate)
            {
                //GameObject temp = Instantiate(projectile, firePoint.position, Quaternion.identity);
                GameObject temp = myPool.GetPooledObject();
                temp.transform.position = firePoint.position;

                temp.GetComponent<EnemyBullet>().RotateToPlayer();
                temp.GetComponent<EnemyBullet>().ResetLifeTime();

                temp.SetActive(true);

                SetObjectAudioManager(temp); 

                shootTimer = 0;
            }
        }
        else
        {
            shootTimer = 0; 
        }
    }

    protected void SetObjectAudioManager(GameObject temp)
    {
        if (temp.GetComponent<EnemyBullet>() == null)
        {
            temp.GetComponent<Bullet>().myAudioManager = myAudioManager;
        }
        else
        {
            temp.GetComponent<EnemyBullet>().myAudioManager = myAudioManager;
        }
    }

    protected virtual IEnumerator Death()
    {
        //NEEDS: ANIMATION, DEATH EFFECT
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        enemyAnim.SetTrigger("death");

        myAudioManager.Play(myDeathSound);
        myPool.DeleteObjectPool(); 

        Instantiate(deathParticle, transform.position, Quaternion.identity);

        SpawnItems(); 

        yield return new WaitForSeconds(1f); 

        Destroy(gameObject); 
    }

    protected virtual void DisplayHPBar()
    {
        hpImage.fillAmount = healthPoint / maxHealthPoint; 

        //have red go down first, then white for visual effect
        if (hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= 0.005f; //Delay effect
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount; 
        }

    }

    public void InjureEnemy(int damageToTake)
    {
        //Debug.Log("Enemy Injured"); 

        healthPoint -= damageToTake;

        myAudioManager.Play(myHurtSound); 
    }

    //chance spawn items upon death
    void SpawnItems()
    {
        int itemDropped = 0;

        foreach (DropItem item in dropItems)
        {
            float chance = Random.Range(0f, 100f);

            //Ex: if drop rate is 10%, it needs to be a 10 or lower to spawn
            if (chance <= item.dropRate)
            {
                Instantiate(item.dropObject, RandomItemSpawn(), Quaternion.identity);
                itemDropped++; 
            }
        }

        if (itemDropped > 0)
        {
            myAudioManager.Play(dropItemSound); 
        }
    }

    Vector2 RandomItemSpawn()
    {
        float diff = 0.5f;

        float newX = Random.Range(-diff, diff);
        float newY = Random.Range(-diff, diff);
        Vector2 newPosition = new Vector2(transform.position.x + newX, transform.position.y + newY);
        return newPosition; 
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision"); 
        //Instantiate(hitEffect, transform.position, Quaternion.identity); 
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            //Debug.Log("player bullet hit"); 

            Bullet pBullet = collision.gameObject.GetComponent<Bullet>(); 

            InjureEnemy(pBullet.bulletDamage);
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Vector2 sightLineDirection = target.position - transform.position;
        Debug.DrawRay(transform.position, sightLineDirection, Color.red);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, simpleRange);
    }

    protected void RotateFirePointToPlayer()
    {
        Vector3 direction = playerSnapshot.position - firePoint.position;
        Vector3 rotation = firePoint.position - playerSnapshot.position;

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0, 0, rot + 180);
    }

    protected void SpawnProjectiles(int _numberProjectiles, float bulletSpreadRange)
    {

        //if numProj is even
        if ((_numberProjectiles % 2) == 0)
        {
            float multAngle = bulletSpreadRange / (_numberProjectiles / 2);
            float currentAngle = bulletSpreadRange;

            for (int i = 0; i < (_numberProjectiles / 2); i++)
            {
                Quaternion spread = firePoint.rotation * Quaternion.Euler(0, 0, currentAngle);
                Quaternion spread2 = firePoint.rotation * Quaternion.Euler(0, 0, -currentAngle);

                //GameObject tmpObject = Instantiate(projectile, firePoint.position, spread);
                GameObject tmpObject = myPool.GetPooledObject();
                tmpObject.transform.position = firePoint.position;
                tmpObject.transform.rotation = spread;
                SetObjectAudioManager(tmpObject);
                tmpObject.SetActive(true); 

                //GameObject tmpObject2 = Instantiate(projectile, firePoint.position, spread2);
                GameObject tmpObject2 = myPool.GetPooledObject();
                tmpObject2.transform.position = firePoint.position;
                tmpObject2.transform.rotation = spread2;
                SetObjectAudioManager(tmpObject2);
                tmpObject2.SetActive(true);

                Rigidbody2D bulletRB = tmpObject.GetComponent<Rigidbody2D>();
                Rigidbody2D bulletRB2 = tmpObject2.GetComponent<Rigidbody2D>();


                bulletRB.velocity = bulletRB.transform.right * tmpObject.GetComponent<EnemyBullet>().moveSpeed;
                bulletRB2.velocity = bulletRB2.transform.right * tmpObject.GetComponent<EnemyBullet>().moveSpeed;

                currentAngle -= multAngle;
            }
        }
        else
        {

            int loopNum = (_numberProjectiles / 2) + 1;
            float multAngle = bulletSpreadRange / (_numberProjectiles / 2);
            float currentAngle = bulletSpreadRange;

            for (int i = 0; i < loopNum; i++)
            {
                if (i == 0)
                {
                    Quaternion spread = firePoint.rotation * Quaternion.Euler(0, 0, 0);

                    //GameObject temp = Instantiate(projectile, firePoint.position, spread);
                    GameObject temp = myPool.GetPooledObject();
                    temp.transform.position = firePoint.position;
                    temp.transform.rotation = spread;
                    temp.SetActive(true);

                    SetObjectAudioManager(temp);
                    Rigidbody2D rb = temp.GetComponent<Rigidbody2D>();
                    rb.velocity = rb.transform.right * temp.GetComponent<EnemyBullet>().moveSpeed;
                }
                else
                {
                    Quaternion spread = firePoint.rotation * Quaternion.Euler(0, 0, currentAngle);
                    Quaternion spread2 = firePoint.rotation * Quaternion.Euler(0, 0, -currentAngle);

                    //GameObject tmpObject = Instantiate(projectile, firePoint.position, spread);
                    GameObject tmpObject = myPool.GetPooledObject();
                    tmpObject.transform.position = firePoint.position;
                    tmpObject.transform.rotation = spread;
                    SetObjectAudioManager(tmpObject);
                    tmpObject.SetActive(true);

                    //GameObject tmpObject2 = Instantiate(projectile, firePoint.position, spread2);
                    GameObject tmpObject2 = myPool.GetPooledObject();
                    tmpObject2.transform.position = firePoint.position;
                    tmpObject2.transform.rotation = spread2;
                    SetObjectAudioManager(tmpObject2);
                    tmpObject2.SetActive(true);

                    Rigidbody2D bulletRB = tmpObject.GetComponent<Rigidbody2D>();
                    Rigidbody2D bulletRB2 = tmpObject2.GetComponent<Rigidbody2D>();

                    bulletRB.velocity = bulletRB.transform.right * tmpObject.GetComponent<EnemyBullet>().moveSpeed;
                    bulletRB2.velocity = bulletRB2.transform.right * tmpObject.GetComponent<EnemyBullet>().moveSpeed;

                    currentAngle -= multAngle;
                }
            }
        }
    }

}
