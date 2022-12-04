using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [SerializeField] float attackRange = 2f;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 15f;
    [SerializeField] float patrolRaidus = 6f;
    [SerializeField] float patrolWaitTime = 2f;
    [SerializeField] float chaseSpeed = 4f;
    [SerializeField] float searchSpeed = 3.5f;

    private bool isSearched = false;


    enum State
    {
        Idle,
        Search,
        Chase,
        Attack,
    }

    [SerializeField] private State currentState = State.Idle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        //agent.SetDestination(player.position); // enemy is following the player by the shortest path
        StateCheck();
        StateExecute();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    private void StateCheck()
    {
        float distanceToTarget = Vector3.Distance(player.position,transform.position);  

        if(distanceToTarget <= chaseRange && distanceToTarget > attackRange)
        {
            currentState = State.Chase;
        }
        else if(distanceToTarget <= attackRange)
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Search;  
        }
        
    }

    private void StateExecute()
    {
        switch (currentState)
        {
            case State.Idle:
                print("Idle");
                break;
            case State.Search:
                print("Search");
                if (!isSearched && agent.remainingDistance <= 0.1f || !agent.hasPath && !isSearched)
                {
                    Vector3 agentTarget = new Vector3(agent.destination.x, transform.position.y, agent.destination.z);
                    agent.enabled = false;
                    transform.position = agentTarget;
                    agent.enabled = true;

                    Invoke("Search", patrolWaitTime);

                    isSearched = true;
                }
                break;
            case State.Chase:
                print("Chase");
                Chase();
                break;
            case State.Attack:
                print("Attack");
                Attack();
                break;
        }
    }

    private void Search()
    {
        agent.speed = searchSpeed;
        agent.isStopped = false;
        isSearched = false;
        agent.SetDestination(GetRandomPosition());
    }

    private void Attack()
    {
        if(player == null)
        {
            return;
        }
        agent.isStopped = true;
        LookTheTarget(player.position);
    }

    private void Chase()
    {
        if(player == null)
        {
            return;
        }
        agent.speed = chaseSpeed;
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRaidus;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRaidus, 1);
        return hit.position;

    }

    private void LookTheTarget(Vector3 target)
    {
        Vector3 lookPos = new Vector3(target.x, target.y, target.z);
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(lookPos-transform.position),
            turnSpeed * Time.deltaTime);
    }

}


//enum State
//{
//    Idle,
//    Searh,
//    Chase,
//    Attack,
//}
