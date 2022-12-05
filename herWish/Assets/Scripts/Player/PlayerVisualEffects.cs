using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour
{
    public ParticleSystem jumpParticles;
    public ParticleSystem groundedParticles; 

    private void Start()
    {
        SetJumpParticleState(false); 
    }


    public void SetJumpParticleState(bool state)
    {
        if (state == false) 
        {
            jumpParticles.Stop(); 
        }


        if (state == true)
        {
            jumpParticles.Play(); 
        }
    }

    public void SetGroundedParticleState(bool state)
    {
        groundedParticles.gameObject.SetActive(state);
    }

    public void PlayGroundParticles()
    {
        groundedParticles.Play();
    }
}
