using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = AgentMachine.State;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    Path path;

    AgentInterface agentInterface;
    readonly AgentMachine state = new();
    Vector3 lastPatrol;

    void Awake()
    {
        state.OnStateChange += HandleStateChange;
        agentInterface = GetComponent<AgentInterface>();
    }

    void OnEnable()
    {
        state.Current = State.Patrolling;
    }

    void HandleStateChange(State previous, State current)
    {
        var match = (previous, current);
        if(match == (State.Uninitialized, State.Patrolling))
        {
            StartPatrolling();
        }
        else if(match == (State.Patrolling, State.Chasing))
        {
            FromPatrollingToChasing();
        }
        else if(match == (State.Chasing, State.Patrolling))
        {
            FromChasingToPatrolling();
        }
        else if(match == (State.Patrolling, State.Dead))
        {
            FromPatrollingToDead();
        }
        else if(match == (State.Chasing, State.Dead))
        {
            FromChasingToDead();
        }
        else
        {
            Debug.LogWarning($"Unknown state transition: {previous}->{current}");
        }
    }

    void HandleDestinationReached()
    {
        agentInterface.Destination = path.Next();
    }

    void StartPatrolling()
    {
        agentInterface.OnPathReached += HandleDestinationReached;
        agentInterface.Destination = path.Next();
    }

    void FromPatrollingToChasing()
    {
        agentInterface.OnPathReached -= HandleDestinationReached;
        lastPatrol = agentInterface.Destination;
        //chase player here
    }

    void FromChasingToPatrolling()
    {
        agentInterface.OnPathReached += HandleDestinationReached;
        agentInterface.Destination = lastPatrol;
    }

    void FromPatrollingToDead()
    {
        agentInterface.OnPathReached -= HandleDestinationReached;
        //die
    }

    void FromChasingToDead()
    {
        //die
    }   
}
