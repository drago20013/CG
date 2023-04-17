using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public UIController uiController;

    void Update()
    {
        if (!uiController.timerIsRunning) {
            UnityEngine.Debug.Log("Time's up, you've died!!");
            SceneManager.LoadScene("Level1");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7){
            UnityEngine.Debug.Log("You've died!!");
            SceneManager.LoadScene("Level1");
        }
    }
}
