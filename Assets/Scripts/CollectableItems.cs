using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CollectableItems : MonoBehaviour
{
    //create a variable for the powers held by player
    // public bool antiGravityPower;
    // public bool increaseWeightPower;

    public string chosenPotion;

    public List<string> availablePotions = new List<string>();

    public PotionToDisplay potionToDisplay;


    private void Start() 
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        // Destroy the collectible item
        if (other.gameObject.layer == 6)
        {
            string potionName = other.gameObject.tag;
            Destroy(other.gameObject);
            UnityEngine.Debug.Log(tag);
            availablePotions.Insert(0,potionName);
            this.changeImage();
        }
    }

    void changeImage() 
    {
        UnityEngine.Debug.Log(this.availablePotions.Count);
        if (availablePotions.Count != 0) 
        {
            this.chosenPotion = this.availablePotions[0];
            potionToDisplay.changeImage(this.chosenPotion);

        } else {
            potionToDisplay.changeImage(null);
        }
    }


    public void usePotion() 
    {
        UnityEngine.Debug.Log(availablePotions);
        this.availablePotions.RemoveAt(0);
        this.changeImage();
    }

    // shift, f.e. [0,1,2] becomes [1,2,0] 
    public void OnTogglePotion() 
    {
        int length = availablePotions.Count;
        if (length > 1){
            string lastElement = availablePotions[length - 1];
            availablePotions.RemoveAt(length - 1);
            availablePotions.Insert(0, lastElement);
            this.changeImage();

        }
    }
}
