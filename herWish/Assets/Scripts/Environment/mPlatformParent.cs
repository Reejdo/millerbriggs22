using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mPlatformParent : MonoBehaviour
{
    public bool parentPlayer = true;

    public GameObject playerObject;
    public Transform parentObject;

    public float maxXDistance, maxYDistance;

    [SerializeField] float xDist, yDist;

    public MovePlatforms myMovePlatform;
    public int movePlatformNumber;

    public UpdateFilePlayerPos playerFileData;
    public TeleportOnLoad teleportLoad;

    private bool justParented = false;

    // Start is called before the first frame update

    private void Awake()
    {
        playerObject = GameObject.FindObjectOfType<PlayerControl>().gameObject;  

        playerFileData = GameObject.FindGameObjectWithTag("PlayerFileData").GetComponent<UpdateFilePlayerPos>();

        teleportLoad = GameObject.FindObjectOfType<TeleportOnLoad>().GetComponent<TeleportOnLoad>(); 
    }

    private void Start()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        }

        if (teleportLoad != null)
        {
            if (teleportLoad.TeleportedPlayer() == true)
            {
                Debug.Log("Player moved"); 
                if (myMovePlatform.playerParented[movePlatformNumber] == true)
                {
                    playerObject.transform.SetParent(parentObject);
                }
            }
        }
        else
        {
            Debug.LogError("Teleport load not found!"); 
        }

    }


    private void Update()
    {
        float xOriginalDist = Mathf.Abs(playerObject.transform.position.x) - Mathf.Abs(parentObject.transform.position.x);


        xDist = Mathf.Abs(xOriginalDist); 
        yDist = playerObject.transform.position.y - parentObject.transform.position.y;

        //only check if player is parented to this object's parent object
        if (playerObject.transform.parent == parentObject)
        {
            //Debug.Log("Parented!"); 

            //NEED TO UPDATE PLAYER POSITION FOR SAVING HERE SO IT LOADS CORRECTLY
            playerFileData.UpdatePlayerInfo();

            if (xDist > maxXDistance || Mathf.Abs(yDist) > maxYDistance)
            {
                Debug.Log("Dist higher");

                myMovePlatform.playerParented[movePlatformNumber] = false;

                playerObject.transform.SetParent(null);

                justParented = false; 
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") && parentPlayer))
        {
            if (collision.contacts[0].point.y > transform.position.y && !justParented)
            {
                justParented = true; 
                //Debug.Log("Contact point y: " + collision.contacts[0].point.y + " Transform: " + transform.position.y); 
                playerObject.transform.SetParent(parentObject);
                myMovePlatform.playerParented[movePlatformNumber] = true;
            }
        }
    }
}
