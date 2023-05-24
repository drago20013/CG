using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SpongeController : MonoBehaviour
{

        public Rigidbody rb;
        public Transform cam;
        public GameObject player;
        public float speed, maxForce;
        private Vector2 move;  

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnF(InputAction.CallbackContext context) 
    {
        if(context.performed)
            switchControllerToSponge();
    }

    public void switchControllerToSponge() {
        enabled = false;
        player.SetActive(true);
        GameObject fakeRat = gameObject.transform.GetChild(1).gameObject;
        GameObject triggerCone = gameObject.transform.GetChild(0).gameObject;
        fakeRat.SetActive(false);
        triggerCone.SetActive(true);
        UnityEngine.Debug.Log("Controller IS Switched");
    }

    void Start(){
        speed = 10;
        maxForce = 1;
    } 

    void FixedUpdate()
    {
        Move();
        player.transform.position = new Vector3(gameObject.transform.position.x, player.transform.position.y, gameObject.transform.position.z);
    }


    void Move()
    {
        UnityEngine.Debug.Log("AaAAAAAAAAAAAAAA");
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.y, 0, -move.x);
     
        targetVelocity = Quaternion.Euler(0f, cam.eulerAngles.y - 90, 0f) * targetVelocity;
        targetVelocity *= speed;

        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0f, velocityChange.z);

        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
}
