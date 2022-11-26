using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public bool toggleOn = true;
    private float length;
    private float startPos;
    public GameObject myCam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (toggleOn == false)
        {
            gameObject.SetActive(false);
        }

        startPos = transform.position.x;

        length = GetComponent<SpriteRenderer>().bounds.size.x;

        if (GetComponent<SpriteRenderer>() == null)
        {
            length = 20;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (toggleOn == true)
        {
            gameObject.SetActive(true);
        }


        float temp = (myCam.transform.position.x * (1 - parallaxEffect));
        float dist = (myCam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }

    }
}
