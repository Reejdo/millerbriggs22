using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceItem : MonoBehaviour
{
    private PlayerControl myPlayerControl;
    public float bounceForce;
    public Animator myAnim; 

    // Start is called before the first frame update
    void Start()
    {
        myPlayerControl = GameObject.Find("player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayerControl == null)
        {
            myPlayerControl = GameObject.Find("player").GetComponent<PlayerControl>(); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            myPlayerControl.PlayerBounceUp(bounceForce);
            myAnim.SetTrigger("bounce"); 
        }
    }
}
