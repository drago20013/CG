using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UIElements;


public class QuestionItem {
        public string question;
        public string answer;

        public QuestionItem(string question, string answer){
            this.question = question;
            this.answer = answer;
        }
    }

public class Questions {
    private int current_question = -1;

    
    public List<QuestionItem> questions = new List<QuestionItem>{
        new QuestionItem("Show sodium","sodium"),
        new QuestionItem("What is the most common organic element","carbon"),
        new QuestionItem("What is the most conductive metal on the periodic table?","silver"),
        new QuestionItem("What is the most metallic element on the periodic table?","francium"),
        new QuestionItem("Known as the densest natural element, this heavy metal is often used in electrical contacts or fountain pens","osmium"),
    };

    public void addQuestion(string question, string answer) {
        questions.Add(new QuestionItem(question, answer));
    }

    public QuestionItem getQuestion(){
        current_question++;
        if (current_question < questions.Count ){
            return questions[current_question];
        }
        return new QuestionItem("You won", "mf");
    }
}

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform cam;
    public CinemachineFreeLook cinemaCam;
    private float normalFov = 50f;
    private float scopedFov = 15f;
    private Transform zoomTarget;
    public float speed, sensitivity, maxForce, jumpForce, gravity, potionTime, smoothTime;
    private Vector2 move, look;
    private float lookRotation;
    public bool grounded;
    public bool shooting = true;

    public CollectableItems collectableItems;
    public UIController uiController;
    public GameObject sponge;
    private float turnSmoothVel;

    public AudioClip walk, jump;
    AudioSource audioSource;
    public bool shouldPlayWalkSound, isPlaying;
    public bool scoped = false;

    public GameObject crosshair;
    public LayerMask aimColliderLaterMask = new LayerMask();
    public GameObject lazerTransform;
    public GameObject videoClip;


    public Questions questions = new Questions();

    private string question;
    private string answer;

    public Label questionLabel;
    public Label answerLabel;



    //runs once 
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        smoothTime = 0.1f;
        audioSource = GetComponent<AudioSource>();
        zoomTarget = gameObject.transform.GetChild(2).gameObject.transform;
        shouldPlayWalkSound = false;
        isPlaying = false;
        QuestionItem questionItem = questions.getQuestion();
        question = questionItem.question;
        answer = questionItem.answer;

        // Show GUI element with questions[it].question
        questionLabel = uiController.root.Q<Label>("question");
        questionLabel.text = question;
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

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (collectableItems.laserPointerEnable == true){
            Zoom();
        }
    }

    public void OnF(InputAction.CallbackContext context)
    {
        GameObject triggerCone = sponge.transform.GetChild(0).gameObject;
        float distanceToTriggerCone = Vector3.Distance(triggerCone.transform.position, gameObject.transform.position);
        if (this.enabled == true && context.performed && distanceToTriggerCone <= 4.0 && triggerCone.activeSelf == true) {
            switchControllerToSponge();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (collectableItems.laserPointerEnable == true){
            if (context.started) {
                shooting = true;
                lazerTransform.SetActive(true);
            }
            else if(context.canceled) {
                shooting = false;
                lazerTransform.SetActive(false);
            }
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
        if (collectableItems.laserPointerEnable == true){
            GameObject fakeLaserTag = gameObject.transform.GetChild(3).gameObject;
            fakeLaserTag.SetActive(true);
        }

        if (shooting) 
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            if (Physics.Raycast(ray, out RaycastHit raycasetHit, 999f, aimColliderLaterMask)){
                lazerTransform.transform.position = raycasetHit.point;
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == this.answer)
                {
                    QuestionItem questionItem = questions.getQuestion();
                    question = questionItem.question;
                    if (question == "You won"){
                        videoClip.SetActive(true);
                        Zoom();
                    }
                    answer = questionItem.answer;
                    questionLabel = uiController.root.Q<Label>("question");
                    questionLabel.text = question;
                }

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

    void Zoom()
    {   

        scoped = !scoped;
        if (scoped) {
            onScoped();
        } else if (!scoped) {
            onUnScoped();
        }
    }

    void onScoped()
    {
        crosshair.SetActive(true);
        cinemaCam.m_CommonLens = true;
        cinemaCam.m_Lens.FieldOfView = scopedFov;
        cinemaCam.LookAt = zoomTarget;        
    }

    void onUnScoped()
    {
        crosshair.SetActive(false);
        cinemaCam.m_Lens.FieldOfView = normalFov;
        // cinemaCam.m_CommonLens = false;
        cinemaCam.LookAt = gameObject.transform;
        cinemaCam.Follow = gameObject.transform;

    }


    public void SetGrounded(bool state)
    {
        grounded = state;
    }
}
