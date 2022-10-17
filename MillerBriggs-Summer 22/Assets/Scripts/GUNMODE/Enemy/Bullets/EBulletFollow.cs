using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBulletFollow : EnemyBullet
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        RotateToPlayer(); 

        if (gameObject.activeInHierarchy)
        {
            LifeTime(); 
        }
        else
        {
            currentLifeTime = 0; 
        }
    }

    void LifeTime()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= destroyTime)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            currentLifeTime = 0; 
        }
    }


    protected override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); 
    }
}
