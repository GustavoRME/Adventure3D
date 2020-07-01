using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{    
    public static SpawnController Instance { get; private set; }

    [SerializeField]
    [Tooltip("Check point where player will spawn")]
    private CheckpointSpawn checkpoint;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCheckPoint(CheckpointSpawn checkpoint)
    {
        if (this.checkpoint)
            this.checkpoint.DisableCheckPoint();

        this.checkpoint = checkpoint;
        this.checkpoint.EnableCheckPoint();
    }

    public void SpawnAtCheckPoint()
    {                        
        PlayerController.s_Instance.Respawn(checkpoint.Position);
    }
}
