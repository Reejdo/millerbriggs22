using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollideAction : MonoBehaviour
{
    public UnityEvent myEvent;

    [Header("DEFAULT TRUE, CHANGE FOR MULTIPLE EVENTS")]
    public bool onlyActivateOnce = true;
    private bool hasActiavted; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (onlyActivateOnce)
            {
                if (!hasActiavted)
                {
                    hasActiavted = true;
                    TriggerEvent(); 
                }
            }
            else
            {
                TriggerEvent(); 
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (onlyActivateOnce)
            {
                if (!hasActiavted)
                {
                    hasActiavted = true;
                    TriggerEvent();
                }
            }
            else
            {
                TriggerEvent();
            }
        }
    }

    void TriggerEvent()
    {
        myEvent.Invoke(); 
    }
}
