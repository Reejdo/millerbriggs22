using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

public class CineMachineShake : MonoBehaviour
{
    public static CineMachineShake Instance { get; private set; }
    
    private CinemachineVirtualCamera cinemachineCam;
    private float shakeTimer;
    private float startIntensity;
    private float totalShakeTime; 

    private void Awake()
    {
        Instance = this; 
        cinemachineCam = GetComponent<CinemachineVirtualCamera>(); 
    }


    public void ShakeCamera(float intensity, float timer)
    {
        CinemachineBasicMultiChannelPerlin cPerlin = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cPerlin.m_AmplitudeGain = intensity;

        startIntensity = intensity;
        totalShakeTime = timer; 
        shakeTimer = timer; 
    }


    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0f)
        {

            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin cPerlin = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, shakeTimer / totalShakeTime); 
        }
        else
        {
            CinemachineBasicMultiChannelPerlin cPerlin = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cPerlin.m_AmplitudeGain = 0f; 
        }
    }
}
