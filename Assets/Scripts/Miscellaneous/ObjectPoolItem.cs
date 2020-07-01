using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to help in the inspector the object item want to be pooled

[Serializable]
public class ObjectPoolItem
{
    [Tooltip("Item to be created the pool")]
    public GameObject item = null;
    
    [Tooltip("Amount of item want create")]
    public int amount = 0;

    [Tooltip("Parent where the item will be instantiate")]
    public Transform parent = null;
}
