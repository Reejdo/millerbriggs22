using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class OutsideDoor : MonoBehaviour
{
    public bool isInRange;
    public bool hasOpenedDoor = false;
    private bool isFading;

    public SpriteRenderer doorSprite, visualCue;
    public Light2D doorLight; 
    public float fadeTime;
    private float fade;
    private float fadeSpeed = 0.0f;
    private Animator myAnim;
    private TriggerLoadScene myTriggerScene; 

    // Start is called before the first frame update
    void Start()
    {
        doorLight.intensity = 0f;
        myAnim = GetComponent<Animator>();
        myTriggerScene = GetComponent<TriggerLoadScene>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasOpenedDoor)
        {
            CheckLights();
        }


        if (isInRange && doorSprite.color.a > 0.8f && !hasOpenedDoor)
        {
            if (InputManager.GetInstance().GetInteractPressed())
            {
                hasOpenedDoor = true;
                StartCoroutine(FadeAndLoad());
            }
        }
        
        myAnim.SetBool("openedDoor", hasOpenedDoor); 
    }

    void CheckLights()
    {
        if (isInRange)
        {
            if (doorSprite.color.a != 1f)
            {
                //Debug.Log("Fade in!");
                fade = Mathf.SmoothDamp(doorSprite.color.a, 1f, ref fadeSpeed, fadeTime);
            }
        }

        if (!isInRange)
        {
            if (doorSprite.color.a != 0f)
            {
                //Debug.Log("Fade out!");
                fade = Mathf.SmoothDamp(doorSprite.color.a, 0f, ref fadeSpeed, fadeTime);
            }
        }


        //fade in the door, door light, and the visual cue
        doorLight.intensity = fade; 
        doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, fade);
        visualCue.color = new Color(1f, 1f, 1f, fade);

    }

    IEnumerator FadeAndLoad()
    {
        hasOpenedDoor = true; 
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, i);
            doorLight.intensity = i; 
            yield return null;

        }

        myTriggerScene.LoadNextScene();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
