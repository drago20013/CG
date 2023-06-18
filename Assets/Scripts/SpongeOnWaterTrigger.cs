using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SpongeOnWaterTrigger : MonoBehaviour
{
    public UIController uiController;
    public GameObject activateIcon;
    public Rigidbody rb;
    // public boolean SpongeOnWater = false;
    public double timeWhenEnteredWater = 0;
    public double sinkTime = -1;
    public int timeOnWater = 30;
    AudioSource audioSource;
    SpongeController controller;
    public float sinkingSpeed = (float)-0.00015;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(uiController.timeRemaining >= sinkTime && sinkTime != -1) {
            gameObject.transform.position += new Vector3 (0,  sinkingSpeed, 0);
            if (gameObject.transform.position.y <= 46.0 && controller.enabled) {
                audioSource.enabled = true;
                Invoke("loadScene", 1.2f);
            }
        }
    }

    private void loadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider other)
    {

        controller = gameObject.GetComponent<SpongeController>();
        if(other.gameObject.layer == 4 && controller.enabled == false) {
            activateIcon.SetActive(true);
            timeWhenEnteredWater = uiController.timeRemaining;
            sinkTime = timeWhenEnteredWater - timeOnWater;


        }
        if(other.gameObject.layer == 7) {
            rb.velocity = new Vector3(0f,0f,0f); 
            rb.angularVelocity = new Vector3(0f,0f,0f);
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

    }
}
