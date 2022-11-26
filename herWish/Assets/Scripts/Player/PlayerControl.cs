using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D myRigidBody2D;
    private Animator myAnim;

    [Header("[Movement]")]
    [SerializeField]
    private float moveSpeed;
    private int facingDirection; //facing left or right
    public bool isMoving; 

    [Header("[Finding Ground]")]
    public float groundCheckRadius = 0.1f;
    public Transform groundCheckPoint, groundCheckPoint2;
    public bool groundCheck_1, groundCheck_2; 
    public LayerMask whatIsGround;
    [SerializeField]
    private bool isGrounded;
    public bool jumpColliderHit;

    [Header("[Booleans]")]
    public bool canMove = true;
    //only allows player to move away from edge
    public bool edgeMove = true; 
    public bool canJump = true;
    public bool isJumping = false;
    public bool isLaunching = false;
    public bool justLaunched = false;
    public bool groundedLeft; 

    [Header("[Other]")]
    public SpriteRenderer playerSpriteRender;

    private int facePlantDirection;
    [SerializeField] private float longFallTime = 1.5f;
    [SerializeField] private float facePlantTime = 1f;
    [SerializeField] private float airTimer, previousAirTime, facePlantTimer; 
    private bool facePlant = false;
    private bool moveBuffer;
    private float moveBufferTime = 0.05f;
    private float mBufferTimer; 

    void Start()
    {
        SetMoveState(true); 

        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>(); 

        facingDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        groundedLeft = CheckGroundedLeft();

        CheckPlayerEdge(); 
        CheckFacePlant(); 
        PlayerFacingDirection();
        UpdateIsGrounded();
        UpdateAnimations(); 
    }

    private void FixedUpdate()
    {
        if (moveBuffer)
        {
            mBufferTimer += Time.deltaTime;

            if (mBufferTimer > moveBufferTime)
            {
                moveBuffer = false; 
                mBufferTimer = 0;
            }
        }


        if (canMove && !moveBuffer && edgeMove)
        {
            Move();
        }
    }


    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        if (!isJumping)
        {
            //if completely grounded 
            if (BothGroundedTrue())
            {
                //Debug.Log("Both grounded true"); 
                myRigidBody2D.velocity = new Vector2(moveX * moveSpeed, myRigidBody2D.velocity.y);

                if (Mathf.Abs(moveX) > 0.1)
                {
                    isMoving = true; 
                }
                else
                {
                    isMoving = false; 
                }

            }
            else if (!BothGroundedTrue() || !OneGroundedTrue())
            {
                myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, myRigidBody2D.velocity.y);

                isMoving = false;
            }
        }
        else
        {
            myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, myRigidBody2D.velocity.y);

            isMoving = false;
        }
    }

    void CheckPlayerEdge()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        //11/18
        //isJumping might be a source of problems in this function
        if (!isJumping)
        {
            if (BothGroundedTrue())
            {
                edgeMove = true;
            }
            else if (OneGroundedTrue())
            {
                //if player is moving at all while one grounded, check if he can move direction he's trying to
                if (Mathf.Abs(moveX) > 0.1)
                {
                    //if player isn't grounded left, make sure they can't go left
                    if (!CheckGroundedLeft() && CheckGroundedRight())
                    {
                        if (moveX > 0.1)
                        {
                            myRigidBody2D.velocity = new Vector2(moveX * moveSpeed, myRigidBody2D.velocity.y);
                            isMoving = true;
                            Debug.Log("Moving away from edge");
                            edgeMove = true;
                        }
                        else
                        {
                            StopHorMovement();
                            edgeMove = false;
                            isMoving = false;
                        }
                    }
                    else if (!CheckGroundedRight() && CheckGroundedLeft())
                    {
                        if (moveX < -0.1)
                        {
                            myRigidBody2D.velocity = new Vector2(moveX * moveSpeed, myRigidBody2D.velocity.y);
                            isMoving = true;
                            edgeMove = true;
                            Debug.Log("Moving away from edge");
                        }
                        else
                        {
                            StopHorMovement();
                            edgeMove = false;
                            isMoving = false;
                        }
                    }
                }
                else
                {
                    isMoving = false;
                }
            }
        }
    }

    void PlayerFacingDirection()
    {
        //Make sure player can't change direction while jumping
        if (!isJumping)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.1)
            {
                facingDirection = 1;
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.1)
            {
                facingDirection = -1;
            }
        }
    }

    void UpdateIsGrounded()
    {
        if (justLaunched)
        {
            //Debug.Log("Just launched is true"); 
            if (BothGroundedTrue() || jumpColliderHit)
            {
                isGrounded = true;
                moveBuffer = true; 
                justLaunched = false; 
            }
            else
            {
                isGrounded = false; 
            }
        }
        else
        {
            if (OneGroundedTrue() || jumpColliderHit)
            {
                isGrounded = true; 
            }
            else
            {
                isGrounded = false; 
            }
        }


    }

    void UpdateAnimations()
    {
        myAnim.SetBool("isMoving", isMoving);
        myAnim.SetBool("isGrounded", isGrounded);
        myAnim.SetBool("isJumping", isJumping); 
        myAnim.SetBool("isLaunching", isLaunching);
        myAnim.SetBool("facePlant", facePlant); 
        myAnim.SetFloat("facingDirection", facingDirection);
        myAnim.SetFloat("facePlantDirection", facePlantDirection);
    }

    void CheckFacePlant()
    {
        if (!facePlant)
        {
            if (isGrounded)
            {
                airTimer = 0f;
            }
            else
            {
                airTimer += Time.deltaTime;
                previousAirTime = airTimer;
                //We don't want player to be able to change this while faceplanted
                facePlantDirection = facingDirection;
            }
        }
        if (previousAirTime > longFallTime && OneGroundedTrue())
        {
            Debug.Log("FacePlant");

            if (facePlantTimer < facePlantTime)
            {
                facePlant = true;
                //Debug.Log("Face plant is true");
                facePlantTimer += Time.deltaTime;
                SetMoveState(false);
                SetPlayerVelocityZero();
            }

            if (facePlantTimer > facePlantTime)
            {
                previousAirTime = 0;
                facePlant = false;
                facePlantTimer = 0;
                SetMoveState(true);
            }
        }
    }

    //use this for bouncy platforms
    public void ResetAirTimer()
    {
        airTimer = 0f; 
    }

    public void PlayerDefaultBounce(float forceX, float forceY)
    {
        float newForceX = forceX * facingDirection; 

        myRigidBody2D.velocity = new Vector2(newForceX, forceY);
        ResetAirTimer(); 
    }

    public void PlayerSideBounce(float forceX, float forceY)
    {
        myRigidBody2D.velocity = new Vector2(forceX, forceY);
        ResetAirTimer();
    }


    public void StopHorMovement()
    {
        myRigidBody2D.velocity = new Vector2(0f, myRigidBody2D.velocity.y); 
    }

    public void SetJumpFacingDirection(int direction)
    {
        facingDirection = direction;
    }

    //checks if grounded two is grounded for walking to an edge
    bool CheckGroundedLeft()
    {
        //return Physics2D.OverlapCircle(groundCheckPoint2.position, groundCheckRadius, whatIsGround);
        return groundCheck_2;
    }

    //checks if grounded one is grounded for walking to an edge
    bool CheckGroundedRight()
    {
        //return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround); 
        return groundCheck_1;
    }

    //returns if both grounded are true
    bool BothGroundedTrue()
    {
        return groundCheck_1 && groundCheck_2; 
    }

    //returns if one grounded is true
    public bool OneGroundedTrue()
    {
        return groundCheck_1 || groundCheck_2;
    }

    public bool GetIsGrounded()
    {
        return isGrounded; 
    }

    public void SetMoveState(bool state)
    {
        canMove = state; 
    }

    public void SetPlayerVelocityZero()
    {
        myRigidBody2D.velocity = Vector2.zero;
    }

    public bool GetIsMoving()
    {
        return isMoving; 
    }

    public bool GetMoveState()
    {
        return canMove; 
    }

    public bool GetPlayerFacePlant()
    {
        return facePlant; 
    }
  
}
