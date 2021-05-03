using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class PauseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public Button resumeBtn;
    public Button saveQuitBtn;

    [FMODUnity.EventRef]
    public string buttonClickPath;
    [FMODUnity.EventRef]
    public string backButtonClickPath;

    private EventInstance buttonClickRef;
    private EventInstance backButtonClickRef;
    void Start()
    {
        pauseMenu.SetActive(false);
        resumeBtn.onClick.AddListener(ResumeGame);
        saveQuitBtn.onClick.AddListener(QuitGame);

        buttonClickRef = FMODUnity.RuntimeManager.CreateInstance(buttonClickPath);
        backButtonClickRef = FMODUnity.RuntimeManager.CreateInstance(backButtonClickPath);
    }

    /// <summary>
    /// Pauses/unpauses the game based onf the pause key being pressed.
    /// This behavior should supercede any screen except for the death screen.
    /// All other screens should detect if pause button is pressed and turn themselves off if so.
    /// </summary>
    void Update()
    {
        if(Input.GetButtonDown("Pause") && !PauseController.gameOver)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseController.ForcePauseState(pauseMenu.activeSelf, pauseMenu.activeSelf);
        }
    }

    /// <summary>
    /// Unpauses the game when called.
    /// No effect if game is already unpaused.
    /// </summary>
    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        PauseController.ForcePauseState(false, false);
    }

    /// <summary>
    /// Exits the game back to main menu scene regardless of anything. 
    /// Currently, this does not save anything about the game so only call this after a prompt.
    /// </summary>
    void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayButtonAudio(GameObject button)
    {
        if (button.tag == "AffirmativeButton")
        {
            buttonClickRef.start();
        }
        else if (button.tag == "BackButton")
        {
            backButtonClickRef.start();
        }
    }
}
