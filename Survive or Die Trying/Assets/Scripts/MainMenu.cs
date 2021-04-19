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
    Button controlsBtn;
    Canvas controls;
    /// <summary>
    /// Attaches behavior to the play button on the main menu. 
    /// Also adds quit app and load controls screen functionality.
    /// </summary>
    void Start()
    {
        playGameBtn = GameObject.Find("playGameBtn").GetComponent<Button>();
        playGameBtn.onClick.AddListener(LoadGame);
        quitGameBtn = GameObject.Find("QuitGameBtn").GetComponent<Button>();
        quitGameBtn.onClick.AddListener(QuitGame);
        controlsBtn = GameObject.Find("ControlsBtn").GetComponent<Button>();
        controlsBtn.onClick.AddListener(ViewControls);
        controls = GameObject.Find("ControlsCanvas").GetComponent<Canvas>();
        controls.enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (controls.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseControls();
            }
        }
    }
    void LoadGame()
    {
        SceneManager.LoadScene("Map");
    }
    void QuitGame()
    {
        Application.Quit();
    }

    void ViewControls()
    {
        controls.enabled = true;
       
        
    }
    void CloseControls()
    {
        controls.enabled = false;
    }
}

