using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeAndLoad : MonoBehaviour
{
    public UnityEvent LoadScene;
    public bool hasLoaded;
    public Image roomFader;
    private PlayerControl myPlayerControl;

    private void Start()
    {
        roomFader.color = new Color(roomFader.color.r, roomFader.color.g, roomFader.color.b, 0f);

        myPlayerControl = GameObject.FindObjectOfType<PlayerControl>().GetComponent<PlayerControl>();
    }


    public void CallFadeLoad()
    {
        StartCoroutine(FadeLoad());
    }


    IEnumerator FadeLoad()
    {
        roomFader.gameObject.SetActive(true);

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            roomFader.color = new Color(roomFader.color.r, roomFader.color.g, roomFader.color.b, i);
            yield return null;
        }

        LoadScene.Invoke();

    }
}
