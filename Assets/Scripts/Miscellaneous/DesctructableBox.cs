using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesctructableBox : MonoBehaviour
{
    [SerializeField]
    private GameObject desctructableObject = null;

    public void DestructBox()
    {
        Instantiate(desctructableObject, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
