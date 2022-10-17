using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePuzzleManager : MonoBehaviour
{
    [Header("Puzzle Type Settings")]
    public bool onePoint; 
    public bool twoPoint;
    public bool fourPoint;

    [Header("Directions Hit")]
    public bool hitLeft;
    public bool hitRight;
    public bool hitUp;
    public bool hitDown; 


    public void UpdateHitActivity(GameObject obj, bool isHitting)
    {
        //Debug.Log("Called Hit Activity"); 

        //Debug.Log("Has hit " + isHitting); 
        float xDiff = transform.position.x - obj.transform.position.x;
        float yDiff = transform.position.y - obj.transform.position.y; 

        //on x axis
        if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
        {
            //beam from the right
            if (transform.position.x <= obj.transform.position.x)
            {
                hitRight = isHitting;
            }
            //beam from the left
            else
            {
                hitLeft = isHitting;
            }
        }
        else
        {
            //beam above
            if (transform.position.y < obj.transform.position.y)
            {
                hitUp = isHitting;
            }
            //beam below
            else
            {
                hitDown = isHitting;
            }
        }
    }
}
