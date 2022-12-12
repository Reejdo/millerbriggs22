using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class PressCompleteKey : MonoBehaviour
{
    public UnityEvent CompleteEvent;
    private bool hasInvoked; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !hasInvoked) 
        {
            hasInvoked = true;
            CompleteEvent.Invoke(); 
        }
    }
}
