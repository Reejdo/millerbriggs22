using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

//Enter is when mouse enters, Exit is when mouse exits object
public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator hoverAnim;
    public bool hoverOn; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverOn = true; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverOn = false; 
    }


    private void Update()
    {
        hoverAnim.SetBool("hoverOn", hoverOn); 
    }
}
