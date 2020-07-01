using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float horizontalSmooth = 1.0f;

    [SerializeField]
    private float verticalSmooth;

    [SerializeField]
    private float maxVertical;

    [SerializeField]
    private float minVertical;

    private Transform target;
    private InputMaster input;

    private float vertical;
    private float lastVertical;

    private void Awake()
    {
        input = new InputMaster();

        input.Player.LookRotation.performed += ctx => vertical = ctx.ReadValue<Vector2>().y;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject cameraPos = GameObject.Find("Camera Pos");

        if (cameraPos)
            target = cameraPos.transform;
        else
            Debug.Log("Cannot possible find [Camera Pos] to attachet at target into the script");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, horizontalSmooth * Time.fixedDeltaTime);
            transform.forward = Vector3.Lerp(transform.forward, target.forward, horizontalSmooth * Time.fixedDeltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        GameObject cameraPos = GameObject.Find("Camera Pos");

        if (cameraPos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(cameraPos.transform.position, cameraPos.transform.position + cameraPos.transform.forward);
        }
    }
}
