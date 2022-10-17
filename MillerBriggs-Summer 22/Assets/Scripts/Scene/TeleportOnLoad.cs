using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private DataManager myDataManager;
    [SerializeField]
    private Transform[] checkPoints;
    private Transform player, mainCamera, cameraFollow; 

    void Start()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        player = GameObject.Find("player").GetComponent<Transform>();
        mainCamera = GameObject.FindObjectOfType<Camera>().GetComponent<Transform>();
        cameraFollow = GameObject.FindGameObjectWithTag("CameraFollow").GetComponent<Transform>(); 

        TeleportPlayer(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TeleportPlayer()
    {
        player.position = checkPoints[myDataManager.lastCheckpoint].position;
        mainCamera.position = new Vector3(cameraFollow.position.x, cameraFollow.position.y, mainCamera.position.z); 
        
    }
}
