using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D;
    [SerializeField] private BoxCollider2D playerCollider;

    [Header("[Finding Ground]")]
    public float groundCheckRadius = 0.1f;
    public Transform groundCheckPoint, groundCheckPoint2;
    public LayerMask whatIsGround;
    [SerializeField]
    private bool isGrounded;

    private Animator myAnim;

    [Header("[Movement]")]
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float midAirMoveSpeed;
    private float xMovement, moveHorizontal; 
    private float facingDirection; //facing left or right

    [Header("[Jumping]")]
    public KeyCode jumpKeyCode;
    public float jumpForce;
    //time after player leaves platform, 0.2 seconds after you walk over the edge to jump
    public float currentJumps = 1;
    public float maximumJumps = 1;
    public float coyoteTime = 0.1f;
    private float hangCounter;
    //buffer length so you can jump a little bit before you hit the ground if you pressed it before the ground
    public float jumpBufferLength = 0.1f;
    public float jumpBufferCount;
    //So the impact doesn't happen for small distances
    public float minImpactTime = 1f;
    private float fallTimeForImpact;

    [Header("[Wall Jump]")]
    public LayerMask whatIsWall;
    public float wallSlideSpeed;
    [SerializeField] private float wallHopForce; 
    [SerializeField] private float wallJumpForce;
    [SerializeField] private bool isTouchingWall;
    public bool isWallSliding;
    [SerializeField] private Vector2 wallJumpDirection; 

    [Header("[Camera]")]
    public Transform camTarget;
    public float camAheadAmount, camAheadSpeed;

    [Header("[Booleans]")]
    public bool canMove = true;
    public bool canJump = true;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        facingDirection = 1;

        //Makes the direction equal to 1
        wallJumpDirection.Normalize();

        SetJumpCountMax();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded(); 

        Jump();
        SetJumpCountLower();

        UpdateCameraPosition();

        CheckIfTouchingWall();
        CheckIfWallSliding();
        PlayerFacingDirection();

        UpdateAnimations(); 
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    void Move()
    {
        xMovement = Input.GetAxisRaw("Horizontal");

        moveHorizontal = xMovement * moveSpeed;

        if (CheckGrounded())
        {
            myRigidBody2D.velocity = new Vector2(moveHorizontal, myRigidBody2D.velocity.y);
        }

        else if (!CheckGrounded() && !isWallSliding)
        {
            myRigidBody2D.velocity += new Vector2(moveHorizontal * midAirMoveSpeed * Time.deltaTime, 0);
            myRigidBody2D.velocity = new Vector2(Mathf.Clamp(myRigidBody2D.velocity.x, -moveSpeed, +moveSpeed), myRigidBody2D.velocity.y);
        }

        else if (isWallSliding)
        {
            if (myRigidBody2D.velocity.y < -wallSlideSpeed)
            {
                Debug.Log("Wall Slide");
                myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, -wallSlideSpeed);
            }
        }


        //myRigidBody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidBody2D.velocity.y);

        /* particle effects
        if (Input.GetAxisRaw("Horizontal") != 0 && isGrounded)
        {
            footEmission.rateOverTime = 35f; //start spawning
        }
        else
        {
            footEmission.rateOverTime = 0f; //stop spawning
        }

        //impact effect
        //previous frame we weren't on ground
        if (!wasOnGround && isGrounded && (fallTimeForImpact >= minImpactTime))
        {
            impactEffect.gameObject.SetActive(true);
            impactEffect.Stop();
            impactEffect.transform.position = footSteps.transform.position;
            impactEffect.Play();
        }
        */

        //make it so the impact effect happens only after a certain amount of fall time
        if (!isGrounded)
        {
            fallTimeForImpact += Time.deltaTime;
        }
        else if (isGrounded && fallTimeForImpact > 0f)
        {
            fallTimeForImpact = 0f;
        }

        //update wasOnGround at the end of the frame for particles
        //wasOnGround = isGrounded;
    }

    void UpdateCameraPosition()
    {
        //move camera point
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, camAheadAmount * Input.GetAxisRaw("Horizontal"),
                camAheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
        }
    }

    void Jump()
    {
        if (!isWallSliding)
        {
            //manage coyote time
            if (isGrounded)
            {
                SetJumpCountMax();
                hangCounter = coyoteTime;
            }
            else if (maximumJumps == 1 && !isGrounded)
            {
                hangCounter -= Time.deltaTime;
            }

            //manage jump buffer
            if (Input.GetKeyDown(jumpKeyCode) && currentJumps > 0)
            {
                jumpBufferCount = jumpBufferLength;
            }
            else
            {
                jumpBufferCount -= Time.deltaTime;
            }


            //Make player jump
            if (jumpBufferCount >= 0 && hangCounter > 0 && canJump)
            {
                myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, jumpForce);
                jumpBufferCount = 0;
            }

            if (Input.GetKeyUp(jumpKeyCode) && myRigidBody2D.velocity.y > 0)
            {
                myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, myRigidBody2D.velocity.y * 0.5f);
            }
        }

        else if ((isWallSliding || isTouchingWall) && Input.GetKeyDown(jumpKeyCode))
        {
            isWallSliding = false;
            currentJumps--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * -facingDirection, wallJumpForce * wallJumpDirection.y);
            myRigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
            Debug.Log("WallJump");
            facingDirection = -facingDirection; 
        }
    }

    //subtract from the jump count
    void SetJumpCountLower()
    {
        if (Input.GetKeyUp(jumpKeyCode) && currentJumps > 0 && myRigidBody2D.velocity.y != 0 && canJump)
        {
            currentJumps -= 1;
        }
    }

    //sets jump count to max
    void SetJumpCountMax()
    {
        currentJumps = maximumJumps;
    }

    void PlayerFacingDirection()
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

    //returns value of player grounded state
    bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround) ||
            Physics2D.OverlapCircle(groundCheckPoint2.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfTouchingWall()
    {
        isTouchingWall = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, transform.right * facingDirection, 0.1f, whatIsWall);
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !CheckGrounded() && myRigidBody2D.velocity.y < 0)
        {
            isWallSliding = true;

        }
        else
        {
            isWallSliding = false;
        }
    }

    void UpdateAnimations()
    {
        myAnim.SetFloat("moveHorizontal", xMovement);
        myAnim.SetBool("isGrounded", isGrounded);
        myAnim.SetFloat("facingDirection", facingDirection);
        myAnim.SetBool("isSliding", isWallSliding);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckPoint2.position, groundCheckRadius);
    }

}
