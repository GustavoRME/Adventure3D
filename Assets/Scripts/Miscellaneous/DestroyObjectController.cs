using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectController : MonoBehaviour
{    
    [SerializeField]
    private float timeToDestroy = 5f;

    [SerializeField]
    private float timeToCrossFloor = 3f;

    private float elapsedTime;

    private bool allDisabled;

    private void FixedUpdate()
    {
        if (elapsedTime > timeToCrossFloor)
        {
            if(!allDisabled)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).TryGetComponent(out Collider coll))
                    {
                        coll.enabled = false;
                        Debug.Log("Coll Disabled");
                    }
                }

                allDisabled = true;
            }
        }

        if (elapsedTime > timeToDestroy)
            Destroy(gameObject);

        elapsedTime += Time.fixedDeltaTime;
    }
}
