using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{ 
    //Controllers
    private Animator anim;
    private InputMaster controls;

    //Parameters to Animations
    private WeaponController.WeaponType lastWeaponUsed;         //Used to know what the weapon was being used by player. It's only called to control the animation to take out or put away the pistol. How the Current Weapon from the WeaponController script it's update before call the animation, i needed create this variable to control this
    private Vector2 input;                                      //Vector 2 from the input keys WASD    
    private int staffCombo;                                     //Control the current state of the combo Length [0, 3]            

    private readonly float comboDelay = 0.6f;                   //Delay to reset the staffCombo
    private float lastClickedTime;                              //Get the current time when the mouse is clicked with staff on hand

    //States    
    private AnimatorStateInfo currentState;

    private readonly int idleState = Animator.StringToHash("Base Layer.EllenIdle");
    private readonly int combo01State = Animator.StringToHash("Base Layer.Staff Combo.EllenCombo1");
    private readonly int combo02State = Animator.StringToHash("Base Layer.Staff Combo.EllenCombo2");
    private readonly int combo03State = Animator.StringToHash("Base Layer.Staff Combo.EllenCombo3");
    private readonly int combo04State = Animator.StringToHash("Base Layer.Staff Combo.EllenCombo4");
    private readonly int firePistolState = Animator.StringToHash("Base Layer.EllenGunShoot");
    private readonly int spawnState = Animator.StringToHash("Base Layer.EllenSpawn");
    private readonly int deathState = Animator.StringToHash("Base Layer.EllenDeath");


    private void Awake()
    {
        anim = GetComponent<Animator>();        
        
        controls = new InputMaster();
        
        controls.Player.Movement.performed += ctx => input = ctx.ReadValue<Vector2>();
        controls.Player.Fire.performed += ctx => AttackAnimationController();
        controls.Player.Pistol.performed += ctx => SwitchWeaponAnimationController();
        controls.Player.Staff.performed += ctx => TakeOutStaffWithoutPistolAnimation();        
    }

    private void Start()
    {
        controls.Enable();
        EllenHealth.Instance.OnTakeDamage += TakeDamageAnimation;
        EllenHealth.Instance.OnDied += DiedAnimation;
    }

    private void OnDisable()
    {
        controls.Disable();
        EllenHealth.Instance.OnTakeDamage -= TakeDamageAnimation;
        EllenHealth.Instance.OnDied -= DiedAnimation;
    }

    private void FixedUpdate()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);

        if (PlayerController.s_Instance.IsLive)
        {
            //Stop the movement if is on the melee combo state or is shooting 
            if (IsMeleeComboState() || currentState.fullPathHash == firePistolState)
            {
                PlayerController.s_Instance.StopMovement();
            }
            else
            {
                PlayerController.s_Instance.ReturnMovement();
            }

            //Reset staff combo
            if (Time.time - lastClickedTime > comboDelay && staffCombo > 0)
            {
                staffCombo = 0;
            }
            else if (currentState.fullPathHash == combo04State)
            {
                if (currentState.normalizedTime > currentState.length - 0.1f && staffCombo > 0)
                {
                    staffCombo = 0;
                }
            }
            else if (staffCombo > 2 && !IsMeleeComboState())
            {
                staffCombo = 0;
            }

            //Run animation
            anim.SetFloat("Speed", input.y);
        }

        if(currentState.fullPathHash == spawnState)
        {            
            PlayerController.s_Instance.CanMove = false;            
        }        
        else if(currentState.fullPathHash == deathState)
        {
            if (currentState.normalizedTime > currentState.length - .3f)
                SpawnController.Instance.SpawnAtCheckPoint();

        }
        else if(currentState.fullPathHash == idleState)
        {
            PlayerController.s_Instance.CanMove = true;
        }
    }

    //Called by press mouse 1 key
    private void AttackAnimationController()
    {
        if (PlayerController.s_Instance.IsGrounded())
        {
            if (WeaponController.s_Intance.CurrentWeapon == WeaponController.WeaponType.Staff)
            {
                MeleeAttack();

                lastClickedTime = Time.time;
            }
            else
            {
                if(WeaponController.s_Intance.HaveAmmo())
                    FirePistol();
            }
        }
    }

    //Called by pistol animation
    public void FireAnimationStart()
    {        
        WeaponController.s_Intance.Shoot();         
    } 

    //Called by staff combo animation 
    public void MeleeAttackStart()
    {
        anim.SetBool("IsAttacking", false);                       
        WeaponController.s_Intance.Staff(staffCombo);
    }

    //Called by staff combo animation 
    public void MeleeAttackEnd()
    {                
    }

    //Take on hand pistol
    private void TakeOutPistolAnimation()
    {        
        anim.SetTrigger("TakeOutPistol");
    }

    private void TakeOutStaffWithoutPistolAnimation()
    {        
        if(WeaponController.s_Intance.CurrentWeapon == WeaponController.WeaponType.Staff && lastWeaponUsed == WeaponController.WeaponType.Pistol)
        {
            PutAwayPistolAnimation();

            lastWeaponUsed = WeaponController.WeaponType.Staff;
        }
    }

    //Retain Pistol
    private void PutAwayPistolAnimation()
    {        
        anim.SetTrigger("PutAwayPistol");        
    }

    private void SwitchWeaponAnimationController()
    {        
        if(lastWeaponUsed == WeaponController.WeaponType.Staff)
        {
            //Take pistol animation
            TakeOutPistolAnimation();
        }
        else
        {
            //Retain pistol animation
            PutAwayPistolAnimation();
        }

        //Update to the current Weapon
        lastWeaponUsed = WeaponController.s_Intance.CurrentWeapon;
    }
    
    //Set animation fire
    private void FirePistol()
    {
        anim.SetTrigger("Fire");        
    }

    //Set animation melee attack
    private void MeleeAttack()
    {
        anim.SetBool("IsAttacking", true);
        anim.SetInteger("StaffCombo", staffCombo);

        staffCombo++;
    }    
   
    /// <summary>
    /// Return if the current state is on melee combo attack
    /// </summary>
    /// <returns></returns>
    private bool IsMeleeComboState()
    {
        return currentState.fullPathHash == combo01State || currentState.fullPathHash == combo02State || currentState.fullPathHash == combo03State || currentState.fullPathHash == combo04State;
    }   

    private void TakeDamageAnimation(Vector3 enemyPos)
    {        
        Vector3 dir = enemyPos - transform.position;
        float angle = Vector3.Angle(transform.forward, dir);

        bool isRight = dir.x > transform.forward.x ? true : false;

        anim.SetBool("IsHitRight", isRight);
        anim.SetFloat("HitAngle", angle);
        anim.SetTrigger("Hitted");        
    }

    private void DiedAnimation()
    {
        anim.SetTrigger("Died");
    }
}
