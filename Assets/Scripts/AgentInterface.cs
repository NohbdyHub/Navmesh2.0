using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class AgentInterface : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;

    public delegate void HandlePathReached();
    public event HandlePathReached OnPathReached;

    public Vector3 Destination
    {
        get => agent.destination;
        set { agent.destination = value; }
    }

    public bool Stopped
    {
        get => agent.isStopped;
        set { agent.isStopped = value; }
    }

    void Update()
    {
        // https://discussions.unity.com/t/how-can-i-tell-when-a-navmeshagent-has-reached-its-destination/52403
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            OnPathReached?.Invoke();
        }
    }
}
