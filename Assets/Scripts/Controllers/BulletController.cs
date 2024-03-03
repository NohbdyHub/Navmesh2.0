using System.Collections;
using System.Collections.Generic;
using NETWORK_ENGINE;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : NetworkComponent
{
    //new Rigidbody rigidbody;

    int bounces = 2;

    void OnCollisionEnter(Collision collision)
    {
        if(IsServer)
        {
            if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                SendUpdate("BALLDIE", "");
                //Destroy(gameObject);
                collision.transform.parent.GetComponent<AgentController>().Shoot();
            }

            bounces--;
            if(bounces == 0)
            {
                SendUpdate("BALLDIE", "");
                Destroy(gameObject);
            }
        }
    }

    public override IEnumerator SlowUpdate()
    {
        yield break;
    }

    public override void HandleMessage(string flag, string value)
    {
        if(flag == "BALLDIE")
        {
            
        }
        throw new System.NotImplementedException();
    }

    public override void NetworkedStart()
    {
        //rigidbody = GetComponent<Rigidbody>();
    }
}
