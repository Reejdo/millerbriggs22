using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Dummy class since the script is static
    // Need to load coroutine
    private class LoadingMonoBehaviour : MonoBehaviour { }

    public enum Scene
    {
        MainMenu,
        Loading,
        Scene_Opening,
        Scene_DreamCampus_v1,
        Scene_DreamCampus_v2, 
        Scene_CortexTower, 
        Scene_ECol_v1, 
        Scene_ECol_v2,
        Scene_GunTown, 
        Scene_Gun, 
        Scene_GunBoss,
        CutScene,
    }

    private static Action onLoaderCallback; //delegate that returns void
    private static AsyncOperation loadingAsyncOperation; 

    public static void Load(Scene scene)
    {
        // Set the loader callback action to load the target scene
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene)); 
            LoadSceneAsync(scene);
        };

        // Load the loading scene
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return new WaitForSeconds(2f); //wait 2 second before continuing

        yield return null; 

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString()); 

        //while the async operation isn't done and is still loading
        while (!loadingAsyncOperation.isDone)
        {
            yield return null; 
        }
    }

    //returns value between 0 and 1 for progress of loading scene
    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress; 
        }
        else
        {
            return 1f; 
        }
    }


    public static void LoaderCallback()
    {
        // Triggered after first update which lets the screen refresh
        // Execute loader callback action, which will load the target scene
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null; 
        }
    } 

}
