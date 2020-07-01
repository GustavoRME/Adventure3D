using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController s_Intance;

    public enum WeaponType 
    { 
        Staff, 
        Pistol
    };

    public WeaponType CurrentWeapon { get; private set; }

    private StaffController staff;
    private PistolController pistol;

    private InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();

        controls.Player.Staff.performed += ctx => TakeStaff();                              //1 key
        controls.Player.Pistol.performed += ctx => TakePistol();                            //2 key

        staff = FindObjectOfType<StaffController>();
        pistol = FindObjectOfType<PistolController>();

        //Start with the staff
        CurrentWeapon = WeaponType.Staff;

        //Retain both weapons
        PutAwayStaff();
        PutAwayPistol();

        s_Intance = this;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void Shoot()
    {
        pistol.Attack();

    }

    public void Staff(int combo)
    {
        if (!staff.isHandle)
            staff.PullTheGun();

        staff.Attack();
        staff.staffCombo = combo;
    }

    //Retain staff
    private void PutAwayStaff()
    {        
        staff.PutAway();

        //Even retain the staff, the current weapon keep being  the staff.
        CurrentWeapon = WeaponType.Staff;
    }

    //Take staff
    private void TakeStaff()
    {
        //Before take the staff, retain the pistol if is the current weapon
        if (CurrentWeapon == WeaponType.Pistol)
        {
            PutAwayPistol();
        }
      
        //Depending if the weapon is already on the hand or not, take or retain
        if(staff.isHandle)
        {
            //Retain
            PutAwayStaff();
        }
        else
        {
            //Take it
            staff.PullTheGun();
        }
        
        CurrentWeapon = WeaponType.Staff;
    }

    //Retain pistol
    private void PutAwayPistol()
    {
        pistol.PutAway();
    }

    //Take pistol
    private void TakePistol()
    {
        //Before take the pistol, retain the staff if is the current weapon
        if (CurrentWeapon == WeaponType.Staff)
        {
            PutAwayStaff();
        }
        
        //Depending if the weapon is already on the hand or not, take or retain
        if(pistol.isHandle)
        {
            //Retain
            PutAwayPistol();

            //Always it's retained the pistol, the current weapon will be staff
            CurrentWeapon = WeaponType.Staff;
        }
        else
        {
            //Take it
            pistol.PullTheGun();
            CurrentWeapon = WeaponType.Pistol;

        }

    }   

    public bool HaveAmmo()
    {
        return pistol.HaveBullets();
    }
}
