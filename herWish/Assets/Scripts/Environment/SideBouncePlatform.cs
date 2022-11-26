using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBouncePlatform : MonoBehaviour
{
    private PlayerControl myPlayerControl;
    public float bounceForceX, bounceForceY;
    public Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myPlayerControl = GameObject.FindObjectOfType<PlayerControl>().GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayerControl == null)
        {
            myPlayerControl = GameObject.FindObjectOfType<PlayerControl>().GetComponent<PlayerControl>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //player moving right towards object, bounce the player left
            if (collision.gameObject.transform.position.x < gameObject.transform.position.x)
            {
                //Debug.Log("Bounce left"); 
                myPlayerControl.PlayerSideBounce(-bounceForceX, bounceForceY);
                myAnim.SetTrigger("bounce");
            }
            //bounce the player right
            else
            {
                //Debug.Log("Bounce right"); 
                myPlayerControl.PlayerSideBounce(bounceForceX, bounceForceY);
                myAnim.SetTrigger("bounce");
            }

        }
    }
}
