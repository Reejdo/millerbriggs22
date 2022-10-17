using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGunMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Transform spawnPos; 
    public Camera cam;
    private Animator anim;
    public Vector2 movement, mousePos;
    private bool isMoving, canMove = true;
    public float directionToPoint = 1;

    private float lastMoveX, lastMoveY;

    [SerializeField] private float knockBackForce; 
    public float knockBackTime;
    private float kBTimer;
    private bool isKnockedBack;

    private PlayerHealth myPlayerHealth; 
    private void Awake()
    {
        anim = GetComponent<Animator>();
        myPlayerHealth = GetComponent<PlayerHealth>();
        SetCanMove(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!myPlayerHealth.dead)
        {
            if (canMove)
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                if (Mathf.Abs(movement.x) > 0.1 || Mathf.Abs(movement.y) > 0.1)
                {
                    isMoving = true;
                    SetLastMoveValues();
                }
                else
                {
                    isMoving = false;
                }

            }
            else
            {
                isMoving = false; 
            }
        }
        else
        {
            isMoving = false; 
        }


        if (isKnockedBack)
        {
            kBTimer += Time.deltaTime; 

            if (kBTimer > knockBackTime)
            {
                isKnockedBack = false;
                kBTimer = 0;
                SetCanMove(true); 
            }
        }


        //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        anim.SetFloat("moveX", movement.x);
        anim.SetFloat("moveY", movement.y);
        anim.SetFloat("lastMoveX", lastMoveX);
        anim.SetFloat("lastMoveY", lastMoveY); 

        anim.SetBool("isMoving", isMoving);
    }

    private void FixedUpdate()
    {
        if (!myPlayerHealth.dead)
        {
            if (!isKnockedBack)
            {
                rb.velocity = movement.normalized * moveSpeed;
            }

        }
        else
        {
            rb.velocity = Vector2.zero; 
        }

        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }

    void SetLastMoveValues()
    {
        if (movement.x > 0.1)
        {
            lastMoveX = 1; 
        }
        else if (movement.x < -0.1)
        {
            lastMoveX = -1; 
        }
        else
        {
            lastMoveX = 0; 
        }


        if (movement.y > 0.1)
        {
            lastMoveY = 1;
        }
        else if (movement.y < -0.1)
        {
            lastMoveY = -1;
        }
        else
        {
            lastMoveY = 0;
        }

    }


    public void PlayerKnockBack(Transform attackerPosition)
    {
        isKnockedBack = true;
        SetCanMove(false); 

        Vector2 knockBackDirection = (transform.position - attackerPosition.position).normalized;
        rb.velocity = knockBackDirection * knockBackForce; 
    }

    void SetCanMove(bool state)
    {
        canMove = state;     
    }

}
