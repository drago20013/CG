using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIControllerBusted : MonoBehaviour
{
    public VisualElement background;
    public Sprite bustedSprite;
    public static string levelName;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        background = root.Q<VisualElement>("BustedScene");
    }

    void LoadRestart()
    {
        // SceneManager.LoadScene("Level1");
    }

    void LoadMainMenu()
    {
        // SceneManager.LoadScene("Level2");
    }


    public void Restart(InputAction.CallbackContext context) {
        SceneManager.LoadScene(levelName);
    }


    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}

