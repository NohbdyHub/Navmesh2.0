using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
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

    void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * move * 10;
        rigidbody.angularVelocity = new(0f, rotation * 5, 0f);
        if(fire && offCooldown)
        {
            StartCoroutine(StartCooldown());
            var realBullet = Instantiate(bullet, eye.position, eye.rotation);
            realBullet.GetComponent<Rigidbody>().velocity = eye.forward * (10 + rigidbody.velocity.magnitude);
            Debug.Log("FIRE!!!");
        }
    }

    IEnumerator StartCooldown()
    {
        offCooldown = false;
        yield return new WaitForSeconds(1f);
        offCooldown = true;
        yield break;
    }

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
