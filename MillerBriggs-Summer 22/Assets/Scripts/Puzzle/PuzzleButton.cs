using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleButton : MonoBehaviour
{
    [Header("Puzzle Piece Information")]
    public PuzzlePiece[] myPuzzlePieces;
    public bool isRotateClockwise;
    public bool isRotateCounterClock; 
    public bool isFlipHor;
    public bool isFlipVert;

    private bool isInRange;
    
    private bool canBeUsed;
    private float animationNumber = 1; 
    private Animator myAnim;


    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        canBeUsed = true; 
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteractButton();

        myAnim.SetFloat("animationNumber", animationNumber); 
    }

    void CheckInteractButton()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange && canBeUsed)
        {
            StartCoroutine(ButtonAnimation()); 
        }
    }

    void InteractButton()
    {
        if (isRotateClockwise)
        {
            foreach (PuzzlePiece piece in myPuzzlePieces)
            {
                piece.Rotate90Degrees(true); 
            }
        }
        else if (isRotateCounterClock)
        {
            foreach (PuzzlePiece piece in myPuzzlePieces)
            {
                piece.Rotate90Degrees(false);
            }
        }
        else if (isFlipVert)
        {
            foreach (PuzzlePiece piece in myPuzzlePieces)
            {
                piece.FlipVertical();
            }
        }
        else if (isFlipHor)
        {
            foreach (PuzzlePiece piece in myPuzzlePieces)
            {
                piece.FlipHorizontal(); 
            }
        }
        else if (isRotateClockwise && isRotateCounterClock && isFlipHor && isFlipVert)
        {
            Debug.Log("Can't be all 4 types of button!"); 
        }
    }

    IEnumerator ButtonAnimation()
    {
        canBeUsed = false;
        InteractButton();

        if (animationNumber == 1)
        {
            animationNumber = 4; 
        }
        else if (animationNumber == 4)
        {
            animationNumber = 1;
        }

        canBeUsed = true;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

}
