using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] breakParticles; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (GameObject obj in breakParticles)
        {
            Instantiate(obj, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); 
    }

}
