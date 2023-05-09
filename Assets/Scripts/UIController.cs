using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Label bufTime;
    public Label liveTime;
    public bool timerIsRunning = false;

    public Sprite redPotion, bluePotion;
    private Label choosenPotion;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        bufTime = root.Q<Label>("buf-time");
        liveTime = root.Q<Label>("time");
        choosenPotion = root.Q<Label>("potion-image");
        bufTime.text = "00:00";
        liveTime.text = "00:00";
        timerIsRunning = true;
        //choosenPotion.style.backgroundImage = new StyleBackground(redPotion);
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

    public void OnQuit() {
        UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }
}
