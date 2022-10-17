using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    public float bulletForce;
    [SerializeField] private float destroyTime = 10;
    private float currentLifeTime;
    public GameObject destroyEffect, wallDestroyEffect;
    public int bulletDamage;
    public AudioManager myAudioManager;
    private GainAmmo myGainAmmo; 

    [Header("ADD AMMO FOR DEFAULT BULLET")]
    public bool addAmmoOnHit; 

    // Start is called before the first frame update
    void Start()
    {

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        myGainAmmo = FindObjectOfType<GainAmmo>().GetComponent<GainAmmo>();

        StartDirection(); 
    }

    // Update is called once per frame
    void Update()
    {
        //LifeTime(); 
        if (myGainAmmo == null)
        {
            Debug.Log("Can't find gain ammo!"); 
            myGainAmmo = FindObjectOfType<GainAmmo>().GetComponent<GainAmmo>(); 
        }
    }

    void LifeTime()
    {
        if (gameObject.activeInHierarchy)
        {
            currentLifeTime += Time.deltaTime;
        }
        else
        {
            currentLifeTime = 0;
        }

        if (currentLifeTime >= destroyTime)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            //Destroy(gameObject); 
            gameObject.SetActive(false);

            Debug.Log("Ran out of time");
        }
    }


    public void InstantiateDirection()
    {
        StartDirection(); 
    }

    protected virtual void StartDirection()
    {
        currentLifeTime = 0; 

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePosition - transform.position;
        Vector3 rotation = transform.position - mousePosition;

        //normalizing this makes sure that it doesn't increase or decrease speed depending on mouse position on screen
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletForce;

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (addAmmoOnHit && collision.gameObject.CompareTag("Enemy"))
        {
            myGainAmmo.IncrementAmmoGain(); 
        }

        Instantiate(wallDestroyEffect, transform.position, Quaternion.identity);
        //Destroy(gameObject); 
        gameObject.SetActive(false);
    }

}
