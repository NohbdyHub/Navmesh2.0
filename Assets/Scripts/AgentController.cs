using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    Path path;

    AgentInterface agentInterface;

    void Awake()
    {
        agentInterface = GetComponent<AgentInterface>();
    }

    void OnEnable()
    {
        agentInterface.OnPathReached += () => { agentInterface.Destination = path.Next(); };
        agentInterface.Destination = path.Next();
    }

    void OnDisable()
    {
        agentInterface = null;
    }
}
