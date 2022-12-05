using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerLaunch : MonoBehaviour
{
    private float holdStartTime;
    [SerializeField] private float launchForce;
    [SerializeField] float MAX_FORCE_HOLD_TIME = 2f;
    [SerializeField] float MIN_FORCE = 1f; 
    [SerializeField] float MAX_FORCE = 25f;

    //[SerializeField] private float bufferBetweenJumps = 0.5f;
    //private float bufferTimer;

    [SerializeField] private float min_stretch_time = 1f;
    [SerializeField] private float min_effect_time = 0.2f; 

    private float bufferJustLaunched = 0.1f;
    private float justLaunchedTimer;
    private bool justLaunched;
    public bool mouseJustDown = false; 

    [SerializeField] float initialGColorValue = 70f, initialBColorValue = 1f; 
    private float iGColorNorm; 

    public Image jumpDirector, jumpDirBackground;

    private PlayerControl myJumpPlayer;
    private Vector3 mousePosition;
    private Camera mainCamera;


    private AudioManager myAudioManager;
    [SerializeField] private Sounds s_revJump, s_Jump;
    [SerializeField] private Animator mySpriteAnim;
    private PlayerVisualEffects myPlayerEffects; 

    // Start is called before the first frame update
    void Start()
    {
        myJumpPlayer = GetComponent<PlayerControl>();
        myPlayerEffects = GetComponent<PlayerVisualEffects>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); 

        jumpDirector.gameObject.SetActive(false);
        jumpDirBackground.gameObject.SetActive(false);

        iGColorNorm = initialGColorValue / 255f; 
    }

    // Update is called once per frame
    void Update()
    {
        if (myJumpPlayer.isLaunching && !myJumpPlayer.GetIsGrounded())
        {
            myJumpPlayer.isLaunching = false;
            jumpDirector.gameObject.SetActive(false);
            jumpDirBackground.gameObject.SetActive(false);
        }


        //set isJumping back to false once player touches ground
        if (myJumpPlayer.GetIsGrounded() && myJumpPlayer.isJumping && !justLaunched)
        {
            myJumpPlayer.isJumping = false;
            myJumpPlayer.SetPlayerVelocityZero(); 
        }

        //make sure player isn't faceplanted before launching
        if (!myJumpPlayer.GetPlayerFacePlant())
        {
            //make sure player is grounded
            if (myJumpPlayer.OneGroundedTrue())
            {
                //initial press
                if (Input.GetMouseButtonDown(0) && myJumpPlayer.GetIsGrounded())
                {
                    holdStartTime = Time.time;
                    mouseJustDown = true; 
                }

                //make sure mouse was held when player was on ground
                if (Input.GetMouseButton(0))
                {
                    float holdDownTime = Time.time - holdStartTime;

                    //Update the jump director's color
                    UpdateDirectorColor(holdDownTime);

                    //only start if player is above a minimum threshold
                    if (CalculateHoldForce(holdDownTime) > MIN_FORCE)
                    {
                        jumpDirector.gameObject.SetActive(true);
                        jumpDirBackground.gameObject.SetActive(true);

                        //make sure player can't jump
                        myJumpPlayer.SetMoveState(false);

                        if (!myJumpPlayer.isLaunching)
                        {
                            myJumpPlayer.isLaunching = true;
                            myAudioManager.Play(s_revJump, true); 
                        }

                        myJumpPlayer.SetPlayerVelocityZero();
                    }

                }

                if (Input.GetMouseButtonUp(0))
                {
                    float holdDownTime = Time.time - holdStartTime;

                    if (CalculateHoldForce(holdDownTime) > MIN_FORCE)
                    {
                        LaunchPlayer(CalculateHoldForce(holdDownTime), holdDownTime);

                        myAudioManager.StopSound(s_revJump); 
                        myAudioManager.Play(s_Jump, true); 

                        myJumpPlayer.isLaunching = false;
                        myJumpPlayer.SetMoveState(true);
                        jumpDirector.gameObject.SetActive(false);
                        jumpDirBackground.gameObject.SetActive(false);

                        justLaunched = true;
                        mouseJustDown = false;
                    }
                }
            }
            else
            {
                holdStartTime = Time.time;
            }

            //makes sure just launched isn't instantly false because of initial ground collision
            if (justLaunched)
            {
                justLaunchedTimer += Time.deltaTime; 

                if (justLaunchedTimer > bufferJustLaunched)
                {
                    justLaunched = false;
                    justLaunchedTimer = 0;

                    myJumpPlayer.justLaunched = true;
                }
            }
        }
        else
        {
            holdStartTime = Time.time;
            jumpDirector.gameObject.SetActive(false);
            jumpDirBackground.gameObject.SetActive(false);
        }

    }

    private float CalculateHoldForce(float holdTime)
    {
        //clamp 01 makes sure value stays between 0 and 1 
        float holdTimeNormalized = Mathf.Clamp01(holdTime / MAX_FORCE_HOLD_TIME);

        launchForce = holdTimeNormalized * MAX_FORCE;

        return launchForce; 
    }


    public void LaunchPlayer(float force, float holdDownTime)
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); 
        Vector3 direction = mousePosition - transform.position;

        //Debug.Log(direction); 

        if (direction.y > 0)
        {
            if (holdDownTime > min_stretch_time)
            {
                mySpriteAnim.SetTrigger("stretch");
            }

            if (holdDownTime > min_effect_time)
            {
                myPlayerEffects.SetJumpParticleState(true);
            }
        }


        myJumpPlayer.isJumping = true;

        //normalizing this makes sure that it doesn't increase or decrease speed depending on mouse position on screen
        myJumpPlayer.myRigidBody2D.velocity = new Vector2(direction.x, direction.y).normalized * force;

        if (direction.x > 0)
        {
            myJumpPlayer.SetJumpFacingDirection(1); 
        }
        else if (direction.x < 0)
        {
            myJumpPlayer.SetJumpFacingDirection(-1);
        }
    }

    void UpdateDirectorColor(float holdTime)
    {
        float holdTimeNormalized = Mathf.Clamp01(holdTime / MAX_FORCE_HOLD_TIME);

        float gColor = 1 - holdTimeNormalized; 

        float fillAmount = holdTimeNormalized;

        jumpDirector.fillAmount = fillAmount; 

        if (gColor >= 0.5f)
        {
            jumpDirector.color = new Color(iGColorNorm, gColor, initialBColorValue, jumpDirector.color.a);
        }

        //Change to red hue at the ladder half
        else if (gColor < 0.5f)
        {
            jumpDirector.color = new Color(1f, gColor, initialBColorValue, jumpDirector.color.a);
        }

        if (gColor <= 0)
        {
            jumpDirector.color = new Color(1f, .12f, .12f, jumpDirector.color.a);
        }
    }

}
