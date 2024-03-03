using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = AgentMachine.State;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    Path path;
    [SerializeField]
    PlayerWatcher watcher;

    readonly AgentMachine state = new();
    readonly Dictionary<(State, State), Action> edges = new();
    AgentInterface agentInterface;
    Vector3 lastPatrol;
    int health = 3;

    void Awake()
    {
        state.OnStateChange += HandleStateChange;
        watcher.OnAwarenessChange += HandleAwarenessChange;
        
        /*Register state transitions*/
        agentInterface = GetComponent<AgentInterface>();
        edges.Add((State.Uninitialized, State.Patrolling), () => {
            agentInterface.OnPathReached += HandleDestinationReached;
            agentInterface.Destination = path.Next();
        });

        edges.Add((State.Patrolling, State.Chasing), () => {
            agentInterface.OnPathReached -= HandleDestinationReached;
            watcher.OnPlayerPositionUpdate += HandlePlayerPositionUpdate;
            lastPatrol = agentInterface.Destination;
        });

        edges.Add((State.Chasing, State.Patrolling), () => {
            watcher.OnPlayerPositionUpdate -= HandlePlayerPositionUpdate;
            agentInterface.OnPathReached += HandleDestinationReached;
            agentInterface.Destination = lastPatrol;
        });

        edges.Add((State.Patrolling, State.Dead), () => {
            agentInterface.OnPathReached -= HandleDestinationReached;
            Destroy(gameObject);
        });

        edges.Add((State.Chasing, State.Dead), () => {
            watcher.OnPlayerPositionUpdate -= HandlePlayerPositionUpdate;
            Destroy(gameObject);
        });
    }

    void Start()
    {
        state.Current = State.Patrolling;
    }

    public void Shoot()
    {
        health--;
        if(health == 0)
        {
            state.Current = State.Dead;
        }
    }
    #region StateChanges
    void HandleAwarenessChange(bool visible)
    {
        if(visible)
        {
            state.Current = State.Chasing;
        }
        else
        {
            state.Current = State.Patrolling;
        }
    }

    void HandleStateChange(State previous, State current)
    {
        if(edges.TryGetValue((previous, current), out Action handleEdge))
        {
            handleEdge();
        }
        else
        {
            Debug.LogWarning($"Unknown state transition: {previous}->{current}");
        }
    }
    #endregion

    #region StateSpecific
    void HandlePlayerPositionUpdate(Transform playerTransform)
    {
        agentInterface.Destination = playerTransform.position;
    }

    void HandleDestinationReached()
    {
        agentInterface.Destination = path.Next();
    }
    #endregion
}