using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyController : MonoBehaviour
{ 
    public float deathDistance = 2.5f;
    public float noticeDistance = 8;

    public float distanceAway;
    public Transform thisObject;
    public Transform target;
    private NavMeshAgent navComponent;
    public Transform[] patrolPoints;
    private int PatrolPointCounter = 0;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navComponent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        distanceAway = Vector3.Distance(target.position, transform.position);

        if (distanceAway < noticeDistance)
        {
            LookAtPlayer();

            if (distanceAway >= 2f)
            {
                ChasePlayer();
            }

            else
            {
                GoToNextPoint();
            }

        }

        if (navComponent.remainingDistance < 3f)
        {
            GoToNextPoint();
        }

        if (distanceAway <= deathDistance)
        {
            PlayerGotCought();
        }
    }

    private void ChasePlayer()
    {
        navComponent.SetDestination(target.position);
    }

    private void GoToNextPoint()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }
        else
        {
            navComponent.destination = patrolPoints[PatrolPointCounter].position;
            PatrolPointCounter = (PatrolPointCounter + 1) % patrolPoints.Length;
        }
    }

    private void LookAtPlayer()
    {
        transform.LookAt(target);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeDistance);
    }

    private void PlayerGotCought()
    {
        Debug.Log("You got yeeted");
    }
}
