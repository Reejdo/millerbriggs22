using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectPool : MonoBehaviour
{
    public static PlayerObjectPool instance; 

    [SerializeField] public const int weaponTypes = 2;

    [SerializeField] private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private int[] amountToPool;

    [SerializeField] private int[] bulletID = new int[weaponTypes];

    [SerializeField] private List<int> ids = new List<int>(); 

    public GameObject[] bulletPrefab;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void BeginPooling()
    {
        for (int i = 0; i < weaponTypes; i++)
        {
            for (int j = 0; j < amountToPool[i]; j++)
            {
                //Debug.Log("pooling");
                GameObject obj = Instantiate(bulletPrefab[i]);
                obj.SetActive(false);

                pooledObjects.Add(obj);
                ids.Add(bulletID[i]);
            }
        }
    }


    public void SetBulletIDs(int gunNumber, int ID)
    {
        bulletID[gunNumber] = ID; 
    }

    public GameObject GetPooledObject(int gunNumber)
    {
        for (int j = 0; j < pooledObjects.Count; j++)
        {
            if (!pooledObjects[j].activeInHierarchy)
            {
                if (ids[j].Equals(bulletID[gunNumber]))
                {
                    //Debug.Log("Id at " + j + " is " + bulletID[gunNumber]);
                    return pooledObjects[j];
                }

            }
        }

        return null; 
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
