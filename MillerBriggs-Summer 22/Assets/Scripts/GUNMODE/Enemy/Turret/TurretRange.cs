using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRange : MonoBehaviour
{
    public Turret myTurret;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered!");
        if (collision.gameObject.CompareTag("Player"))
        {
            myTurret.SetPlayerRange(true); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            myTurret.SetPlayerRange(false);
        }
    }
}
