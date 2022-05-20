using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D;

    public float groundCheckRadius = 0.1f; 
    public Transform groundCheckPoint, groundCheckPoint2;
    public LayerMask whatIsGround;
    [SerializeField]
    private bool isGrounded;

    private Animator myAnim;
    public SpriteRenderer playerSpriteRender;

    [SerializeField]
    private float moveSpeed, jumpForce;

    public KeyCode jumpKeyCode;

    //time after player leaves platform, 0.2 seconds after you walk over the edge to jump
    public float coyoteTime = 0.1f;
    private float hangCounter;

    //buffer length so you can jump a little bit before you hit the ground if you pressed it before the ground
    public float jumpBufferLength = 0.1f;
    public float jumpBufferCount;

    public Transform camTarget;
    public float camAheadAmount, camAheadSpeed;

    public ParticleSystem footSteps, impactEffect;
    //if we want to change emission
    private ParticleSystem.EmissionModule footEmission;
    private bool wasOnGround; //lets us know that we were on the ground for impact effect

    //So the impact doesn't happen for small distances
    public float minImpactTime = 1f; 
    public float fallTimeForImpact;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        footEmission = footSteps.emission; 
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded();
        Jump();

    }

    private void FixedUpdate()
    {
        Move();
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

    void Jump()
    {
        //manage coyote time
        if (isGrounded)
        {
            hangCounter = coyoteTime;
        }
        else
        {
            hangCounter -= Time.deltaTime; 
        }

        //manage jump buffer
        if (Input.GetKeyDown(jumpKeyCode))
        {
            jumpBufferCount = jumpBufferLength; 
        }
        else
        {
            jumpBufferCount -= Time.deltaTime; 
        }


        //Make player jump
        if (jumpBufferCount >= 0 && hangCounter > 0)
        {
            Debug.Log("Jump"); 
            myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, jumpForce);
            jumpBufferCount = 0; 
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckPoint2.position, groundCheckRadius);
    }

}
