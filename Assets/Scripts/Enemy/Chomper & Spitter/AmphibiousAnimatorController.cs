using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmphibiousAnimatorController : MonoBehaviour
{
    private AmphibiousController controller;
    
    private Animator anim;
    private AnimatorStateInfo currentState;

    readonly int attackState = Animator.StringToHash("Base Layer.ChomperAttack");
    readonly int deadState = Animator.StringToHash("Base Layer.ChomperHit4");

    private bool isAttacking;
    private bool isDead;
    
    private void Awake()
    {
        controller = GetComponent<AmphibiousController>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);

        if(controller.CurrentState == EnemyStates.Patrolling)
        {            
            anim.SetBool("IsMoving", controller.IsMoving);
            anim.SetBool("IsRunning", false);

            isAttacking = false;
            //anim.applyRootMotion = false;
        }
        else if(controller.CurrentState == EnemyStates.Hunting)
        {
            anim.SetBool("IsRunning", true);

            isAttacking = false;
            //anim.applyRootMotion = false;
        }
        else if(controller.CurrentState == EnemyStates.Attacking)
        {
            if(currentState.fullPathHash == attackState)
            {                
                if (currentState.normalizedTime > 0.9f)
                    controller.IsAttackEnd = true;
            }

            if (!isAttacking)
                anim.SetTrigger("Attack");
            
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsMoving", false);
            
            isAttacking = true;            
        }
        else
        {
            //It's Dead
            if (!isDead)
                anim.SetTrigger("IsDead");

            isDead = true;

            if(currentState.fullPathHash == deadState)
            {
                if(currentState.normalizedTime > 0.9f)
                    Destroy(gameObject);
            }
        }       
    }

    private void OnEnable()
    {
        GetComponent<EnemyHealth>().OnTakeDamage += TakeDamageAnimation;        
    }

    private void OnDisable()
    {
        GetComponent<EnemyHealth>().OnTakeDamage -= TakeDamageAnimation;
    }

    private void TakeDamageAnimation(int combo)
    {
        if (currentState.fullPathHash != attackState || currentState.normalizedTime > 0.35f)
        {
            int hitCombo = Mathf.Clamp(combo, 0, 2);

            anim.SetInteger("HitNumber", hitCombo);
            anim.SetTrigger("Hitted");
        }        
    }
}
