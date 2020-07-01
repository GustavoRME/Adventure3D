using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerController : MonoBehaviour
{
    //Sligleton
    public static ObjectPoolerController s_Instance;

    [SerializeField]
    [Tooltip("default parent it's used if the item not have a parent set")]
    private Transform defaultParent = null;

    [Space]

    [SerializeField]
    [Tooltip("Array the items to add at the pool")]
    private ObjectPoolItem[] items = null;

    private Dictionary<string, List<GameObject>> poolObject;

    private void Awake()
    {        
        poolObject = new Dictionary<string, List<GameObject>>();

        //--- Create pool ---

        //Pass through the items
        for (int i = 0; i < items.Length; i++)
        {
            ObjectPoolItem itemToPool = items[i];

            //Create pool list
            List<GameObject> objectPool = new List<GameObject>();            
                                    
            for (int j = 0; j < itemToPool.amount; j++)
            {
                //Wether the parent in the item to pool is null, use default parent
                Transform parent = itemToPool.parent != null ? itemToPool.parent : defaultParent;

                //Instantiate at the scene 
                GameObject go = Instantiate(itemToPool.item, parent);

                //Disable the object
                go.SetActive(false);

                //Add at the list
                objectPool.Add(go);
            }

            //Add at pool
            poolObject.Add(itemToPool.item.gameObject.tag, objectPool);
        }
        
        //Singleton
        s_Instance = this;
    }    

    public GameObject GetPoolItem(string tag)
    {
        if (!poolObject.ContainsKey(tag))
            return null;

        foreach (string key in poolObject.Keys)
        {
            //Found the key
            if(key == tag)
            {
                //Pass through the list pool
                foreach (GameObject item in poolObject[key])
                {
                    //isn`t active, return it
                    if (!item.activeInHierarchy)
                        return item;
                }
            }
        }

        //No one item is disable into the pool, return null
        return null;
    }    
}
