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

    public float movementSpeed = 1.0f;
    public float jumpForce = 1.0f;
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
            Debug.Log("jumpped");
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
}
