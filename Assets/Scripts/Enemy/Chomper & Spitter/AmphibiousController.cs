using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AmphibiousController : MonoBehaviour
{
    public EnemyStates CurrentState { get; set; }    
    
    private Patrol patrol;
    private HuntPlayer hunt;
    private BiteAttack bite;

    private NavMeshAgent agent;
    
    public bool IsMoving { get; private set; }
    public bool IsAttackEnd { get; set; } = true;

    private bool isDed;
    private bool isHitted;

    private void Awake()
    {       
        patrol = GetComponent<Patrol>();
        hunt = GetComponent<HuntPlayer>();
        bite = GetComponent<BiteAttack>();

        agent = GetComponent<NavMeshAgent>();        
    }

    private void FixedUpdate()
    {
        CurrentState = GetEnemyStates();

        switch (CurrentState)
        {
            case EnemyStates.Patrolling:
                patrol.Patrolling();
                break;
            case EnemyStates.Hunting:
                hunt.HuntingPlayer();
                isHitted = false;
                break;
            case EnemyStates.Attacking:
                AttackState();
                break;
            case EnemyStates.Dead:
                agent.isStopped = true;
                break;         
        }

        if (CurrentState != EnemyStates.Attacking)
            IsAttackEnd = true;

        IsMoving = agent.velocity.magnitude > 0.1f;
    }

    private EnemyStates GetEnemyStates()
    {
        //Default state
        EnemyStates enemyStates = EnemyStates.Patrolling;

        if(isDed)
        {
            enemyStates = EnemyStates.Dead;
        }
        else if(CurrentState == EnemyStates.Hunting && bite.CanBite() || CurrentState == EnemyStates.Attacking && !IsAttackEnd)
        {            
            if(PlayerController.s_Instance.IsLive)                
                enemyStates = EnemyStates.Attacking;
        }
        else if(hunt.IsSight() || isHitted)
        {
            if(PlayerController.s_Instance.IsLive)                
                enemyStates = EnemyStates.Hunting;
        }       

        return enemyStates;
    }

    private void AttackState()
    {
        if(IsAttackEnd)
            IsAttackEnd = false;

        if (bite.IsActived)
            bite.Bite();

        transform.LookAt(PlayerController.s_Instance.PlayerPosition);
        agent.isStopped = true;        
    }

    private void OnEnable()
    {
        GetComponent<EnemyHealth>().OnDied += () => isDed = true;
        GetComponent<EnemyHealth>().OnTakeDamage += LookToPlayer;

    }

    private void OnDisable()
    {
        GetComponent<EnemyHealth>().OnDied -= () => isDed = true;
        GetComponent<EnemyHealth>().OnTakeDamage -= LookToPlayer;

    }

    private void LookToPlayer(int x)
    {
        transform.LookAt(PlayerController.s_Instance.PlayerPosition);
        isHitted = true;
    }
}
