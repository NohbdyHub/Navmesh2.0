using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMachine
{
    public enum State
    {
        Uninitialized, Patrolling, Chasing, Dead
    }

    public delegate void HandleStateChange(State previous, State current);
    public event HandleStateChange OnStateChange;

    State current;

    public State Current {
        get => current;
        set => Change(value);
    }

    void Change(State to)
    {
        State last = current;
        current = to;
        OnStateChange?.Invoke(last, to);
    }
}
