using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectableItems : MonoBehaviour
{
    public string chosenPotion;
    //public bool potionAvailable = false;

    public List<string> availablePotions = new List<string>();

    // public bool triggerCone = false;

    public UIController uiController;
    public bool laserPointerEnable = false;

    private void OnTriggerEnter(Collider other)
    {
        // Destroy the collectible item
        if (other.gameObject.layer == 6)
        {
            string objectTag = other.gameObject.tag;

            if (objectTag == "Cheese")
                loadNextScene();
            else if (objectTag == "laserPointer"){
                UnityEngine.Debug.Log("LaserPointer is picked up");
                laserPointerEnable = true;
            }
            else {
                availablePotions.Insert(0,objectTag);
                changeImage();
            }
            Destroy(other.gameObject);

        }
        // else if (other.gameObject.tag == "triggerCone") {
        //     triggerCone = true;
        // }
    }

    private void loadNextScene() {
        string level = SceneManager.GetActiveScene().name;
        int currentLevel = level[5] - '0';
        currentLevel++;
        SceneManager.LoadScene("Level"+ currentLevel);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void changeImage() 
    {
        UnityEngine.Debug.Log(availablePotions.Count);
        if (availablePotions.Count != 0) 
        {
            uiController.changeImage(availablePotions[0]);
            chosenPotion = availablePotions[0];

        } else {
            uiController.changeImage("");
            chosenPotion = "";
        }
    }


    public void usePotion() 
    {
        //UnityEngine.Debug.Log(availablePotions);
        if (availablePotions.Count > 0) {
            //potionAvailable = true;
            availablePotions.RemoveAt(0);
        }
        else { 
            //potionAvailable = false; 
        }
        changeImage();
    }

    // shift, f.e. [0,1,2] becomes [1,2,0] 
    public void OnTogglePotion() 
    {
        int length = availablePotions.Count;
        if (length > 1){
            string lastElement = availablePotions[length - 1];
            availablePotions.RemoveAt(length - 1);
            availablePotions.Insert(0, lastElement);
            changeImage();
        }
    }
}
