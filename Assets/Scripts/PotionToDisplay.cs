using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PotionToDisplay : MonoBehaviour
{
    public Image currentPotionImage;

    // Technically sprites should be taken in smarter way, but who fucking cares
    public Sprite antiGravityImage;
    public Sprite increaseWeightImage;
    

    // Start is called before the first frame update
    void Start()
    {
        // when no potion make it false always
        currentPotionImage.enabled = false;
    }

    // please forgive me god, it is the ugliest flag but I want to sleep today
    public void changeImage(string potionName) 
    {  
        if (potionName == "antiGravity")
        {
            currentPotionImage.sprite = antiGravityImage;
            currentPotionImage.enabled = true;
        }

        else if (potionName == "increaseWeight")
        {
            currentPotionImage.sprite = increaseWeightImage;
            currentPotionImage.enabled = true;
        }
        else if (potionName == null) 
        {
            currentPotionImage.enabled = false;
        }

    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
