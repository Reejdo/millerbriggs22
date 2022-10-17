using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D;
    private Animator myAnim;
    private DialogueManager myDialogueManager; 

    [SerializeField] private BoxCollider2D playerCollider;

    [Header("[Finding Ground]")]
    public float groundCheckRadius = 0.1f;
    public Transform groundCheckPoint, groundCheckPoint2;
    public LayerMask whatIsGround;
    [SerializeField]
    private bool isGrounded;

    [Header("[Movement]")]
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float midAirMoveSpeed;
    private float xMovement, moveHorizontal;
    public float facingDirection; //facing left or right
    private bool isMoving;
    public float minMoveTime = 2f;
    private float moveTimeForParticles;

    [Header("[Jumping]")]
    public KeyCode jumpKeyCode;
    public float jumpForce;
    //time after player leaves platform, 0.2 seconds after you walk over the edge to jump
    public float currentJumps = 1;
    public float maximumJumps = 1;
    public float coyoteTime = 0.1f;
    private float hangCounter;

    private float jumpTime;
    [SerializeField] private float wallJumpTime = 0.1f; 

    //buffer length so you can jump a little bit before you hit the ground if you pressed it before the ground
    public float jumpBufferLength = 0.1f;
    public float jumpBufferCount;
    //So the impact doesn't happen for small distances
    public float minImpactTime = 1f;
    private float fallTimeForImpact;

    [Header("[Wall Jump]")]
    public LayerMask whatIsWall;
    public float wallSlideSpeed;
    public float wallCheckDistance;
    [SerializeField] private float wallJumpForce; 
    [SerializeField] private bool isTouchingWall;
    public bool isWallSliding;
    [SerializeField] private float wallAirSpeed;
    private float defaultAirSpeed;
    private bool firstWallTouch; 

    [Header("[Camera]")]
    public Transform camTarget;
    public float camAheadAmount, camAheadSpeed;

    [Header("[Booleans]")]
    public bool canMove = true;
    public bool canJump = true;

    [Header("[Other]")]
    public Animator stretchAnim;
    public ParticleSystem impactEffect;
    public ParticleSystem footEmission; 
    public bool wasOnGround;
    public bool wasJustStopped;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>(); 

        facingDirection = 1;

        defaultAirSpeed = midAirMoveSpeed; 

        SetJumpCountMax();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded(); 

        if (!myDialogueManager.dialogueIsPlaying && !isWallSliding && canMove)
        {
            Jump();
        }
        else if (isWallSliding)
        {
            WallJump(); 
        }

        if (!myDialogueManager.dialogueIsPlaying)
        {
            PlayerFacingDirection();
        }


        SetJumpCountLower();
        UpdateCameraPosition();
        CheckWallSliding();
        UpdateAnimations(); 
    }

    private void FixedUpdate()
    {
        //Make sure character can't move during dialogue
        if (!myDialogueManager.dialogueIsPlaying)
        {
            if (canMove)
            {
                Move();
            }
        }
        else
        {
            myRigidBody2D.velocity = new Vector2(0, myRigidBody2D.velocity.y);
        }

    }

    void Move()
    {
        xMovement = Input.GetAxisRaw("Horizontal");

        moveHorizontal = xMovement * moveSpeed;

        if (Mathf.Abs(xMovement) > 0.1f)
        {
            isMoving = true;

            moveTimeForParticles = 0; 
        }
        else 
        {
            isMoving = false;

            moveTimeForParticles += Time.deltaTime;

            if (moveTimeForParticles > minMoveTime)
            {
                wasJustStopped = true; 
            }
        }

        if (CheckGrounded())
        {
            midAirMoveSpeed = defaultAirSpeed; 
            myRigidBody2D.velocity = new Vector2(moveHorizontal, myRigidBody2D.velocity.y);
        }

        else if (!CheckGrounded())
        {
            myRigidBody2D.velocity = new Vector2(moveHorizontal * midAirMoveSpeed, myRigidBody2D.velocity.y);
        }

        ParticleEffects(); 

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
        wasOnGround = isGrounded;
    }

    void Jump()
    {
        //manage time after leaving the ground so you can jump a little bit after leaving the ground
        if (isGrounded)
        {
            SetJumpCountMax();
            hangCounter = coyoteTime;
        }
        else if (!isGrounded)
        {
            hangCounter -= Time.deltaTime;
        }

        //manage time before hitting the ground so you can jump a little bit before touching the ground
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

            //make player stretch
            stretchAnim.SetTrigger("stretch"); 
        }

        if (Input.GetKeyUp(jumpKeyCode) && myRigidBody2D.velocity.y > 0)
        {
            myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, myRigidBody2D.velocity.y * 0.5f);
        }
    }

    void WallJump()
    {
        //firstWallTouch prevents jumping on the same wall
        if (Input.GetKeyDown(jumpKeyCode))
        {
            firstWallTouch = false;
            Vector2 forceToAdd = new Vector2(wallJumpForce * -facingDirection, wallJumpForce);
            myRigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);

            //make player stretch
            stretchAnim.SetTrigger("stretch");
        }
    }


    private void CheckWallSliding()
    {
        //if facing right
        if (facingDirection == 1)
        {
            isTouchingWall = Physics2D.Raycast(transform.position, new Vector2(wallCheckDistance, 0), wallCheckDistance, whatIsWall);
            Debug.DrawRay(transform.position, new Vector2(wallCheckDistance, 0), Color.blue);
        }
        //is facing left
        else
        {
            isTouchingWall = Physics2D.Raycast(transform.position, new Vector2(-wallCheckDistance, 0), wallCheckDistance, whatIsWall);
            Debug.DrawRay(transform.position, new Vector2(-wallCheckDistance, 0), Color.blue);
        }


        //firstWallTouch prevents jumping on the same wall
        if (isTouchingWall && !CheckGrounded() && xMovement != 0 && firstWallTouch)
        {
            midAirMoveSpeed = wallAirSpeed; 
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }
        //if it has been 0.2 seconds 
        else if (jumpTime < Time.time)
        {
            isWallSliding = false;
        }

        //adjust wall slide fall speed 
        if (isWallSliding)
        {
            if (myRigidBody2D.velocity.y < -wallSlideSpeed)
            {
                myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, -wallSlideSpeed);
            }
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

    public void SetMoveState(bool state)
    {
        canMove = state;
    }

    void UpdateCameraPosition()
    {
        //move camera point
        if (Input.GetAxisRaw("Horizontal") != 0 && !myDialogueManager.dialogueIsPlaying)
        {
            camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, camAheadAmount * Input.GetAxisRaw("Horizontal"),
                camAheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
        }
    }

    void UpdateAnimations()
    {
        myAnim.SetBool("isMoving", isMoving); 
        myAnim.SetBool("isGrounded", isGrounded);
        myAnim.SetFloat("facingDirection", facingDirection);
        myAnim.SetBool("isSliding", isWallSliding);
    }

    public void DialogueMode()
    {
        isMoving = false;
        isWallSliding = false; 
        myAnim.SetBool("isMoving", false);
        myAnim.SetBool("isSliding", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckPoint2.position, groundCheckRadius);
    }

    //start it as false outside the function so it isn't constantly updated
    bool startFootEmission = false; 

    void ParticleEffects()
    {
        //movement particles
        if (xMovement != 0 && wasJustStopped && !startFootEmission)
        {
            startFootEmission = true; 
            StartCoroutine(PlayFootParticle()); 
        }

        //impact effect
        //previous frame we weren't on ground
        if (!wasOnGround && isGrounded && (fallTimeForImpact >= minImpactTime))
        {
            stretchAnim.SetTrigger("squash");
            impactEffect.gameObject.SetActive(true);
            impactEffect.Stop();
            impactEffect.Play();
        }
        
    }

    IEnumerator PlayFootParticle()
    {
        var emission = footEmission.emission;
        emission.rateOverTime = 35f;

        wasJustStopped = false; 
        //Debug.Log("Foot emission");
        footEmission.Play();
        //doesn't need to play for very long 

        yield return new WaitForSeconds(0.2f);

        startFootEmission = false;
        emission.rateOverTime = 0f;
    }

    public void SetTouchingWall(bool state)
    {
        firstWallTouch = state;
    }

    public bool GetIsMoving()
    {
        return isMoving; 
    }

    public void PlayerBounceUp(float knockBackForce)
    {
        myRigidBody2D.velocity += new Vector2(0, knockBackForce);
    }
}
