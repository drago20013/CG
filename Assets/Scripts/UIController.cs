using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class UIController : MonoBehaviour
{
    public Label bufTime;
    public Label liveTime;
    public VisualElement background, timers, potion;
    public bool timerIsRunning = false;
    public GameObject mouse;

    public Sprite redPotion, bluePotion, backgroundSprite;
    private Label choosenPotion;

    private Rigidbody rb;
    private ThirdPersonMovement movement;

    public VisualElement root;
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            bufTime = root.Q<Label>("buf-time");
            liveTime = root.Q<Label>("time");
            choosenPotion = root.Q<Label>("potion-image");
            potion = root.Q<VisualElement>("potion");
            bufTime.text = "00:00";
            liveTime.text = "00:00";
        }
        else
        {
            liveTime = root.Q<Label>("time");
            liveTime.text = "00:00";
        }
        background = root.Q<VisualElement>("background");
        timers = root.Q<VisualElement>("timers");
        rb = mouse.GetComponent<Rigidbody>();
        movement = mouse.GetComponent<ThirdPersonMovement>();
        timerIsRunning = true;
    }

    public void changeImage(string potionName)
    {
        if (potionName == "antiGravity")
        {
            choosenPotion.style.backgroundImage = new StyleBackground(bluePotion);
        }
        else if (potionName == "increaseWeight")
        {
            choosenPotion.style.backgroundImage = new StyleBackground(redPotion);
        }
        else
        {
            choosenPotion.style.backgroundImage = null;
        }

    }

    public float timeRemaining;

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayLiveTime(timeRemaining);
            }
            else
            {
                UnityEngine.Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayLiveTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        liveTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DisplayBufTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        bufTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void OnQuit()
    {
        UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (background.style.backgroundImage == new StyleBackground(backgroundSprite))
            {
                OnQuit();
            }
            else
            {
                //stop timer, hide timers and change background
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                movement.enabled = false;
                timerIsRunning = false;
                timers.style.display = DisplayStyle.None;
                if (SceneManager.GetActiveScene().name == "Level1")
                {
                    potion.style.display = DisplayStyle.None;
                }
                background.style.backgroundImage = new StyleBackground(backgroundSprite);
            }
        }
    }

    public void OnContinue()
    {

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        movement.enabled = true;

        timerIsRunning = true;
        timers.style.display = DisplayStyle.Flex;
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            potion.style.display = DisplayStyle.Flex;
        }
        background.style.backgroundImage = null;
    }
}
