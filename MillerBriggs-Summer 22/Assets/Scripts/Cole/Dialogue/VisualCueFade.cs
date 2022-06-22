using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCueFade : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    //image to fade
    private SpriteRenderer img;

    [SerializeField] private float fadeTime;

    private void Awake()
    {
        //visualCue.SetActive(false);
        img = visualCue.GetComponent<SpriteRenderer>();

        //start transparent
        img.color = new Color(1, 1, 1, 0);
    }

    //false for fade in, true for fade out
    IEnumerator FadeImage(bool fadeAway)
    {
        float alpha = 0;
        Debug.Log("Fading");
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                alpha = (i - 0) / (fadeTime - 0); //Normalize value between 0 and 1

                // set color with i as alpha
                img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

                yield return null;
            }
            //guarantee it equals 0 
            img.color = new Color(1, 1, 1, 0);
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= fadeTime; i += Time.deltaTime)
            {
                // set color with i as alpha
                alpha = (i - 0) / (fadeTime - 0); //Normalize value between 0 and 1

                // set color with i as alpha
                img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

                yield return null;
            }

            img.color = new Color(1, 1, 1, 1);
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(FadeImage(false));
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(FadeImage(true));
        }
    }

}
