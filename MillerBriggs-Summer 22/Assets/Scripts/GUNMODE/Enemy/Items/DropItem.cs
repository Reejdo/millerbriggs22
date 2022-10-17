using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem", menuName = "Drop Items")]
public class DropItem : ScriptableObject
{
    public GameObject dropObject;
    public float dropRate; 
}
