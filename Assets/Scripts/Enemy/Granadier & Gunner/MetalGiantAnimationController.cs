using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MetalGiantAnimationController : MonoBehaviour
{    
    private Animator anim;
    private MetalGiantController controller;    

    private AnimatorStateInfo currentState;

    //-------------------------------Animation States--------------------------------
    private readonly int deadState = Animator.StringToHash("Base Layer.GrenadierDeath");
    
    //Attack states
    private readonly int meleeAttackState = Animator.StringToHash("Base Layer.Attacks.GrenadierMeleeAttack");
    private readonly int closeRangeAttackState = Animator.StringToHash("Base Layer.Attacks.GrenadierCloseRangeAttack");
    private readonly int shieldAttackState = Animator.StringToHash("Base Layer.Attacks.GrenadierRangeAttack");
    private readonly int projectilAttackState = Animator.StringToHash("Base Layer.Attacks.GrenadierRangeAttack2");

    //Hit States
    private readonly int hit01 = Animator.StringToHash("Base Layer.Hits.GrenadierHit1");
    private readonly int hit02 = Animator.StringToHash("Base Layer.Hits.GrenadierHit2");
    private readonly int hit03 = Animator.StringToHash("Base Layer.Hits.GrenadierHit3");
    private readonly int hit04 = Animator.StringToHash("Base Layer.HitsGrenadierHit4");

    public bool StartedAttackAnimation { get; private set; }

    private int hitCombo;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<MetalGiantController>();        
    }

    private void FixedUpdate()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);

        Debug.Log("Controller " + controller.currentState.ToString());

        if(controller.currentState == EnemyStates.Dead)
        {
            if(currentState.fullPathHash != deadState)
            {
                anim.SetTrigger("Die");                
            }
            else
            {                
                if (currentState.normalizedTime > 0.95f)
                {
                    CapsuleCollider capsuleColl = GetComponent<CapsuleCollider>();

                    capsuleColl.center = Vector3.zero;
                    capsuleColl.height = 0f;
                    capsuleColl.radius = 1.77f;

                    gameObject.layer = LayerMask.NameToLayer("Ground");

                    Destroy(this);
                }
                    
            }
        }        
        else if((controller.currentState == EnemyStates.Hunting || controller.currentState == EnemyStates.Patrolling))
        {
            Vector3 dir = controller.currentTarget - transform.position;
            float angle = Vector3.Angle(dir, transform.forward);

            if (angle >= 45)
                TurnAnimation(angle, dir);

            anim.SetBool("IsMoving", controller.isMoving);            
        }
        
        if (StartedAttackAnimation)
        {
            if (IsAttackAnimation())
            {
                if (currentState.normalizedTime >= 0.9f)
                {
                    StartedAttackAnimation = false;
                    anim.SetBool("IsAttacking", false);
                }
            }
        }

        if (!IsHittedState())
            anim.SetInteger("HitCombo", 0);
    }

    public void SetMeleeAttackAnimation()
    {
        SetAttackAnimation();
    }

    public void SetCloseRangeAttackAnimation()
    {
        SetAttackAnimation();
        anim.SetTrigger("CloseRange");
    }

    public void SetShieldAttackAnimation()
    {
        SetAttackAnimation();
        anim.SetTrigger("RangeAttack");
    }

    public void SetProjectilAttackAnimation()
    {
        SetAttackAnimation();
        anim.SetTrigger("ProjectilAttack"); 
    }     

    private bool IsAttackAnimation()
    {
        return currentState.fullPathHash == meleeAttackState || currentState.fullPathHash == closeRangeAttackState || currentState.fullPathHash == shieldAttackState || currentState.fullPathHash == projectilAttackState;
    }

    private void SetAttackAnimation()
    {
        anim.SetBool("IsMoving", false);
        anim.SetBool("IsAttacking", true);        

        StartedAttackAnimation = true;
    }

    private void TurnAnimation(float angle, Vector3 dir)
    {
        bool isRight = dir.x > 0 ? false : true;
        
        anim.SetBool("IsRight", isRight);        
        anim.SetInteger("Angle", Mathf.FloorToInt(angle));
    }

    private void HitAnimation(int combo)
    {
        if(currentState.fullPathHash != deadState && !IsAttackAnimation())
        {
            if (!IsHittedState())
                anim.SetTrigger("Hitted");
            else
                anim.SetInteger("HitCombo", combo);            
        }
    }

    private bool IsHittedState()
    {
        return currentState.fullPathHash == hit01 || currentState.fullPathHash == hit02 || currentState.fullPathHash == hit03 || currentState.fullPathHash == hit04;
    }

    private void OnEnable()
    {
        GetComponent<EnemyHealth>().OnTakeDamage += HitAnimation;        
    }

    private void OnDisable()
    {
        GetComponent<EnemyHealth>().OnTakeDamage -= HitAnimation;
    }  
}
