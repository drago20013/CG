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

    public CollectableItems collectableItems;
    public UIController uiController;
    public GameObject sponge;
    private float turnSmoothVel;

    public AudioClip walk, jump;
    AudioSource audioSource;
    public bool shouldPlayWalkSound, isPlaying;

    //runs once 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        smoothTime = 0.1f;
        audioSource = GetComponent<AudioSource>();
        shouldPlayWalkSound = false;
        isPlaying = false;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
            move = context.ReadValue<Vector2>();
            shouldPlayWalkSound = true;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void OnF(InputAction.CallbackContext context)
    {
        GameObject triggerCone = sponge.transform.GetChild(0).gameObject;
        float distanceToTriggerCone = Vector3.Distance(triggerCone.transform.position, gameObject.transform.position);
        if (this.enabled == true && context.performed && distanceToTriggerCone <= 4.0 && triggerCone.activeSelf == true) {
            switchControllerToSponge();
        }
    }

    //function that checks if "E" key is pressed, and then activate potion for 10 seconds
    public void OnE(InputAction.CallbackContext context)
    {
        if (context.started) {
            //if (collectableItems.potionAvailable)
            //{
            
            activatePower(collectableItems.chosenPotion);
            collectableItems.usePotion();
            //}
        }
    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            collectableItems.OnTogglePotion();
        }
    }

    public void switchControllerToSponge() {
        SpongeController input = sponge.GetComponent<SpongeController>();
        GameObject fakeRat = sponge.transform.GetChild(1).gameObject;
        GameObject triggerCone = sponge.transform.GetChild(0).gameObject;
        fakeRat.SetActive(true);
        triggerCone.SetActive(false);
        gameObject.SetActive(false);
        input.enabled = true;
        UnityEngine.Debug.Log("Controller IS Switched");
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


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            shouldPlayWalkSound = true;
        }
        else
        {
            shouldPlayWalkSound = false;
        }

        if (shouldPlayWalkSound && !isPlaying)
        {
            isPlaying = true;
            audioSource.clip = walk;
            audioSource.enabled = true;
            audioSource.loop = true;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
        else if(!shouldPlayWalkSound && isPlaying)
        {
            isPlaying = false;
            audioSource.Stop();
            audioSource.enabled = false;
            audioSource.loop = false ;
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
        if (rb.velocity.magnitude < 0.2) shouldPlayWalkSound = false; 
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
            audioSource.clip = jump;
            audioSource.enabled = true;
            audioSource.loop = false;
            audioSource.Play();
        }

        rb.AddForce(jumpForces, ForceMode.VelocityChange);
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }
}
