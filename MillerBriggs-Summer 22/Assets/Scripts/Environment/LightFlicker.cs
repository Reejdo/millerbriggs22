using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public Light2D flickerLightComponent;
    public float lowIntensity, highIntensity;
    public float maxFlickerTime;
    public bool flickerOn = true; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (flickerOn)
        {
            float randomIntensity = Random.Range(lowIntensity, highIntensity);
            flickerLightComponent.intensity = randomIntensity;


            float randomTime = Random.Range(0f, maxFlickerTime);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
