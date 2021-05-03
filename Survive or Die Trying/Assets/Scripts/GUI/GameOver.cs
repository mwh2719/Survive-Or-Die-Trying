using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject deathCanvas;
    public EntityHealth playerHealth;
    public Button respawnBtn;

    [FMODUnity.EventRef]
    public string deathPath;
    private EventInstance deathRef;
    private static Bus playerMovementBus;
    private static Bus playerDamageBus;

    private bool playDeathAudio = true;
    void Start()
    {
        deathRef = FMODUnity.RuntimeManager.CreateInstance(deathPath);
        deathCanvas.SetActive(false);
        respawnBtn.onClick.AddListener(Respawn);

        playerMovementBus = RuntimeManager.GetBus("bus:/SFX/Player/Movement");
        playerDamageBus = RuntimeManager.GetBus("bus:/SFX/Player/Damage");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth.CurrentHealth == 0)
        {
            PlayerDeath();
        }
        else
        {
            playDeathAudio = true;
        }
    }
    /// <summary>
    /// Load death screen
    /// </summary>
    public void PlayerDeath()
    {
        if (playDeathAudio)
        {
            deathRef.start();
            playerMovementBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
            playerDamageBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
            playDeathAudio = false;
        }
        deathCanvas.SetActive(true);
        PauseController.ForcePauseState(deathCanvas.activeSelf, deathCanvas.activeSelf);
        PauseController.gameOver = true;
    }

    /// <summary>
    /// Respawn after dying
    /// </summary>
    void Respawn()
    {
        Scene currScene = SceneManager.GetActiveScene(); //get current scene
        SceneManager.LoadScene(currScene.name); //reload current scene
    }
}
