using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    [SerializeField]
    InputActionAsset inputActions;
    [SerializeField]
    new Rigidbody rigidbody;

    float move = 0f;
    float rotation = 0f;
    bool fire = false;

    void OnEnable()
    {
        inputActions.Enable();
        inputActions["Rotate"].performed += OnRotatePerformed;
        inputActions["Rotate"].canceled += (c) => { rotation = 0f; };

        inputActions["Move"].performed += OnMovePerformed;
        inputActions["Move"].canceled += (c) => { move = 0f; };

        inputActions["Fire"].started += OnFireStarted;
    }

    void OnDisable()
    {
        inputActions.Disable();
        inputActions["Rotate"].performed -= OnRotatePerformed;
        inputActions["Move"].performed -= OnMovePerformed;
        inputActions["Fire"].started -= OnFireStarted;
    }

    void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * move * 10;
        rigidbody.angularVelocity = new(0f, rotation * 5, 0f);
    }


    void OnMovePerformed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<float>();
    }

    void OnRotatePerformed(InputAction.CallbackContext context)
    {
        rotation = context.ReadValue<float>();
    }

    void OnFireStarted(InputAction.CallbackContext context)
    {
        Debug.Log($"Fire!");
    }
}
