using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    public bool canTriggerExclaim = true; 
    public bool playerInRange;
    public Enemy myEnemy;
    public bool hasTriggeredExclaim;


    private void Update()
    {
        //allows us to use this script multiple times and not have exclaim activate more than once
        if (canTriggerExclaim)
        {
            if (!myEnemy.CannotSeePlayer())
            {
                if (!hasTriggeredExclaim && playerInRange)
                {
                    hasTriggeredExclaim = true;
                    myEnemy.exclaimAnim.SetTrigger("exclaim");
                }
            }

            if (myEnemy.CannotSeePlayer() || !playerInRange)
            {
                hasTriggeredExclaim = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

}
