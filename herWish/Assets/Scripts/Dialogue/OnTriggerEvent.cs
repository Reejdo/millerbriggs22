using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private bool hasTriggered;
    public UnityEvent myEvent;


    void TriggerEvent()
    {
        myEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            TriggerEvent(); 
        }
    }
}
