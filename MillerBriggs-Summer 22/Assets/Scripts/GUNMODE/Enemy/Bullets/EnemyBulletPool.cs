using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{

    [SerializeField] private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private int amountToPool;

    public void PoolObjects(GameObject bulletPrefab)
    {
        for (int j = 0; j < amountToPool; j++)
        {
            //Debug.Log("pooling");
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);

            pooledObjects.Add(obj);
        }
    }


    public GameObject GetPooledObject()
    {
        //Debug.Log("Get Pooled Object"); 
        
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    public void DeleteObjectPool()
    {
        for (int i = pooledObjects.Count - 1; i >= 0; i--)
        {
            Destroy(pooledObjects[i]);  
            //pooledObjects.RemoveAt(i); 
        }

        Destroy(gameObject); 
    }

}
