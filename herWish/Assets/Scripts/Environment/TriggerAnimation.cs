using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator myAnim; 

    public void TriggerAnim()
    {
        myAnim.SetTrigger("trigger"); 
    }
}
