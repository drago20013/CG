using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpongeOnWaterTrigger : MonoBehaviour
{
    //public UIController uiController;
    public GameObject activateIcon;
    public Rigidbody rb;
    // public boolean SpongeOnWater = false;

    private void OnTriggerEnter(Collider other)
    {
        SpongeController controller = gameObject.GetComponent<SpongeController>();
        if(other.gameObject.layer == 4 && controller.enabled == false) {
            activateIcon.SetActive(true);
        }
        if(other.gameObject.layer == 7) {
            rb.velocity = new Vector3(0f,0f,0f); 
            rb.angularVelocity = new Vector3(0f,0f,0f);
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
