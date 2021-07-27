using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshMove : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;

    public Transform PointB;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(PointB.position);
    }
}
