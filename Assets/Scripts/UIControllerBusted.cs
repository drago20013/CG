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

    void LoadMainMenu()
    {
        UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }


    public void Restart(InputAction.CallbackContext context) {
        SceneManager.LoadScene(levelName);
    }


    public void Exit()
    {
        LoadMainMenu();
    }
}

