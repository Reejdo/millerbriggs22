using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionVision : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("SET ZOOM-IN AS TRUE OR FALSE")]
    public bool zoomIn;
    
    public bool playerActivate;
    public bool transitionOver; 
    //Only need one of these
    public GameObject[] objToChange;
    public GameObject nextRoom; 
    public Transform visionUI;
    public float zoomedOutScale = 10f;
    private float fadeSpeed = 0;
    public float fadeTime = 0f;
    float zoomInScale = 10, zoomOutScale = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (playerActivate)
        {
            if (zoomIn && zoomInScale < 1.1)
            {
                transitionOver = true;
            }
            else if (!zoomIn && zoomOutScale >= (zoomedOutScale - 0.1))
            {

                transitionOver = true;
            }
        }

        if (playerActivate)
        {
            //zoom in to 1f scale
            if (zoomIn && !transitionOver)
            {
                if (visionUI.localScale.x > 1f && !transitionOver)
                {
                    zoomInScale = Mathf.SmoothDamp(visionUI.localScale.x, 1f, ref fadeSpeed, fadeTime);
                    visionUI.localScale = new Vector3(zoomInScale, zoomInScale, visionUI.localScale.z);
                    SetObjStates(true); 

                }

                else
                {
                    transitionOver = true; 
                }
            }
            //zoom out to 10f scale
            else if (!zoomIn && !transitionOver)
            {
                SetObjStates(false); 

                if (nextRoom != null)
                {
                    nextRoom.SetActive(true); 
                }

                if (visionUI.localScale.x < zoomedOutScale)
                {

                    zoomOutScale = Mathf.SmoothDamp(visionUI.localScale.x, zoomedOutScale, ref fadeSpeed, fadeTime);
                    visionUI.localScale = new Vector3(zoomOutScale, zoomOutScale, visionUI.localScale.z);
                }
                else
                {
                    transitionOver = true;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerActivate = true; 
        }
    }

    void SetObjStates(bool state)
    {
        foreach (GameObject obj in objToChange)
        {
            obj.SetActive(state); 
        }
    }

}
