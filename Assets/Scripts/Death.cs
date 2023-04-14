using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7){
            Debug.Log("You've died!!");
            SceneManager.LoadScene("Level1");
        }
    }
}
