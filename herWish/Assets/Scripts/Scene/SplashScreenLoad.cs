using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SplashScreenLoad : MonoBehaviour
{
    public int menuSceneNumber = 1; 
    private float timer;
    public float timeToWait = 3f; 

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; 

        if (timer > timeToWait)
        {
            SceneManager.LoadScene(menuSceneNumber); 
        }
    }
}
