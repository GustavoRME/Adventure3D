using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    bool isHandle { get; set; }

    void Attack();

    void PutAway();

    void PullTheGun();
}
