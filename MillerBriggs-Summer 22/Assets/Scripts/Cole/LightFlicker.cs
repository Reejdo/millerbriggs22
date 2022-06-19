using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public Transform mainLight;
    public Transform flickerLight;
    private Light2D mainLightComponent;
    private Light2D flickerLightComponent;
    public float lowIntensity, highIntensity;
    public float maxFlickerTime; 

    // Start is called before the first frame update
    void Start()
    {
        mainLightComponent = mainLight.GetComponent<Light2D>();
        flickerLightComponent = flickerLight.GetComponent<Light2D>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        for (; ; ) //this is while(true)
        {
            float randomIntensity = Random.Range(lowIntensity, highIntensity);
            flickerLightComponent.intensity = randomIntensity;


            float randomTime = Random.Range(0f, maxFlickerTime);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
