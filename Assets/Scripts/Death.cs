using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Death : MonoBehaviour
{
    public UIController uiController;
    AudioSource audioSource;
    public AudioClip pain;
    public SpongeOnWaterTrigger spongeOnWaterTrigger;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (uiController.timeRemaining == 0) {
            UnityEngine.Debug.Log("Time's up, you've died!!");
            loadScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7){
            audioSource.clip = pain;
            audioSource.enabled = true;
            audioSource.Play();

            UnityEngine.Debug.Log("You've died!!");
            Invoke("loadScene", 1.0f);
            Rigidbody rat_rb = GetComponent<Rigidbody>();
            rat_rb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    private void loadScene(){
        UIControllerBusted.levelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("busted");
    }
}
