using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D;

    public float groundCheckRadius = 0.1f; 
    public Transform groundCheckPoint, groundCheckPoint2;
    public LayerMask whatIsGround;
    private bool isGrounded;

    private Animator myAnim;
    public SpriteRenderer playerSpriteRender;

    [SerializeField]
    private float moveSpeed, jumpForce;
    [SerializeField]
    private KeyCode jumpKeyCode;

    //time after player leaves platform, 0.2 seconds after you walk over the edge to jump
    public float coyoteTime = 0.1f;
    private float hangCounter; 

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded(); 
    }

    private void FixedUpdate()
    {
        Move();
        Jump(); 
    }

    void Move()
    {
        myRigidBody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidBody2D.velocity.y); 
    }

    void Jump()
    {
        if (isGrounded)
        {
            hangCounter = coyoteTime;
        }
        else
        {
            hangCounter -= Time.deltaTime; 
        }


        if (Input.GetKeyDown(jumpKeyCode) && hangCounter > 0)
        {
            myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, jumpForce); 
        }

        if (Input.GetKeyUp(jumpKeyCode) && myRigidBody2D.velocity.y > 0)
        {
            myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, myRigidBody2D.velocity.y * 0.5f); 
        }
    }

    bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround) || 
            Physics2D.OverlapCircle(groundCheckPoint2.position, groundCheckRadius, whatIsGround);
    }

}
