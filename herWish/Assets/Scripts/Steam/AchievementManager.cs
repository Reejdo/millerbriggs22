using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [System.Serializable]
    public struct achievementID
    {
        public string steamID; 
    }

    [System.Serializable]
    public struct statID
    {
        public string steamID;
    }

    [SerializeField]
    private achievementID[] achieveIDs;

    [SerializeField]
    private statID[] statIDs;

    bool isAchievementUnlocked;
    int statValue; 

    public void UnlockAchievement(int _index)
    {
        TestSteamAchievement(achieveIDs[_index].steamID);

        Debug.Log("Achievement with ID: " + achieveIDs[_index].steamID + " unlocked = " + isAchievementUnlocked); 

        if (!isAchievementUnlocked)
        {
            SteamUserStats.SetAchievement(achieveIDs[_index].steamID); 
            
            //Make sure to store so that game updates while running and not just while closing
            SteamUserStats.StoreStats();
        }
    }

    void TestSteamAchievement(string _id)
    {
        //Need to do this, can't set any achievements until a callback has been received
        SteamUserStats.GetAchievement(_id, out isAchievementUnlocked); 
    }

    void TestSteamStat(string _id)
    {
        SteamUserStats.GetStat(_id, out statValue);
    }

    public void DebugRelockAchievement(int _index)
    {
        TestSteamAchievement(achieveIDs[_index].steamID);
        Debug.Log("Achievement with ID: " + achieveIDs[_index].steamID + " unlocked = " + isAchievementUnlocked);

        SteamUserStats.ClearAchievement(achieveIDs[_index].steamID);
        SteamUserStats.StoreStats();
    }

    public void IncrementStat(int _index)
    {
        TestSteamStat(statIDs[_index].steamID); 
        //Debug.Log("Stat with ID: " + statIDs[_index].steamID + " current value = " + statValue);

        statValue++; 

        SteamUserStats.SetStat(statIDs[_index].steamID, statValue);
        SteamUserStats.StoreStats();

        //Debug.Log("Stat with ID: " + statIDs[_index].steamID + " new value = " + statValue);

    }

    public void GetAchievement(int _index)
    {
        bool isUnlocked; 

        SteamUserStats.GetAchievement(achieveIDs[_index].steamID, out isUnlocked);

        //Debug.Log("Acheivement is " + isUnlocked); 
    }

}
