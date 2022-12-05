using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMoveAnimation : MonoBehaviour
{
    public Animator myAnim;
    public const string trigger = "trigger";
    public const string playerTrigger = "FlavorTrigger"; 
    public Transform[] movePositions;
    public float moveSpeed;
    public GameObject seagullObject;
    public SpriteRenderer seagullSR; 
    public int defaultSortLayer, moveSortLayer; 

    private Transform currentPosition;
    private int currentAnimation = 1; 
    private bool triggeredAnimation; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (triggeredAnimation)
        {

            seagullSR.sortingLayerID = SortingLayer.layers[moveSortLayer].id; 

            if (currentPosition.position.x >= seagullObject.transform.position.x)
            {
                seagullSR.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f); 
            }
            else
            {
                seagullSR.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f); 
            }

            if (Vector2.Distance(seagullObject.transform.position, currentPosition.position) < 0.05)
            {
                triggeredAnimation = false;
                myAnim.SetTrigger(trigger);
                seagullSR.sortingLayerID = SortingLayer.layers[defaultSortLayer].id;
            }
            else
            {
                seagullObject.transform.position = Vector2.MoveTowards(seagullObject.transform.position, currentPosition.position, Time.deltaTime * moveSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTrigger) && !triggeredAnimation)
        {
            triggeredAnimation = true;

            MoveTrigger(); 
        }
    }

    void MoveTrigger()
    {
        myAnim.SetTrigger(trigger);

        currentPosition = movePositions[currentAnimation]; 

        int plusOne = currentAnimation + 1;

        if (plusOne >= movePositions.Length)
        {
            currentAnimation = 0; 
        }
        else
        {
            currentAnimation++; 
        }
    }
}
