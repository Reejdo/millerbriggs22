using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;
    public float mouseAngle;
    //private float rotateX = -180f;
    //[SerializeField] private SpriteRenderer sr;
    //[SerializeField] private Sprite upSprite, downSprite, rightSprite, leftSprite;
    //[SerializeField] private Transform gunSprite;
    //[SerializeField] private float originalPivot, flippedPivot;
    public Transform centerOfPlayer; 
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private GameObject[] gunSprite; 
    [SerializeField] private Shooting shootS; 
    [SerializeField] private PGunMovement pm;
    public float mouseScreenX, mouseScreenY;
    [SerializeField] private Vector3 p;

    // Start is called before the first frame update
    void Start()
    {
        //p = gunSprite.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(centerOfPlayer.position);



        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        mouseScreenX = mouseOnScreen.x;
        mouseScreenY = mouseOnScreen.y;

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        //Just to check the angle
        mouseAngle = angle;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 180f));

        //point down
        if (mouseScreenX <= .60 && mouseScreenX >= .30 && mouseScreenY < .4)
        {
            pm.directionToPoint = 1;
            //SetSpriteOG(); 
            disableGuns(); 
            SetSprite(gunSprite[0], spawnPos[0]); 
        } 
        //point right
        else if (mouseScreenY >= 0 && mouseScreenY < .75 && mouseScreenX > .55)
        {
            pm.directionToPoint = 2;
            disableGuns(); 
            SetSprite(gunSprite[1], spawnPos[1]);
        }  
        //point up
        else if (mouseScreenY > .55 && mouseScreenX < .60 && mouseScreenX >= .35)
        {
            pm.directionToPoint = 3;
            disableGuns(); 
            SetSprite(gunSprite[2], spawnPos[2]);
        }
        //point left
        else if (mouseScreenX < .45 && mouseScreenY >= 0 && mouseScreenY < .75)
        {
            pm.directionToPoint = 4;
            disableGuns(); 
            SetSprite(gunSprite[3], spawnPos[3]);
        }


    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void SetSprite(GameObject gun, Transform spawnPos)
    {
        /*sr.sprite = s;
        p.y = y;
        p.x = x; 
        gunSprite.localPosition = p;*/
        gun.SetActive(true); 
        shootS.firePoint = spawnPos; 
    }

    void disableGuns()
    {
        foreach (GameObject gun in gunSprite)
        {
            gun.SetActive(false);
        }
    }
    /*
    void SetSpriteOG()
    {
        sr.sprite = original;
        p.y = originalPivot;
        gunSprite.localPosition = p;
        shootS.firePoint = spawnPos1; 
    }

    void SetSpriteFlipped()
    {
        sr.sprite = flipped;
        p.y = flippedPivot;
        gunSprite.localPosition = p;
        shootS.firePoint = spawnPos2;
    }*/
}
