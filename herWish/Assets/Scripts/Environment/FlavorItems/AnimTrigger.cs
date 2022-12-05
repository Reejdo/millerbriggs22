using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("ENTER DEFAULT, EXIT IF OFF")]
    public bool isEnterTrigger = true;

    [SerializeField]
    private TriggerTypes myTriggerType;

    public Animator myAnim;
    private bool trigger; 

    private void Update()
    {
        myAnim.SetBool(myTriggerType.ToString(), trigger); 
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FlavorTrigger"))
        {
            if (isEnterTrigger)
            {
                Debug.Log("Entered animal close!"); 
                trigger = true; 
            }
            else if (!isEnterTrigger)
            {
                trigger = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FlavorTrigger"))
        {
            if (!isEnterTrigger)
            {
                //Debug.Log("Exit - " + gameObject.name); 
                trigger = true; 
            }
            else if (isEnterTrigger)
            {
                trigger = false; 
            }
        }
    }


    enum TriggerTypes
    {
        close, 
        far
    }

}
