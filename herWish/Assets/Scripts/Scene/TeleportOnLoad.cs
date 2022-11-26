using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform player, mainCamera, cameraFollow; 
    private DataManager myDataManager;
    private bool hasTeleportedPlayer; 

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        if (mainCamera == null)
        {
            mainCamera = GameObject.FindObjectOfType<Camera>().GetComponent<Transform>();
        }

        if (cameraFollow == null)
        {
            cameraFollow = GameObject.FindGameObjectWithTag("CameraFollow").GetComponent<Transform>();
        }


        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>(); 
    }

    private void Update()
    {
        if (myDataManager != null)
        {
            if (myDataManager.GetPosLoadState() && !hasTeleportedPlayer)
            {
                hasTeleportedPlayer = true; 
                TeleportPlayer(myDataManager.GetPlayerSavedPosition()); 
            }
        }
        else if (myDataManager == null)
        {
            myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        }
    }


    public void TeleportPlayer(Vector2 playerPos)
    {
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        player.position = new Vector2(playerPos.x, playerPos.y); 
        mainCamera.position = new Vector3(cameraFollow.position.x, cameraFollow.position.y, mainCamera.position.z);

    }

    public bool TeleportedPlayer()
    {
        return hasTeleportedPlayer; 
    }
}
