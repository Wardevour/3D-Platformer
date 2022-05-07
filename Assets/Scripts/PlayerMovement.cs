using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRigidbody = null;
    private GameControls controls = null;
    private Vector3 currentMovementVector = Vector3.zero;
    private Coroutine lookCoroutine = null;

    public float movementSpeed = 1.0f;
    public float lookSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public GameObject playerModel = null;
    public Transform feetTransform = null;
    public LayerMask groundLayerMask;

    public bool IsGrounded
    {
        get
        {
            return Physics.CheckSphere(feetTransform.position, 0.01f, groundLayerMask);
        }
    }

    private void OnEnable()
    {
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }

        if (controls == null)
        {
            controls = new GameControls();
            controls.Player.Move.performed += OnMove;
            controls.Player.Move.canceled += OnStop;

            controls.Player.Look.performed += OnLook;
            controls.Player.Look.canceled += OnStopLook;
        }

        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (!IsGrounded)
        {
            // Debug.Log("not grounded");
            return;
        }

        playerRigidbody.AddRelativeForce(currentMovementVector);

        if (controls.Player.Jump.IsPressed())
        {
            // Debug.Log("jumped");
            playerRigidbody.AddRelativeForce(Vector3.up * jumpForce);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movementVector = context.ReadValue<Vector2>();

        // Debug.Log("moved: " + movementVector.ToString());

        currentMovementVector = new Vector3(
            movementVector.x * movementSpeed,
            0f,
            movementVector.y * movementSpeed
        );
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        // Debug.Log("stopped");

        currentMovementVector = new Vector3(
            0f,
            0f,
            0f
        );
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Debug.Log("look");
        if (lookCoroutine != null)
        {
            return;
        }

        lookCoroutine = StartCoroutine(LookAtEnumerator());
    }

    private void OnStopLook(InputAction.CallbackContext context)
    {
        Debug.Log("look stopped");
        if (lookCoroutine == null)
        {
            return;
        }

        StopCoroutine(lookCoroutine);
        lookCoroutine = null;

    }
    public IEnumerator LookAtEnumerator()
    {
        while (true)
        {
            Vector2 lookValue = controls.Player.Look.ReadValue<Vector2>();
            Vector3 targetPosition = new Vector3(
                playerModel.transform.position.x + lookValue.y,
                playerModel.transform.position.y,
                playerModel.transform.position.z + lookValue.x
            );
            float step = lookSpeed * Time.deltaTime;
            Vector3 targetDirection = Vector3.RotateTowards(playerModel.transform.position, targetPosition, step, 0.0f);
            playerModel.transform.rotation = Quaternion.LookRotation(targetDirection);
            yield return null;
        }
    }
}
