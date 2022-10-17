using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Transform target;

    public float moveSpeed;

    [SerializeField] protected float destroyTime;

    [SerializeField] protected GameObject destroyEffect, wallDestroyEffect; 

    protected float currentLifeTime;
    public AudioManager myAudioManager;

    public bool startLifeTime = true;

    private Vector3 direction, rotation; 

    //public GameObject destroyEffect; 

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy && startLifeTime)
        {
            LifeTime(); 
        }
        else
        {
            currentLifeTime = 0;
        }

        MoveTowardsPlayer(); 
    }

    void LifeTime()
    {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime >= destroyTime)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            currentLifeTime = 0;
            Debug.Log("Lifetime over"); 
        }
    }

    public void ResetLifeTime()
    {
        currentLifeTime = 0; 
    }

    public virtual void RotateToPlayer()
    {
        direction = target.position - transform.position;
        rotation = transform.position - target.position;

        //normalizing this makes sure that it doesn't increase or decrease speed depending on player position
        rb.velocity = new Vector2(direction.x, direction.y).normalized * moveSpeed;

        //Debug.Log("velocity: " + rb.velocity); 

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    protected virtual void Move()
    {
        //transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); 

    }

    protected virtual void MoveTowardsPlayer()
    {
        //normalizing this makes sure that it doesn't increase or decrease speed depending on player position
        rb.velocity = new Vector2(direction.x, direction.y).normalized * moveSpeed;

        //Debug.Log("velocity: " + rb.velocity);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy bullet collision"); 
        Instantiate(wallDestroyEffect, transform.position, Quaternion.identity);
        //Destroy(gameObject);
        gameObject.SetActive(false); 

    }
}
