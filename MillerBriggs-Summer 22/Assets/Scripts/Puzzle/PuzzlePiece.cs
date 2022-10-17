using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{

    [Header("Puzzle Type Settings")]
    public bool onePoint; 
    public bool twoPoint; 
    public bool shootUp, shootLeft, shootRight, shootDown;
    public int rayCastDistance;
    
    public bool canShootRaycast = true;
    public string tagName = "PuzzleObject"; 
    public RaycastHit2D hit1, hit2; //hit1 is up and down, hit2 is left and right
    public GameObject[] hitObjects; 

    public LayerMask myLayerMask;
    private Transform myTransform;

    void Start()
    { 
        myTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (canShootRaycast)
        {
            ShootRayCast();
            CheckRayCastHit();
        }

    }

    void ShootRayCast()
    {
        Vector2 dir1 = new Vector2(0, 0); 
        Vector2 dir2 = new Vector2(0, 0);

        if (shootUp)
        {
            dir1 = transform.up; 
            //Physics2D.Raycast(transform.position, Vector2.up * rayCastDistance); 
            //Physics.Raycast2D(transform.position, transform.TransformDirection(Vector3.forward), out rayCast1, Mathf.Infinity, layerMask)
        }
        else if (shootDown)
        {
            dir1 = -transform.up; 
        }

        if (shootRight)
        {
            dir2 = transform.right; 
        }
        else if (shootLeft)
        {
            dir2 = -transform.right; 
        }

        if (onePoint)
        {
            if (shootUp || shootDown)
            {
                hit1 = Physics2D.Raycast(transform.position, dir1, Mathf.Infinity, myLayerMask);
                Debug.DrawRay(transform.position, dir1 * rayCastDistance, Color.magenta);
            }
            else if (shootLeft || shootRight)
            {
                hit2 = Physics2D.Raycast(transform.position, dir2, Mathf.Infinity, myLayerMask);
                Debug.DrawRay(transform.position, dir2 * rayCastDistance, Color.magenta);
            }
        }
        else
        {
            hit1 = Physics2D.Raycast(transform.position, dir1, Mathf.Infinity, myLayerMask);
            Debug.DrawRay(transform.position, dir1 * rayCastDistance, Color.magenta);
            hit2 = Physics2D.Raycast(transform.position, dir2, Mathf.Infinity, myLayerMask);
            Debug.DrawRay(transform.position, dir2 * rayCastDistance, Color.cyan);
        }

    }

    OnePuzzleManager hit1PuzzleManager = null;
    OnePuzzleManager hit2PuzzleManager = null;

    void CheckRayCastHit()
    {
        //if hit1 and it is the right tag, send this gameobject to hit manager 1 so it can update booleans correctly
        //if hit something else but the manager isn't null, update to false and make manager null
        if (hit1.collider != null)
        {
            hitObjects[0] = hit1.collider.gameObject;

            if (hit1.collider.CompareTag(tagName))
            {
                hit1PuzzleManager = hit1.collider.gameObject.GetComponent<OnePuzzleManager>();
                hit1PuzzleManager.UpdateHitActivity(gameObject, true);
            }
            else if (hit1.collider.tag != tagName && hit1PuzzleManager != null)
            {
                hit1PuzzleManager.UpdateHitActivity(gameObject, false);
                hit1PuzzleManager = null;
            }
            else
            {
                //Debug.Log("Hit something that wasn't puzzle manager!"); 
            }

        }
        //once null, send this gameobject to hit puzzle manager so it can update booleans correctly
        else if (hit1.collider == null && hit1PuzzleManager != null)
        {
            hit1PuzzleManager.UpdateHitActivity(gameObject, false);
            hit1PuzzleManager = null; 
        }

        //if the second beam hits something, do the same
        if (hit2.collider != null)
        {
            hitObjects[1] = hit2.collider.gameObject;

            if (hit2.collider.CompareTag(tagName))
            {
                hit2PuzzleManager = hit2.collider.gameObject.GetComponent<OnePuzzleManager>();
                hit2PuzzleManager.UpdateHitActivity(gameObject, true);
            }
            else if (hit2.collider.tag != tagName && hit2PuzzleManager != null)
            {
                hit2PuzzleManager.UpdateHitActivity(gameObject, false);
                hit2PuzzleManager = null;
            }
            else
            {
                //Debug.Log("Hit something that wasn't puzzle manager!");
            }
        }

        //once null update hit puzzle manager
        else if (hit2.collider == null && hit2PuzzleManager != null)
        {
            Debug.Log("Updating to false"); 
            hit2PuzzleManager.UpdateHitActivity(gameObject, false);
            hit2PuzzleManager = null;
        }
    }

    public void FlipVertical()
    {
        myTransform.Rotate(180, 0, 0, Space.World); 
    }

    public void FlipHorizontal()
    {
        myTransform.Rotate(0, 180, 0, Space.World);
    }

    public void Rotate90Degrees(bool clockWise)
    {
        if (clockWise)
        {
            myTransform.Rotate(0, 0, -90); 
        }
        else
        {
            myTransform.Rotate(0, 0, 90); 
        }
    }


}
