using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;


public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject camHolder;
    public float speed, sensitivity, maxForce, jumpForce, gravity, antiGravityStartTime;
    private Vector2 move, look;
    private float lookRotation;
    public bool grounded;

    public CollectableItems antiGravity;
    public UIController uiController;

    //runs once 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    //function that checks if "E" key is pressed, and then sets rb.gravity to false for 10 seconds
    public void OnE(InputAction.CallbackContext context)
    {
        if (antiGravity.antiGravityPower)
        {
            onPowerActivate();
        }
    }

    public void onPowerActivate()
    {
        antiGravityStartTime = Time.time;
        antiGravity.antiGravityPower = false;
        rb.useGravity = false;
    }

    void Update()
    {
        if (antiGravityStartTime != 0.0f)
        {
            float elapsedTime = Time.time - antiGravityStartTime;
            uiController.DisplayBufTime(10 - elapsedTime);
            if (elapsedTime > 10)
            { 
                antiGravityStartTime = 0;
                rb.useGravity = true;
            }
        }
    }


    //for physics 
    void FixedUpdate()
    {
        Move();

    }


    void LateUpdate()
    {
        Look();
    }

    void Move()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= speed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0f, velocityChange.z);

        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Look()
    {
        transform.Rotate(Vector3.up * look.x * sensitivity);
        //lookRotation += (-look.y * sensitivity);

        //lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(0f, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }

    void Jump()
    {
        Vector3 jumpForces = Vector3.zero;

        if (grounded)
        {
            jumpForces = Vector3.up * jumpForce;
        }

        rb.AddForce(jumpForces, ForceMode.VelocityChange);
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }
}
