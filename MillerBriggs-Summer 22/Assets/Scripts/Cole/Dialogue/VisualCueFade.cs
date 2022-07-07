using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCueFade : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    //image to fade
    private SpriteRenderer img;

    private float fade; 
    [SerializeField] private float fadeTime; 
    private float fadeSpeed = 0.0f;
    [SerializeField]
    private bool fadeIn = false; 

    private void Awake()
    {

        //visualCue.SetActive(false);
        img = visualCue.GetComponent<SpriteRenderer>();

        //start transparent
        img.color = new Color(1, 1, 1, 0);
    }


    private void Update()
    {
        if (fadeIn)
        {
            if (img.color.a != 1f)
            {
                Debug.Log("Fade in!");
                fade = Mathf.SmoothDamp(img.color.a, 1f, ref fadeSpeed, fadeTime);
                img.color = new Color(1f, 1f, 1f, fade);
            }            
        }

        if (!fadeIn)
        {
            if (img.color.a != 0f)
            {
                Debug.Log("Fade out!"); 
                fade = Mathf.SmoothDamp(img.color.a, 0f, ref fadeSpeed, fadeTime);
                img.color = new Color(1f, 1f, 1f, fade);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //StartCoroutine(FadeImage(false));
            fadeIn = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //StartCoroutine(FadeImage(true));
            fadeIn = false; 
        }
    }

}
