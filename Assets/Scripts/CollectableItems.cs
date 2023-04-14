using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    //create a variable for the powers held by player
    public bool antiGravityPower;


    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Player"))
        // {
        // Add code here to give the player some reward for collecting the item

        // Destroy the collectible item
        if (other.gameObject.layer == 6)
        {
            Destroy(other.gameObject);
            antiGravityPower = true;
        }
        // }
    }




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
