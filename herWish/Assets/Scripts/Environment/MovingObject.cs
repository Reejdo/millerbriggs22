using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public MovePlatforms myMovePlatform;
    public int movePlatformNumber; 

    public Transform wayPoint_1, wayPoint_2;
    [SerializeField] private Transform wayPointTarget;
    private Vector2 moveDir;

    public GameObject platformObject;
    [SerializeField] private float moveSpeed;

    private float waitTime = 0.5f;
    private float waitTimer;

    [SerializeField] private float waitBetweenMoves = 0.75f;
    private float waitMoveTimer; 

    private void Awake()
    {
        //save previous positional data so player can spawn in on moving platform
        if (myMovePlatform.platformPositions[movePlatformNumber] != Vector2.zero)
        {
            platformObject.transform.position = myMovePlatform.platformPositions[movePlatformNumber];
        }
        else
        {
            myMovePlatform.platformPositions[movePlatformNumber] = platformObject.transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        wayPointTarget = wayPoint_2;
    }

    private void Update()
    {
        waitTimer += Time.deltaTime;

        if (waitTimer > waitTime)
        {
            Move();
        }


        myMovePlatform.platformPositions[movePlatformNumber] = platformObject.transform.position;
    }


    void Move()
    {
        if (Vector2.Distance(platformObject.transform.position, wayPoint_1.position) < 0.1)
        {
            waitMoveTimer += Time.deltaTime; 

            if (waitMoveTimer > waitBetweenMoves)
            {
                wayPointTarget = wayPoint_2;
                //Debug.Log("Move to wayPoint_2");

                waitMoveTimer = 0f; 
            }
        }

        if (Vector2.Distance(platformObject.transform.position, wayPoint_2.position) < 0.1)
        {
                waitMoveTimer += Time.deltaTime;

                if (waitMoveTimer > waitBetweenMoves)
                {
                    wayPointTarget = wayPoint_1;
                    //Debug.Log("Move to wayPoint_1");

                    waitMoveTimer = 0f;
                }
        }

        platformObject.transform.position = Vector2.MoveTowards(platformObject.transform.position, wayPointTarget.transform.position, Time.deltaTime * moveSpeed); 

    }
}
