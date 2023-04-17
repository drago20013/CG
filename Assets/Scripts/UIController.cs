using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{

    public Label bufTime;
    public Label liveTime;
    public bool timerIsRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        bufTime = root.Q<Label>("buf-time");
        liveTime = root.Q<Label>("time");
        bufTime.text = "00:00";
        liveTime.text = "00:00";
        timerIsRunning = true;
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
}
