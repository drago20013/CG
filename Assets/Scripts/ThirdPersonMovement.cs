using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;


public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform cam;
    public float speed, sensitivity, maxForce, jumpForce, gravity, potionTime, smoothTime;
    private Vector2 move, look;
    private float lookRotation;
    public bool grounded;

    public CollectableItems potions;
    public UIController uiController;

    private float turnSmoothVel;


    //runs once 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        smoothTime = 0.1f;
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

    //function that checks if "E" key is pressed, and then activate potion for 10 seconds
    public void OnE(InputAction.CallbackContext context)
    {
        if (context.started) {
            //if (potions.potionAvailable)
            //{
                
          
                activatePower(potions.chosenPotion);
            potions.usePotion();
            //}
        }
    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            potions.OnTogglePotion();
        }
    }

    void activatePower(String power)
    {
        if( power == "antiGravity")
        {
            antiGravityActivate();
        }
        else if (power == "increaseWeight")
        {
            increaseWeightActivate();
        }
    }

    public void antiGravityActivate()
    {
        ResetPowers();
        potionTime = Time.time;
        rb.useGravity = false;
    }

    public void increaseWeightActivate()
    {
       ResetPowers();
       potionTime = Time.time;
       rb.mass = 15;
    }

    void ResetPowers() {
        potionTime = 0;
        rb.useGravity = true;
        rb.mass = 1;
    }

    void Update()
    {
        if (potionTime != 0.0f)
        {
            float elapsedTime = Time.time - potionTime;
            uiController.DisplayBufTime(10 - elapsedTime);
            if (elapsedTime > 10)
            {
                ResetPowers();
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
    Vector3 direction = new Vector3();

    void Move()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.y, 0, -move.x);
     
        targetVelocity = Quaternion.Euler(0f, cam.eulerAngles.y - 90, 0f) * targetVelocity;
        targetVelocity *= speed;

        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0f, velocityChange.z);

        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Look()
    {
        direction = new Vector3(move.y, 0f, -move.x);
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y - 90.0f;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, smoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
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
