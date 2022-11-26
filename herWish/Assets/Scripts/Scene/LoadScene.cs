using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void LoadMenu()
    {
        Debug.Log("Load Main Menu");
        SceneLoader.Load(SceneLoader.Scene.Menu);
    }

    public void LoadScene1()
    {
        Debug.Log("Load Scene One");
        SceneLoader.Load(SceneLoader.Scene.Scene_1);
    }

    public void LoadScene2()
    {
        Debug.Log("Load Scene Two");
        SceneLoader.Load(SceneLoader.Scene.Scene_2);
    }

    public void LoadScene3()
    {
        Debug.Log("Load Scene Three");
        SceneLoader.Load(SceneLoader.Scene.Scene_3);
    }

    public void LoadCutScene1()
    {
        Debug.Log("Load CutScene One");
        SceneLoader.Load(SceneLoader.Scene.CutScene_1);
    }

    public void LoadCutScene2()
    {
        Debug.Log("Load CutScene Two");
        SceneLoader.Load(SceneLoader.Scene.CutScene_2);
    }

    public void OnButtonQuit()
    {
        Application.Quit();
    }
}
