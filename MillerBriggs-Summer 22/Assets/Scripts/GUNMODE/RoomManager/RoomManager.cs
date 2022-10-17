using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int enemyCount;
    [SerializeField] private bool roomDefeated;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject entryDoor; 

    void Start()
    {
        SetDoorState(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount <= 0 && !roomDefeated)
        {
            roomDefeated = true;
            SetDoorState(false);
            entryDoor.SetActive(true); 
        }
    }


    public void SetDoorState(bool state)
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(state); 
        }
    }

    public void AddEnemyCount()
    {
        enemyCount++; 
    }

    public void SubtractEnemyCount ()
    {
        enemyCount--; 
    }

    public int GetEnemyCount()
    {
        return enemyCount; 
    }

    public bool GetRoomDefeated()
    {
        return roomDefeated; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !roomDefeated)
        {
            Debug.Log("Enable Doors"); 
            SetDoorState(true);
            entryDoor.SetActive(true);
        }
    }

}
