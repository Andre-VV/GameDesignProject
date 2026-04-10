using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public TMP_Dropdown levelSelect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameButton()
    {
        Debug.Log("Start Game Button Pressed");
        int index = levelSelect.value;

        switch (index)
        {
            case 0:
                Debug.Log("Loading Level 1...");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
                break;
            case 1:
                Debug.Log("Loading Level 2...");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
                break;
            case 2:
                Debug.Log("Loading Level 3...");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
                break;
            case 3:
                Debug.Log("Loading Level 4...");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("Level4");
                break;
            default:
                Debug.LogError("Invalid level index selected: " + index);
                break;
        }

    }
}
