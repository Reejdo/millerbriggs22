using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] private Vector3 shrunk;
    [SerializeField] private Vector3 normalSize; 
    [SerializeField] private float zoomTime = 0.5f;
    [SerializeField] private Transform panelParent;

    private bool panelToGrow = false;
    private bool panelToShrink = false;

    void Update()
    {
        if (panelToGrow)
        {
            panelParent.localScale = Vector3.Lerp(panelParent.localScale, normalSize, Time.deltaTime * zoomTime);
            Debug.Log("growing"); 
        }
        
        if (panelToShrink)
        {
            panelParent.localScale = Vector3.Lerp(panelParent.localScale, shrunk, Time.deltaTime * zoomTime);
            Debug.Log("shrinking");
        }
    }

    public IEnumerator PanelGrow()
    {
        //set local scale to 0
        panelParent.localScale = shrunk;
        panelToGrow = true;

        yield return new WaitForSeconds(zoomTime); 

        panelToGrow = false; 
    }

    public IEnumerator PanelShrink()
    {
        //set local scale to 0
        panelParent.localScale = normalSize;
        panelToShrink = true;

        yield return new WaitForSeconds(zoomTime);

        panelToShrink = false;
    }

}
