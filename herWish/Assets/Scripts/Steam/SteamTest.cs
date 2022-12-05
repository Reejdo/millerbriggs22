using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.Log("Make sure SteamManager is initialized"); 
            return; 
        }

        string name = SteamFriends.GetPersonaName();
        Debug.Log(name); 
    }
}
