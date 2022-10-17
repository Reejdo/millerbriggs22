using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLaser : MonoBehaviour
{
    public PuzzlePiece myPuzzlePiece;
    
    [Header("Laser Settings")]
    public LineRenderer[] lasers;
    [SerializeField]
    private Vector2 hitOneVector, hitTwoVector;
    private float hitObjDistOne, hitObjDistTwo;
    private RaycastHit2D hit1, hit2; //hit1 is up and down, hit2 is left and right

    void Start()
    {
        //disable line renderers at start
        foreach (LineRenderer myLaser in lasers)
        {
            myLaser.enabled = false;
        }

        myPuzzlePiece = GetComponent<PuzzlePiece>();
    }

    // Update is called once per frame
    void Update()
    {
        hit1 = myPuzzlePiece.hit1;
        hit2 = myPuzzlePiece.hit2; 

        UpdateLasers(); 
    }

    void UpdateLasers()
    {
        FindHitObjectDistance();

        Vector2 zeroVector = new Vector2(0, 0);

        if (myPuzzlePiece.onePoint)
        {
            lasers[0].SetPosition(0, zeroVector);
            lasers[0].SetPosition(1, hitOneVector);
            lasers[0].enabled = true;
            lasers[1].enabled = false;
        }
        else if (myPuzzlePiece.twoPoint)
        {
            lasers[0].SetPosition(0, zeroVector);
            lasers[0].SetPosition(1, hitOneVector);
            lasers[0].enabled = true;
            lasers[1].SetPosition(0, zeroVector);
            lasers[1].SetPosition(1, hitTwoVector);
            lasers[1].enabled = true;
        }
    }

    void FindHitObjectDistance()
    {
        //because the line won't rotate with the object, we only need 1 number to make a vector into and it goes in x every time
        if (hit1 != false)
        {
            hitOneVector = new Vector2(Mathf.Abs(transform.position.x - hit1.point.x),
                Mathf.Abs(transform.position.y - hit1.point.y));

        }
        if (hit2 != false)
        {
            hitTwoVector = new Vector2(Mathf.Abs(transform.position.x - hit2.point.x),
                Mathf.Abs(transform.position.y - hit2.point.y));
        }

        if (myPuzzlePiece.onePoint)
        {
            SetOnlyVector();
        }
        else if (myPuzzlePiece.twoPoint)
        {
            SetHitOneVector();
            SetHitTwoVector();
        }
    }

    void SetOnlyVector()
    {
        if (myPuzzlePiece.shootDown)
        {
            if (hitOneVector.x >= hitOneVector.y)
            {
                hitOneVector = new Vector2(0, -hitOneVector.x);
            }
            else
            {
                hitOneVector = new Vector2(0, -hitOneVector.y);
            }
        }
        else if (myPuzzlePiece.shootUp)
        {
            if (hitOneVector.x >= hitOneVector.y)
            {
                hitOneVector = new Vector2(0, hitOneVector.x);
            }
            else
            {
                hitOneVector = new Vector2(0, hitOneVector.y);
            }
        }
        else if (myPuzzlePiece.shootLeft)
        {
            if (hitTwoVector.x >= hitTwoVector.y)
            {
                hitOneVector = new Vector2(-hitTwoVector.x, 0);
            }
            else
            {
                hitOneVector = new Vector2(-hitTwoVector.y, 0);
            }
        }
        else if (myPuzzlePiece.shootRight)
        {
            if (hitTwoVector.x >= hitTwoVector.y)
            {
                hitOneVector = new Vector2(hitTwoVector.x, 0);
            }
            else
            {
                hitOneVector = new Vector2(hitTwoVector.y, 0);
            }
        }
    }

    void SetHitOneVector()
    {
        if (myPuzzlePiece.shootDown)
        {
            if (hitOneVector.x >= hitOneVector.y)
            {
                hitOneVector = new Vector2(0, -hitOneVector.x);
            }
            else
            {
                hitOneVector = new Vector2(0, -hitOneVector.y);
            }
        }
        else if (myPuzzlePiece.shootUp)
        {
            if (hitOneVector.x >= hitOneVector.y)
            {
                hitOneVector = new Vector2(0, hitOneVector.x);
            }
            else
            {
                hitOneVector = new Vector2(0, hitOneVector.y);
            }
        }
    }

    void SetHitTwoVector()
    {
        if (myPuzzlePiece.shootLeft)
        {
            if (hitTwoVector.x >= hitTwoVector.y)
            {
                hitTwoVector = new Vector2(-hitTwoVector.x, 0);
            }
            else
            {
                hitTwoVector = new Vector2(-hitTwoVector.y, 0);
            }
        }
        else if (myPuzzlePiece.shootRight)
        {
            if (hitTwoVector.x >= hitTwoVector.y)
            {
                hitTwoVector = new Vector2(hitTwoVector.x, 0);
            }
            else
            {
                hitTwoVector = new Vector2(hitTwoVector.y, 0);
            }
        }
    }

}
