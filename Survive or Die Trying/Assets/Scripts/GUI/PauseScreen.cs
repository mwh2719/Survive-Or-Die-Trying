using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public Button resumeBtn;
    public Button saveQuitBtn;
    void Start()
    {
        pauseMenu.SetActive(false);
        resumeBtn.onClick.AddListener(ResumeGame);
        saveQuitBtn.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseController.ForcePauseState(pauseMenu.activeSelf, pauseMenu.activeSelf);
        }
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        PauseController.ForcePauseState(false, false);
    }

    void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
