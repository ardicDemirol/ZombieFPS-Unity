using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;


    enum State
    {
        Idle,
        Searh,
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
        StateExecute(); 
    }

    private void StateExecute()
    {
        switch (currentState)
        {
            case State.Idle:
                print("Idle");
                break;
            case State.Searh:
                print("Search");
                break;
            case State.Chase:
                print("Chase");
                break;
            case State.Attack:
                print("Attack");
                break;
        }
    }
}


//enum State
//{
//    Idle,
//    Searh,
//    Chase,
//    Attack,
//}
