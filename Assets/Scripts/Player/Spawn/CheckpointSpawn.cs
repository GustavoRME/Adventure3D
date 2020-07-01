using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpawn : MonoBehaviour
{
    private Animation anim;
    
    public Vector3 Position { get { return transform.position; } }
    
    [SerializeField]
    [Tooltip("Mesh used to access the material for change color")]
    private MeshRenderer mesh;
        
    [Space]

    [SerializeField]
    [Tooltip("HDR Color to identify that pressure padinner it's enable")]
    [ColorUsage(true, true)]
    private Color activeColor = Color.green;

    [SerializeField]
    [Tooltip("HDR Color to identify that pressure padinner it's disabled")]
    [ColorUsage(true, true)]
    private Color disableColor = Color.red;
        
    [SerializeField]
    private bool isActived = false;     //Check if the checkpoint is actived. This means that spawn controller will used this how spawn
    private bool isUsable = true;       //Check if the checkpoint is yet usable, if set how false, the checkpoint was already used, and no can set anymore
    private bool isPlayingFoward;       //Check if the current play animations is forward. If true, so can set how check point

    private void Start()
    {
        anim = GetComponent<Animation>();

        if (isActived)
            SetPressurePadinnerColor(activeColor);
    }

    public void EnableCheckPoint()
    {
        SetPressurePadinnerColor(activeColor);
        isActived = true;
    }

    public void DisableCheckPoint()
    {
        SetPressurePadinnerColor(disableColor);
        isUsable = false;
    }

    public void EndPressure()
    {        
        if(isPlayingFoward)
            SpawnController.Instance.SetCheckPoint(this);
    }

    private void SetPressurePadinnerColor(Color color)
    {
        mesh.material.SetColor("_EmissiveColor", color);
    }

    private void PlayForward()
    {
        if (!anim.isPlaying)
            anim.Play();

        anim["PadPressure"].speed = 1;        
        isPlayingFoward = true;
    }

    private void PlayBackward()
    {
        if (!anim.isPlaying)
            anim.Play();

        anim["PadPressure"].speed = -1;
        isPlayingFoward = false;
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Player") && !isActived && isUsable)
            PlayForward();        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActived && isUsable)
            PlayBackward();        
    }

}
