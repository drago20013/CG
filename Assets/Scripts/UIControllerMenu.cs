using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class UIControllerMenu : MonoBehaviour
{

    public Button loadLevel1Btn;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        loadLevel1Btn = root.Q<Button>("level-1");
        loadLevel1Btn.clicked += LoadLevel1;
    }

    void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
