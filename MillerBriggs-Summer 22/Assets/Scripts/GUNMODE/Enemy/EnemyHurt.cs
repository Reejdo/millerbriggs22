using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    [SerializeField] private Enemy myEnemy;     

    public void InjureEnemy(int incomingDamage)
    {
        myEnemy.InjureEnemy(incomingDamage); 
    }
}
