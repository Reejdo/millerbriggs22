using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health; 
    [SerializeField] private GameObject[] healthImages;
    [SerializeField] private Transform respawnLocation;

    [SerializeField] private float hurtAnimLength, flashInterval;
    private float playerHitTimer, flashIntervalTimer; 
    private int flashNumber = 0;
    private Animator anim;
    [SerializeField] private bool canBeHurt = true, isHurt = false;


    public Material playerSpriteMaterial; 

    public bool dead;
    private bool beganDeath;
    [SerializeField] private float deathTime = 2f;
    [SerializeField] private TriggerLoadScene myTriggerScene;

    private AudioManager myAudioManager; 
    [SerializeField] private Sounds healSound, hurtSound, deathSound;
    [SerializeField] private float shakeIntensity, shakeTime; 

    // Start is called before the first frame update
    void Start()
    {
        RespawnHealth();

        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {

        if (myAudioManager == null)
        {
            myAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>(); 
        }


        if (isHurt)
        {
            playerHitTimer += Time.deltaTime;
            flashIntervalTimer += Time.deltaTime;

            //flashes transparent
            if (flashIntervalTimer >= flashInterval)
            {
                //Debug.Log("Flash Num: " + flashNumber);
                if (flashNumber == 0)
                {
                    //Debug.Log("Set flash number to 1"); 
                    flashNumber = 1;
                    flashIntervalTimer = 0;
                }
                else
                {
                    //Debug.Log("Set flash number to 0");
                    flashNumber = 0;
                    flashIntervalTimer = 0;
                }
            }


            if (playerHitTimer > hurtAnimLength)
            {
                flashNumber = 1; 
                //Make sure it ends with full alpha
                canBeHurt = true;
                isHurt = false;
                playerHitTimer = 0;
                flashIntervalTimer = 0;
                flashNumber = 0;
            }

            anim.SetInteger("flashNum", flashNumber);

        }

        if (health <= 0 && !beganDeath)
        {
            beganDeath = true;
            dead = true;
            StartCoroutine(Death()); 
        }
    }

    IEnumerator Death()
    {
        anim.SetTrigger("death");
        myAudioManager.Play(deathSound); 

        yield return new WaitForSeconds(deathTime);

        myTriggerScene.LoadNextScene();
    }


    void PlayerHurt()
    {
        if (!dead)
        {
            CineMachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime); 
            playerHitTimer = 0; 
            isHurt = true;
            flashNumber = 1;
            anim.SetInteger("flashNum", flashNumber);
            healthImages[health - 1].SetActive(false);
            health--;
            myAudioManager.Play(hurtSound); 
        }
    }

    void RespawnHealth()
    {
        health = maxHealth;
        foreach (GameObject heart in healthImages)
        {
            heart.SetActive(true);
        }
    }

    void ResetMaxHealth()
    {
        maxHealth = 0; 

        foreach (GameObject heart in healthImages)
        {
            maxHealth++;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void AddHealth()
    {
        if (health != maxHealth)
        {
            health++;
            healthImages[health - 1].SetActive(true);
            myAudioManager.Play(healSound); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (canBeHurt)
            {
                canBeHurt = false;
                PlayerHurt();
            }

        }
    }

}
