using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    //create a variable for the powers held by player
    public bool antiGravityPower;


    private void OnTriggerEnter(Collider other)
    {
        // Destroy the collectible item
        if (other.gameObject.layer == 6)
        {
            Destroy(other.gameObject);
            antiGravityPower = true;
        }

    }
}
