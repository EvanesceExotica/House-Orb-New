using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {



    public static ObjectPool current;
    public PooledObject prefab;
    public int pooledAmount = 20;
    public bool willGrow = true;

    List<PooledObject> availableObjects = new List<PooledObject>();

    public PooledObject GetObject()
    {
        PooledObject obj;
        int lastAvailableIndex = availableObjects.Count - 1;
        if(lastAvailableIndex >= 0)
        {
            //if there's sitll something in the list
            obj = availableObjects[lastAvailableIndex];
            availableObjects.RemoveAt(lastAvailableIndex);
            obj.gameObject.SetActive(true);
                
        }
        else
        { //if the list is now empty
            obj = Instantiate<PooledObject>(prefab);
            obj.transform.SetParent(transform, false);
            obj.Pool = this;

        }

        return obj;

    }


    public void AddObject(PooledObject obj)
    {
        obj.gameObject.SetActive(false);
        availableObjects.Add(obj);
    }

    public static ObjectPool GetPool(PooledObject prefab)
    {
        GameObject obj;
        ObjectPool pool;

        if (Application.isEditor)
        { //check here to make sure the object doesn't already exist with this same name to avoid duplicates 
            obj =  GameObject.Find(prefab.name + "Pool");
            if (obj != null)
            {
                pool = obj.GetComponent<ObjectPool>();

                if (pool)
                {
                    return pool; 
                }
            }
        }

        obj = new GameObject(prefab.name + "Pool");
        pool = obj.AddComponent<ObjectPool>();
        pool.prefab = prefab;
        return pool; 
    }


   
}
