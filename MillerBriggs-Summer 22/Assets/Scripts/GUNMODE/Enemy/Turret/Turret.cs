using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float changeStateTime = 0.5f;
    private float shootTimer;
    private float stateTimer; 

    public GameObject projectile;
    
    //0 IS UP, 1 IS RIGHT, 2 IS DOWN, 3 IS LEFT
    [Header("FirePoints = ClockWise")]
    public Transform[] firePoint;

    [Header("0=U 1=D 2=L 3=R 4=LR 5=UD 6=ALL")]
    [SerializeField] private float fireType; 
    private bool isFiring;
    private bool playerInRange; 

    private Animator myAnim; 
    private AudioManager myAudioManager;
    protected EnemyBulletPool myPool;
    public GameObject myBulletPool;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();

        GameObject temp = Instantiate(myBulletPool, transform.position, Quaternion.identity);
        myPool = temp.GetComponent<EnemyBulletPool>();
        myPool.PoolObjects(projectile);

    }

    // Update is called once per frame
    void Update()
    {
        if (myAudioManager == null)
        {
            myAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }


        if (playerInRange && !isFiring)
        {
            stateTimer += Time.deltaTime; 

            if (stateTimer > changeStateTime && !isFiring)
            {
                isFiring = true;
                stateTimer = 0; 
            }

        }
        else if (!playerInRange && isFiring)
        {
            stateTimer += Time.deltaTime;

            if (stateTimer > changeStateTime && isFiring)
            {
                isFiring = false;
                stateTimer = 0;
            }
        }

        Shoot();

        myAnim.SetFloat("fireType", fireType);
        myAnim.SetBool("isFiring", isFiring); 
    }


    void Shoot()
    {
        if (isFiring)
        {
            if (shootTimer < fireRate)
            {
                shootTimer += Time.deltaTime; 
            }
            else
            {
                ChooseFireDirection(); 
                shootTimer = 0;
            }
        }
        else
        {
            shootTimer = 0; 
        }
    }

    void ChooseFireDirection()
    {
        switch(fireType)
        {
            //Case Numbers
            //0=U 1=D 2=L 3=R 4=LR 5=UD 6=ALL

            //For FireTypes, 
            //0 IS UP, 1 IS RIGHT, 2 IS DOWN, 3 IS LEFT
            case 6:
                for (int i = 0; i < 4; i++)
                {
                    GameObject temp = myPool.GetPooledObject();
                    BulletSetUp(temp, firePoint[i]); 
                }
                break;
            case 5:
                GameObject tempU = myPool.GetPooledObject();
                BulletSetUp(tempU, firePoint[0]);
                GameObject tempD = myPool.GetPooledObject();
                BulletSetUp(tempD, firePoint[2]);
                break;
            case 4:
                GameObject tempL = myPool.GetPooledObject();
                BulletSetUp(tempL, firePoint[3]);
                GameObject tempR = myPool.GetPooledObject();
                BulletSetUp(tempR, firePoint[1]);
                break;
            case 3:
                GameObject tempL2 = myPool.GetPooledObject();
                BulletSetUp(tempL2, firePoint[1]);
                break;
            case 2:
                GameObject tempR2 = myPool.GetPooledObject();
                BulletSetUp(tempR2, firePoint[3]);
                break;
            case 1:
                GameObject tempD2 = myPool.GetPooledObject();
                BulletSetUp(tempD2, firePoint[2]);
                break;
            case 0:
                GameObject tempU2 = myPool.GetPooledObject();
                BulletSetUp(tempU2, firePoint[0]);
                break; 
            default:
                Debug.Log("No fire type!"); 
                break; 
        }

        shootTimer = 0; 
    }

    void BulletSetUp(GameObject temp, Transform firePoint)
    {
        temp.transform.position = firePoint.position;
        temp.transform.rotation = firePoint.rotation; 
        Rigidbody2D tempRB = temp.GetComponent<Rigidbody2D>(); 
        tempRB.velocity = tempRB.transform.up * temp.GetComponent<EnemyBullet>().moveSpeed;
        temp.GetComponent<EnemyBullet>().ResetLifeTime(); 
        temp.SetActive(true); 
        SetObjectAudioManager(temp);
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

    public void SetPlayerRange(bool state)
    {
        playerInRange = state;
    }
}
