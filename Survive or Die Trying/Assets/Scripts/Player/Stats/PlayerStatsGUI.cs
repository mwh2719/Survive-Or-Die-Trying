using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsGUI : MonoBehaviour
{
    private PlayerMainController playerMainController;
    private EntityHealth playerHealth;
    public Slider healthBar;
    public Slider thirstBar;
    public Slider hungerBar;
    
    /// <summary>
    /// Grab the player stats. This script and PlayerStats should be on the same object.
    /// </summary>
    void Start()
    {
        playerMainController = GetComponent<PlayerMainController>();
        playerHealth = GetComponent<EntityHealth>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        thirstBar = GameObject.Find("ThirstBar").GetComponent<Slider>();
        hungerBar = GameObject.Find("HungerBar").GetComponent<Slider>();
        healthBar.maxValue = healthBar.value = playerHealth.maxHealth; //set both current bar value and max bar value to max health at start
        thirstBar.maxValue = thirstBar.value = playerMainController.maxThirst; //same for thirst bar
        hungerBar.maxValue = hungerBar.value = playerMainController.maxHunger; //same for hunger bar
        UpdateGUI();
    }

    /// <summary>
    /// Sets the bar value to the current stats each frame so the gauges will visibly decrease
    /// </summary>
    void UpdateGUI()
    {
        healthBar.value = playerHealth.CurrentHealth;
        thirstBar.value = playerMainController.CurrentThirst;
        hungerBar.value = playerMainController.CurrentHunger;
    }
    
    /// <summary>
    /// Update the GUI so the correct values are visible to the user.
    /// </summary>
    void Update()
    {
        UpdateGUI();
    }
}
