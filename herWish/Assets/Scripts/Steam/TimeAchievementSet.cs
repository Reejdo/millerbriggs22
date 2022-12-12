using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAchievementSet : MonoBehaviour
{
    public AchievementManager myAchievementManager;

    public float[] achievementTimes;
    public int[] achievementIndex;  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckTimeAchievements(float currentTime)
    {
        //make sure [0] is 30 minutes time
        if (currentTime <= achievementTimes[0]) 
        { 
            //Unlock all time achievements
            for (int i = 0; i < achievementTimes.Length; i++)
            {
                myAchievementManager.UnlockAchievement(achievementIndex[i]);
            }
            return; 
        }
        //Greater than 30 min less than 34 min
        else if (currentTime > achievementTimes[0] && currentTime <= achievementTimes[1])
        {
            //Unlock all time achievements after 0
            for (int i = 1; i < achievementTimes.Length; i++)
            {
                myAchievementManager.UnlockAchievement(achievementIndex[i]);
            }
            return;
        }
        //Greater than 45 less than 1 hour 
        else if (currentTime > achievementTimes[1] && currentTime <= achievementTimes[2])
        {
            //Unlock all time achievements after 0
            for (int i = 2; i < achievementTimes.Length; i++)
            {
                myAchievementManager.UnlockAchievement(achievementIndex[i]);
            }
            return;
        }
        //Greater than 1 hour less than 2 hours
        else if (currentTime > achievementTimes[2] && currentTime <= achievementTimes[3])
        {
            myAchievementManager.UnlockAchievement(achievementIndex[3]); 
        }
    }
}
