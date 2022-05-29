using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    //[SerializeField] private Vector3 shrunk;
    //[SerializeField] private Vector3 normalSize; 
    //[SerializeField] private Transform panelParent;

    //private bool panelToGrow = false;
    //private bool panelToShrink = false;
    [SerializeField] private Animator panelParentAnimator; 


    void Update()
    {
        /*
        if (panelToGrow)
        {
            panelParent.localScale = Vector3.Lerp(panelParent.localScale, normalSize, Time.deltaTime * growTime);
            Debug.Log("growing"); 
        }
        
        if (panelToShrink)
        {
            panelParent.localScale = Vector3.Lerp(panelParent.localScale, shrunk, Time.deltaTime * shrinkTime);
            Debug.Log("shrinking");
        }
        */ 
    }

    public void PanelGrow()
    {
        
    }

    public void PanelShrink()
    {

    }

}
