using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private TMP_Text state;
    private StateMachine brain;
    private Animator animator;   
    private NavMeshAgent agent;
    private PlayerController player;
    private bool playerIsNear;
    private bool withinAttackRange;
    private float changeMindTime;
    private float attackTime;
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponent<StateMachine>();
        playerIsNear = false;
        withinAttackRange = false;
        brain.PushState(OnIdleEnter, OnIdleExit, Idle);
    }

    // Update is called once per frame
    void Update()
    {
        playerIsNear = Vector3.Distance(transform.position, player.transform.position) < 5;
        withinAttackRange = Vector3.Distance(transform.position, player.transform.position) < 2;
    }
    #region Idle State Functions

    void OnIdleEnter()
    {
        state.text = "Idle";
        agent.ResetPath();
    }

    void Idle()
    {
        if(playerIsNear)
        {
            brain.PushState(OnChaseEnter, OnChaseExit, Chase);
        }
        else if(changeMindTime <= 0 )
        {
            brain.PushState(OnWanderEnter, OnWanderExit, Wander);
            changeMindTime = Random.Range(4, 10);
        }
    }

    void OnIdleExit()
    {

    }
    #endregion
    #region Wander State Functions

    void OnWanderEnter()
    {
        state.text = "Wander";
        animator.SetBool("Chase", true);
        Vector3 wanderDirection = (Random.insideUnitSphere * 4f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(wanderDirection, out navMeshHit, 3f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.SetDestination(destination);
    }

    void Wander()
    {
       
        if (agent.remainingDistance <= 0.25f)
        {
            agent.ResetPath();
            brain.PushState(OnIdleEnter, OnIdleExit, Idle);
        }

        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
    }

    void OnWanderExit()
    {
        animator.SetBool("Chase", false);
    }
    #endregion

    #region Chase State Functions
    void OnChaseEnter()
    {
        Debug.Log("Chase Called");
        state.text = "Chase";
        animator.SetBool("Chase", true);
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
        if(Vector3.Distance(transform.position, player.transform.position) > 5.5f)
        {
            Debug.Log("Leaving chase");
            brain.PopState();
            brain.PushState(OnIdleEnter,OnIdleExit, Idle);
        }

        if(withinAttackRange)
        {
            brain.PushState(OnAttackEnter,null, Attack);
        }
    }

    void OnChaseExit()
    {
        animator.SetBool("Chase", false);
    }
    #endregion

    #region Attack State Functions

    void OnAttackEnter()
    {
        agent.ResetPath();
        state.text = "Attack";
    }

    void Attack()
    {
        attackTime -= Time.deltaTime;
        if(!withinAttackRange)
        {
            brain.PopState();
        }
        else if( attackTime <= 0 )
        {
            animator.SetTrigger("Attack");
            player.Hurt(2, 1);
            attackTime = 2f;
        }
    }

   

    #endregion
}
