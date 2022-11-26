using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class LogInventoryUI : MonoBehaviour
{
    public LogInventory myLogInventory;
    public TMP_Text currentLogText, maxLogText;

    // Start is called before the first frame update
    void Start()
    {
        maxLogText.text = "" + myLogInventory.GetMaxLogs(); 
    }

    // Update is called once per frame
    void Update()
    {
        currentLogText.text = "" + myLogInventory.GetLogsCollected(); 
    }
}
