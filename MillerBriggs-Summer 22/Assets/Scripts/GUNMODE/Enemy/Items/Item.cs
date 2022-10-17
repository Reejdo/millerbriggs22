using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float simpleRange;
    Vector2 moveDirection;
    private bool playerInRange;
    [SerializeField] private float moveSpeed;
    public ItemTypes myItem;
    private AudioManager myAudioManager; 

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myAudioManager == null)
        {
            myAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }


        if (Vector2.Distance(transform.position, target.position) < simpleRange)
        {
            Vector2 playerDirection = (target.position - transform.position).normalized;

            moveDirection = playerDirection;

            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ItemCollected();
            Destroy(gameObject); 
        }
    }


    void ItemCollected()
    {
        switch (myItem)
        {
            case (ItemTypes.health):
                {
                    PlayerHealth myPlayerHealth = GameObject.FindObjectOfType<PlayerHealth>(); 
                    if (myPlayerHealth == null)
                    {
                        Debug.Log("Can't find player health!");
                        myPlayerHealth = GameObject.FindObjectOfType<PlayerHealth>(); 
                    }
                    myPlayerHealth.AddHealth(); 
                    break; 
                }
            case (ItemTypes.skullAmmo):
                {
                    Shooting myShooting = GameObject.FindObjectOfType<Shooting>(); 
                    if (myShooting == null)
                    {
                        Debug.Log("Can't find shooting script!"); 
                        myShooting = GameObject.FindObjectOfType<Shooting>();
                    }

                    myShooting.AddAmmo(1);

                    break; 
                }
            case (ItemTypes.thirdAmmo):
                {
                    Shooting myShooting = GameObject.FindObjectOfType<Shooting>();
                    if (myShooting == null)
                    {
                        Debug.Log("Can't find shooting script!");
                        myShooting = GameObject.FindObjectOfType<Shooting>();
                    }

                    myShooting.AddAmmo(2);

                    break; 
                }
            default:
                break; 
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, simpleRange);
    }
}





public enum ItemTypes
{
    health, 
    skullAmmo, 
    thirdAmmo
}
