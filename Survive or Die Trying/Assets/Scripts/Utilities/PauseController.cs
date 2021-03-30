using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseController : MonoBehaviour
{
    public static bool inGui;
    public static bool gameIsPaused;
    public static PlayerCharacterController playerCharacterController;

    void Awake()
    {
        playerCharacterController = FindObjectOfType<PlayerCharacterController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (inGui)
            {
                // Gui will exit themselves
                inGui = false;
            }
        }
    }

    public static void TogglePausing()
    {
        gameIsPaused = !gameIsPaused;
        StateChange(gameIsPaused);
    }

    public static void ForcePauseState(bool pauseState, bool isInGui)
    {
        gameIsPaused = pauseState;
        StateChange(gameIsPaused);

        inGui = isInGui;
        if(inGui) Cursor.visible = true;
    }

    private static void StateChange(bool newState)
    {
        // Pauses the game by making all time based stuff stop
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            playerCharacterController.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            playerCharacterController.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
