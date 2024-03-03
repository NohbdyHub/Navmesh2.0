using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWatcher : MonoBehaviour
{
    [SerializeField]
    Transform eye;
    [SerializeField]
    Transform player;

    public delegate void HandleAwarenessChange(bool visible);
    public delegate void HandlePlayerPositionUpdate(Transform playerTransform);
    public event HandleAwarenessChange OnAwarenessChange;
    public event HandlePlayerPositionUpdate OnPlayerPositionUpdate;

    bool near = false;
    bool previousVisibility = false, currentVisibility = false;
    Vector3 playerLocation = Vector3.zero;

    public Vector3 PlayerLocation {
        get => playerLocation;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            near = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            near = false;
        }
    }

    void FixedUpdate()
    {
        if(near)
        {
            var direction = (player.transform.position - eye.transform.position).normalized;
            float dot = Vector3.Dot(direction, transform.forward);
            currentVisibility = 
                dot > 0.5f && 
                Physics.Raycast(eye.position, direction, out RaycastHit hit) && 
                hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")
            ;

            if(currentVisibility)
            {
                OnPlayerPositionUpdate?.Invoke(player);
            }
        }
        else
        {
            currentVisibility = false;
        }

        //change in visibility happened
        if((!previousVisibility && currentVisibility) || (previousVisibility && !currentVisibility))
        {
            OnAwarenessChange?.Invoke(currentVisibility);
        }

        previousVisibility = currentVisibility;
    }
}
