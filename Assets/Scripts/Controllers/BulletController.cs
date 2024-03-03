using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    new Rigidbody rigidbody;

    int bounces = 2;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
            collision.gameObject.GetComponentInParent<AgentController>().Shoot();
        }
        bounces--;
        if(bounces == 0)
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
}
