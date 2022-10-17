using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderJumpEnable : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;
    public Collider2D myCollider; 

    void Start()
    {
        //Find the player's feet
        player = GameObject.Find("groundCheck").GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("groundCheck").GetComponent<Transform>();
        }

        ColliderState(); 

    }


    void ColliderState()
    {
        //enable collider if player is above this object, otherwise disable
        if (player.position.y > gameObject.transform.position.y)
        {
            //Debug.Log("Player is higher than object"); 
            myCollider.enabled = true; 
        }
        else
        {
            //Debug.Log("Player is lower than object");
            myCollider.enabled = false; 
        }
    }

}
