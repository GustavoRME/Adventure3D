using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllenHealth : MonoBehaviour
{
    public static EllenHealth Instance { get; private set; }
    
    public int maxHealth = 5;
    
    public int Health { get; private set; }

    public Action<Vector3> OnTakeDamage;
    public Action OnIncreaseHealth;
    public Action OnDecreaseHealth;
    public Action OnDied;

    private void Awake()
    {
        Health = maxHealth;

        Instance = this;
    }
  
    public void TakeDamage(int damage, Vector3 enemyPos)
    {
        if (Health > 0)
        {
            //Stay the health between zero and the max health. 
            Health = Mathf.Clamp(Health - damage, 0, maxHealth);

            OnTakeDamage?.Invoke(enemyPos);
            OnDecreaseHealth?.Invoke();

            if (Health <= 0)
                OnDied?.Invoke();
        }
    }

    public void IncreaseHealth()
    {
        if (Health < maxHealth)
        {
            Health++;
            OnIncreaseHealth?.Invoke();
        }
    }

    public void RestoreHealth()
    {
        Health = maxHealth;

        OnIncreaseHealth?.Invoke();
    }
}
