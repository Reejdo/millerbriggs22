using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchOnHit : MonoBehaviour
{
    public Animator stretchAnim;
    public PlayerControl playerControl;
    public Transform hitPosition, hitPosition2;
    public float hitRadius;
    public LayerMask whatCanHit; 

    // Start is called before the first frame update
    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckHitHead() && !collision.collider.isTrigger)
        {
            stretchAnim.SetTrigger("squash"); 
        }
    }

    bool CheckHitHead()
    {
        return Physics2D.OverlapCircle(hitPosition.position, hitRadius, whatCanHit) ||
            Physics2D.OverlapCircle(hitPosition.position, hitRadius, whatCanHit);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.75F);
        Gizmos.DrawWireSphere(hitPosition.position, hitRadius);
        Gizmos.DrawWireSphere(hitPosition2.position, hitRadius);
    }
}
