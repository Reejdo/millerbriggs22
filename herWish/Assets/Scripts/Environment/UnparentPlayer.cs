using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentPlayer : MonoBehaviour
{
    private GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject == null)
        {
            Debug.Log("NULL");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Unparent");
            playerObject.transform.SetParent(null);
        }
    }
}
