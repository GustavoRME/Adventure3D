using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Action<int> OnTakeDamage;
    public Action OnDied;

    [SerializeField]
    private int maxLife = 10;
    
    public int Life { 
        get
        {
            return maxLife;
        }        
    }

    public void TakeDamage(int damage)
    {
        OnTakeDamage?.Invoke(0);

        TakeDamage(damage);
    }    
    public void TakeDamage(int damage, int combo)
    {
        OnTakeDamage?.Invoke(combo);

        maxLife -= damage;

        if (maxLife <= 0)
            OnDied?.Invoke();
    }

}
