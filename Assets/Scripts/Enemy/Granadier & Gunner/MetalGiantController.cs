using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(EnemyHealth), typeof(CloseRangeAttack), typeof(RangeAttack))]
public class MetalGiantController : MonoBehaviour
{        
    public EnemyStates currentState;

    [HideInInspector]
    public bool isMoving;
    
    [HideInInspector]
    public Vector3 currentTarget;    

    private MetalGiantAnimationController anim;
    private EnemyHealth health;
    
    private CloseRangeAttack closeRange;
    private RangeAttack rangeAttack;

    private Patrol patrol;
    private HuntPlayer hunt;

    private NavMeshAgent agent;

    //Control which the attacks can use. 
    private bool hasMeleeAttack;
    private bool hasCloseRangeAttack;
    private bool hasShieldAttack;
    private bool hasProjectilAttack;

    //Used to check if the the attack animation already stated
    private bool startedAttack;

    private void Awake()
    {
        anim = GetComponent<MetalGiantAnimationController>();
        health = GetComponent<EnemyHealth>();

        closeRange = GetComponent<CloseRangeAttack>();
        rangeAttack = GetComponent<RangeAttack>();

        patrol = GetComponent<Patrol>();
        hunt = GetComponent<HuntPlayer>();

        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        //Set the current state
        currentState = GetEnemyStates();
                
        //State Machine Controller
        switch (currentState)
        {
            case EnemyStates.Patrolling:
                PatrolState();
                break;
            case EnemyStates.Hunting:
                HuntPlayerState();
                break;
            case EnemyStates.Attacking:
                AttackState();
                break;
            case EnemyStates.Dead:
                DeadState();
                break;           
        }

        isMoving = agent.velocity.magnitude > 0.1f;
    }
    
    private void PatrolState()
    {
        agent.isStopped = false;
        
        patrol.Patrolling();

        currentTarget = patrol.Target;
    }

    private void HuntPlayerState()
    {
        agent.isStopped = false;
        
        hunt.HuntingPlayer();

        currentTarget = hunt.Target;
    }

    private void AttackState()
    {
        //Ever will attack, stop the movement
        agent.isStopped = true;
        
        //Check if the attack already started
        if (!startedAttack)
        {
            //The preference order from attacks is: 
            //Has projectil use it, else
            //Has shield use it, else
            //Has close range use it, else
            //Has melee attack use it.
           
            if (hasShieldAttack)
            {
                //Start animation
                anim.SetShieldAttackAnimation();
            }
            else if (hasCloseRangeAttack)
            {
                //Start animation
                anim.SetCloseRangeAttackAnimation();
            }
            else if (hasMeleeAttack)
            {
                //Start animation
                anim.SetMeleeAttackAnimation();
            }            
        }
        else
        {       
            //The only attacks need check collider with player as the animation is running, it's into the close range attacks
            if (hasCloseRangeAttack)
            {
                if(closeRange.IsStarted)
                    closeRange.CloseRange();
            }
            else if(hasMeleeAttack)
            {
                if(closeRange.IsStarted)
                    closeRange.PunchAttack();
            }
        }
       
        startedAttack = anim.StartedAttackAnimation;
    }

    private void DeadState()
    {
        Destroy(this);
        Destroy(hunt);
        Destroy(patrol);
        Destroy(agent);
        Destroy(closeRange);
        Destroy(rangeAttack);        
    }

    private EnemyStates GetEnemyStates()
    {       
        EnemyStates enemyStates;

        if(health.Life <= 0)
        {
            enemyStates = EnemyStates.Dead;
        }
        //Only get the attacking state if Has attack and the current state is hunting, OR if already started the attack
        else if((currentState == EnemyStates.Hunting && HasAttack()) || startedAttack)                    
        {
            enemyStates = EnemyStates.Attacking;
        }
        //Only get the hunt state if is seeing player
        else if(hunt.IsSight())
        {
            enemyStates = EnemyStates.Hunting;
        }
        //How default, if isn't state is match, stay in the patrolling
        else
        {
            enemyStates = EnemyStates.Patrolling;
        }

        return enemyStates;
    }

    private bool HasAttack()
    {        
        hasMeleeAttack = closeRange.CanMeleeAttack();
        hasCloseRangeAttack = closeRange.CanCloseRangeAttack();
        hasShieldAttack = rangeAttack.CanActivedShield();
        hasProjectilAttack = rangeAttack.CanShootProcjetil();

        return hasMeleeAttack || hasCloseRangeAttack || hasShieldAttack || hasProjectilAttack;
    }
}
