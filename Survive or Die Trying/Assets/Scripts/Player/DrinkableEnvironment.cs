using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkableEnvironment : MonoBehaviour
{
    [SerializeField]
    private float thirstRestoration;

    public void Drink(GameObject drinker)
    {
        // Checks if it is drinkable by entity
        PlayerMainController drinkerStats = drinker.GetComponent<PlayerMainController>();
        if (drinkerStats != null)
        {
            drinkerStats.CurrentThirst += thirstRestoration;
        }
    }
}
