using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PControlOld : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D;

    [Header("[Finding Ground]")]
    public float groundCheckRadius = 0.1f; 
    public Transform groundCheckPoint, groundCheckPoint2;
    public LayerMask whatIsGround;
    [SerializeField]
    private bool isGrounded;

    private Animator myAnim;
    public SpriteRenderer playerSpriteRender;

    [Header("[Movement]")]
    [SerializeField]
    private float moveSpeed;
    private int facingDirection; //facing left or right


    [Header("[Jumping]")]
    public KeyCode jumpKeyCode;
    public float jumpForce;
    //time after player leaves platform, 0.2 seconds after you walk over the edge to jump
    public float currentJumps = 2;
    public float maximumJumps = 2; 
    public float coyoteTime = 0.1f;
    private float hangCounter;

    //buffer length so you can jump a little bit before you hit the ground if you pressed it before the ground
    public float jumpBufferLength = 0.1f;
    public float jumpBufferCount;

    [Header("[Camera]")]
    public Transform camTarget;
    public float camAheadAmount, camAheadSpeed;


    [Header("[Particles]")]
    public ParticleSystem footSteps;
    public ParticleSystem impactEffect; 
    public ParticleSystem dashEffect;
    //if we want to change emission
    private ParticleSystem.EmissionModule footEmission;
    //lets us know that we were on the ground for impact effect
    private bool wasOnGround; 

    //So the impact doesn't happen for small distances
    public float minImpactTime = 1f; 
    private float fallTimeForImpact;

    [Header("[Dashing]")]
    [SerializeField]
    public KeyCode dashKeyCode;
    [SerializeField]
    private float dashTime, dashSpeed, dashCooldown;
    private int dashDirX, dashDirY; 
    private float dashTimeLeft; 
    private float distanceBetweenImages;
    private float lastImageXPos;
    private float lastDashTime = -100;


    [Header("[Booleans]")]
    public bool canMove = true;
    public bool canJump = true;
    public bool canDash = true; 
    public bool isDashing = false;
    public bool isTouchingWall = false;

    // Start is called before the first frame update
    void Start()
    {
        SetJumpCountMax(); 
        myRigidBody2D = GetComponent<Rigidbody2D>();
        footEmission = footSteps.emission;

        facingDirection = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded();

        Jump();
        PlayerFacingDirection();
        DashInputCheck();
        MinusJumpCount();

        CheckDialogue(); 
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }

        CheckDash();
    }

    void Move()
    {
        myRigidBody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidBody2D.velocity.y); 

        //move camera point
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, camAheadAmount * Input.GetAxisRaw("Horizontal"), 
                camAheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z); 
        }

        //footstep effect
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

        //make it so the impact effect happens only after a certain amount of fall time
        if (!isGrounded)
        {
            fallTimeForImpact += Time.deltaTime; 
        }
        else if (isGrounded && fallTimeForImpact > 0f)
        {
            fallTimeForImpact = 0f; 
        }

        //update wasOnGround at the end of the grame
        wasOnGround = isGrounded; 
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

    void Jump()
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

    void DashInputCheck()
    {
        if (Input.GetKeyDown(dashKeyCode))
        {
            if (canDash)
            {
                AttemptToDash();
            }
        }
    }

    void AttemptToDash()
    {
        canDash = false;    
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDashTime = Time.time;

        dashEffect.gameObject.SetActive(true);
        dashEffect.Play(); 

        SetDashDirection(); 
    }

    void SetDashDirection()
    {
        //Determine what input the player is pressing
        if (Input.GetAxisRaw("Horizontal") > 0.1)
        {
            dashDirX = 1;
            //update facing direction
            facingDirection = 1; 
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.1)
        {
            dashDirX = -1;
            //update facing direction
            facingDirection = -1; 
        }
        else
        {
            dashDirX = 0;
        }

        if (Input.GetAxisRaw("Vertical") > 0.1)
        {
            dashDirY = 1;
        }
        else if (Input.GetAxisRaw("Vertical") < -0.1)
        {
            dashDirY = -1;
        }
        else
        {
            dashDirY = 0;
        }

        //If the player hasn't pressed an input
        if (dashDirX == 0 && dashDirY == 0)
        {
            if (!isGrounded)
            {
                dashDirY = 1;
            }
            else
            {
                dashDirX = facingDirection;
            }
        }

    }

    void CheckDash()
    {
        //Checks dash every frame

        if (isDashing)
        {
            canDash = false; 

            if (dashTimeLeft > 0)
            {
                canJump = false;
                canMove = false;
                Vector2 moveHorz = transform.right * dashDirX; 
                Vector2 moveVert = transform.up * dashDirY; 
                
                myRigidBody2D.velocity = (moveHorz + moveVert).normalized * dashSpeed;

                dashTimeLeft -= Time.deltaTime;
                dashEffect.transform.position = gameObject.transform.position;
            }
        } 

        if ((dashTimeLeft <= 0 || isTouchingWall) && isDashing)
        {
            StartCoroutine(DashEffectFade());
            myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, 0f); 
            //dashEffect.Stop();
            isDashing = false;
            canMove = true;
            canJump = true;
        }

        if (isGrounded)
        {
            canDash = true; 
        }
    }

    IEnumerator DashEffectFade()
    {
        dashEffect.Stop();
        yield return new WaitForSeconds(0.25f);
        dashEffect.gameObject.SetActive(false); 

    }

    //returns value of player grounded state
    bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround) || 
            Physics2D.OverlapCircle(groundCheckPoint2.position, groundCheckRadius, whatIsGround);
    }

    //subtract from the jump count
    void MinusJumpCount()
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

    //if dialogue is playing, player can't move
    void CheckDialogue()
    {
        if (DialogueManager.instance.dialogueIsPlaying)
        {
            canMove = false;
            canJump = false;
            canDash = false;
        }
        else
        {
            canMove = true;
            canJump = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckPoint2.position, groundCheckRadius);
    }

}