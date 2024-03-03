using System.Collections;
using NETWORK_ENGINE;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : NetworkComponent
{
    [SerializeField]
    InputActionAsset inputActions;
    [SerializeField]
    new Rigidbody rigidbody;
    [SerializeField]
    Transform eye;
    [SerializeField]
    GameObject bullet;

    float move = 0f;
    float rotation = 0f;
    bool fire = false, offCooldown = true;

    void OnEnable()
    {
        inputActions.Enable();

        inputActions["Move"].performed += OnMovePerformed;
        inputActions["Move"].canceled += OnMoveCanceled;

        inputActions["Rotate"].performed += OnRotatePerformed;
        inputActions["Rotate"].canceled += OnRotateCanceled;

        inputActions["Fire"].started += OnFireStarted;
        inputActions["Fire"].canceled += OnFireCanceled;
    }

    void OnDisable()
    {
        inputActions["Move"].performed -= OnMovePerformed;
        inputActions["Move"].canceled -= OnMoveCanceled;

        inputActions["Rotate"].performed -= OnRotatePerformed;
        inputActions["Rotate"].canceled -= OnRotateCanceled;

        inputActions["Fire"].started -= OnFireStarted;
        inputActions["Fire"].canceled -= OnFireCanceled;

        inputActions.Disable();
    }

    /*
    void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * move * 10;
        rigidbody.angularVelocity = new(0f, rotation * 5, 0f);
        if(fire && offCooldown)
        {
            StartCoroutine(StartCooldown());
            var realBullet = Instantiate(bullet, eye.position, eye.rotation);
            realBullet.GetComponent<Rigidbody>().velocity = rigidbody.velocity + (15 * eye.forward);
            Debug.Log("FIRE!!!");
        }
    }
    */

    IEnumerator StartCooldown()
    {
        offCooldown = false;
        yield return new WaitForSeconds(1f);
        offCooldown = true;
        yield break;
    }

    #region Networking
    public override IEnumerator SlowUpdate()
    {
        while(true)
        {
            rigidbody.velocity = transform.forward * move * 10;
            rigidbody.angularVelocity = new(0f, rotation * 5, 0f);
            if(fire && offCooldown)
            {
                StartCoroutine(StartCooldown());
                var realBullet = Instantiate(bullet, eye.position, eye.rotation);
                realBullet.GetComponent<Rigidbody>().velocity = rigidbody.velocity + (15 * eye.forward);
                Debug.Log("FIRE!!!");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void HandleMessage(string flag, string value)
    {
        throw new System.NotImplementedException();
    }

    // can't be bothered to make this "Correct" but I hate the whole Find By Name thing
    public override void NetworkedStart()
    {
        transform.position = GameObject.Find("Level").transform.Find("Spawn").transform.position;

        foreach(var watcher in FindObjectsByType<PlayerWatcher>(FindObjectsSortMode.None))
        {
            watcher.player = transform;
        }
    }
    #endregion

    #region InputHandling
    void OnMovePerformed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<float>();
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        move = 0f;
    }

    void OnRotatePerformed(InputAction.CallbackContext context)
    {
        rotation = context.ReadValue<float>();
    }

    void OnRotateCanceled(InputAction.CallbackContext context)
    {
        rotation = 0f;
    }

    void OnFireStarted(InputAction.CallbackContext context)
    {
        fire = true;
    }

    void OnFireCanceled(InputAction.CallbackContext context)
    {
        fire = false;
    }
    #endregion
}
