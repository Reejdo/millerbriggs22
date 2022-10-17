using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject popUpQuestion;
    public GameObject otherButtonsGrouping;

    private void Start()
    {
        popUpQuestion.SetActive(false);
    }

    public void InteractPopUp()
    {
        otherButtonsGrouping.SetActive(false); 

        if (popUpQuestion.activeSelf)
        {
            popUpQuestion.SetActive(false); 
        }
        else
        {
            popUpQuestion.SetActive(true); 
        }
    }

    public void EnableOtherButtons()
    {
        popUpQuestion.SetActive(false);
        otherButtonsGrouping.SetActive(true);
    }

}
