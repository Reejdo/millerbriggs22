using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Debug.Log("Load Main Menu");
        SceneLoader.Load(SceneLoader.Scene.MainMenu);
    }

    public void LoadOpeningScene()
    {
        Debug.Log("Load Level One");
        SceneLoader.Load(SceneLoader.Scene.Scene_Opening);
    }

    public void LoadDreamCampus_v1()
    {
        Debug.Log("Load Dream Campus v1");
        SceneLoader.Load(SceneLoader.Scene.Scene_DreamCampus_v1);
    }

    public void LoadDreamCampus_v2()
    {
        Debug.Log("Load Dream Campus v2");
        SceneLoader.Load(SceneLoader.Scene.Scene_DreamCampus_v2);
    }

    public void LoadCortexTower()
    {
        Debug.Log("Load Cortex Tower");
        SceneLoader.Load(SceneLoader.Scene.Scene_CortexTower);
    }

    public void Load_ECol_v1()
    {
        Debug.Log("Load ECol v1");
        SceneLoader.Load(SceneLoader.Scene.Scene_ECol_v1);
    }

    public void Load_ECol_v2()
    {
        Debug.Log("Load ECol v1");
        SceneLoader.Load(SceneLoader.Scene.Scene_ECol_v2);
    }

    public void LoadGunTownScene()
    {
        Debug.Log("Load Gun Town Scene");
        SceneLoader.Load(SceneLoader.Scene.Scene_GunTown);
    }


    public void LoadGunScene()
    {
        Debug.Log("Load Gun Scene");
        SceneLoader.Load(SceneLoader.Scene.Scene_Gun);
    }

    public void LoadGunBossScene()
    {
        Debug.Log("Load Gun Scene");
        SceneLoader.Load(SceneLoader.Scene.Scene_GunBoss);
    }

    public void LoadCutScene()
    {
        Debug.Log("Load CutScene scene");
        SceneLoader.Load(SceneLoader.Scene.CutScene);
    }

    public void OnButtonQuit()
    {
        Application.Quit();
    }
}
