using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject deathCanvas;
    public EntityHealth playerHealth;
    public Button respawnBtn;

    [FMODUnity.EventRef]
    public string deathPath;
    private EventInstance deathRef;
    void Start()
    {
        deathRef = FMODUnity.RuntimeManager.CreateInstance(deathPath);
        deathCanvas.SetActive(false);
        respawnBtn.onClick.AddListener(Respawn);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth.CurrentHealth == 0)
        {
            PlayerDeath();
        }
    }
    /// <summary>
    /// Load death screen
    /// </summary>
    public void PlayerDeath()
    {
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
