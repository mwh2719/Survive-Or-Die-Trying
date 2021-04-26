using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseController : MonoBehaviour
{
    public static bool inGui = false;
    public static bool gameIsPaused = false;
    public static bool gameOver = false;
    public static PlayerCharacterController playerCharacterController;

    /// <summary>
    /// Grabs the player at start to be able to disable player controls when paused.
    /// </summary>
    void Awake()
    {
        gameOver = false;
        playerCharacterController = GameObject.Find("Player").GetComponent<PlayerCharacterController>();
        ForcePauseState(false, false);
    }

    /// <summary>
    /// Clicking pause should always exit pause screen.
    /// TODO: make this check for death screen as that cannot be exited.
    /// </summary>
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

    /// <summary>
    /// Call this to force the game to either pause or unpause instead of toggle
    /// </summary>
    /// <param name="pauseState">The new pause state</param>
    /// <param name="isInGui">If a GUI was opened for pausing</param>
    public static void ForcePauseState(bool pauseState, bool isInGui)
    {
        gameIsPaused = pauseState;
        StateChange(gameIsPaused);

        inGui = isInGui;
        if(inGui) Cursor.visible = true;
    }

    /// <summary>
    /// Handles pausing/unpausing the game in its entirety. (timeScale set to 0)
    /// Should never be called outside this class.
    /// </summary>
    /// <param name="newState">The state to change the pause state to</param>
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
