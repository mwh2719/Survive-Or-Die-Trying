using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    Button playGameBtn;
    Button quitGameBtn;

    /// <summary>
    /// Attaches behavior to the play button on the main menu. 
    /// Also adds quit app functionality.
    /// </summary>
    void Start()
    {
        playGameBtn = GameObject.Find("playGameBtn").GetComponent<Button>();
        playGameBtn.onClick.AddListener(LoadGame);
        quitGameBtn = GameObject.Find("QuitGameBtn").GetComponent<Button>();
        quitGameBtn.onClick.AddListener(QuitGame);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadGame()
    {
        SceneManager.LoadScene("Map");
    }
    void QuitGame()
    {
        Application.Quit();
    }
}

