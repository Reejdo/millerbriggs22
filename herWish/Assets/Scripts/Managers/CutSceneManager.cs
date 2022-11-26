using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class CutSceneManager : MonoBehaviour
{

    [SerializeField] private UnityEvent loadSceneEvent; 
    [SerializeField] private float sceneTime;
    private float timer;
    private bool loadOnce; 

    // Update is called once per frame
    void Update()
    {
        if (timer < sceneTime) 
        {
            timer += Time.deltaTime; 
        }

        else if (timer > sceneTime && !loadOnce) 
        {
            loadOnce = true;
            loadSceneEvent.Invoke(); 
        }
    }
}
