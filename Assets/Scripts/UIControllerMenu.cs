using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;



public class UIControllerMenu : MonoBehaviour
{

    //public Button loadLevel1Btn, loadLevel2Btn;
    public VisualElement background;
    public Sprite startScene, chooseLevelScene, authorScene;
    public Button b_level1, b_level2, b_level3;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        b_level1 = root.Q<Button>("Level1");
        b_level2 = root.Q<Button>("Level2");
        b_level3 = root.Q<Button>("Level3");

        background = root.Q<VisualElement>("StartScene");
        b_level1.clicked += LoadLevel1;
        b_level2.clicked += LoadLevel2;
        b_level3.clicked += LoadLevel3;

    }

    void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void ChooseLevelScene(InputAction.CallbackContext context)
    {
        b_level1.style.display = DisplayStyle.Flex;
        b_level2.style.display = DisplayStyle.Flex;
        b_level3.style.display = DisplayStyle.Flex;

        background.style.backgroundImage = new StyleBackground(chooseLevelScene);
    }

    public void LoadCredits(InputAction.CallbackContext context)
    {
        b_level1.style.display = DisplayStyle.None;
        b_level2.style.display = DisplayStyle.None;
        b_level3.style.display = DisplayStyle.None;

        background.style.backgroundImage = new StyleBackground(authorScene);
    }

    void LoadMenu()
    {
        b_level1.style.display = DisplayStyle.None;
        b_level2.style.display = DisplayStyle.None;
        b_level3.style.display = DisplayStyle.None;

        background.style.backgroundImage = new StyleBackground(startScene);
    }
        
    public void Exit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (background.style.backgroundImage == new StyleBackground(startScene))
            {
                Application.Quit();
            }
            else
                LoadMenu();
        }
    }
}
