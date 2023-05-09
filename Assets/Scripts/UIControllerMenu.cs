using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class UIControllerMenu : MonoBehaviour
{

    public Button loadLevel1Btn, loadLevel2Btn;
    public Button exitBtn;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        loadLevel1Btn = root.Q<Button>("level-1");
        loadLevel1Btn.clicked += LoadLevel1;
        loadLevel2Btn = root.Q<Button>("level-2");
        loadLevel2Btn.clicked += LoadLevel2;
        exitBtn = root.Q<Button>("Exit");
        exitBtn.clicked += Exit;
    }

    void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    void Exit()
    {
        Application.Quit();
    }
}
