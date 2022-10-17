using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GainAmmo : MonoBehaviour
{
    public Image uiImage; //Regular bar
    public float increment, currentAmmoPoint = 0;
    public int gunToAddAmmo = 1;
    private Shooting myShooting; 

    // Start is called before the first frame update
    void Start()
    {
        myShooting = GetComponent<Shooting>();

        currentAmmoPoint = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHPBar(); 
    }

    public void IncrementAmmoGain()
    {
        Debug.Log("Increment");

        float checkAmmo = currentAmmoPoint + increment; 

        if (checkAmmo >= 1 && currentAmmoPoint != 0)
        {
            currentAmmoPoint = 0;
            myShooting.GainAmmo(gunToAddAmmo); 
        }
        else
        {
            currentAmmoPoint += increment; 
        }
    }

    void DisplayHPBar()
    {
        uiImage.fillAmount = currentAmmoPoint / 1f;
    }

}
