using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovePlatforms", menuName = "MovePlatforms")]

public class MovePlatforms : ScriptableObject
{
    public Vector2[] platformPositions;
    public bool[] playerParented; 

}
