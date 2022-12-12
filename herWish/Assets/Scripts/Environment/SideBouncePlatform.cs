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
            myPlayerControl.PlayerSideBounce(bounceForceX, bounceForceY);
            myAnim.SetTrigger("bounce");

        }
    }
}
