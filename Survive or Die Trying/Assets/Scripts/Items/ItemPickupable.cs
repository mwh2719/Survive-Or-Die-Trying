using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source : https://www.youtube.com/watch?v=HQNl3Ff2Lpo
public class ItemPickupable : Interactable
{
    public Item item;

    /// <summary>
    /// Call this when the player clicks to interact with the item.
    /// </summary>
    public override void Interact()
    {
        base.Interact();
        PickUp();
    }
    
    /// <summary>
    /// Will remove the object from game-space and add the item to the player's inventory.
    /// </summary>
    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        PlayerInventory.instance.Add(item);
        Destroy(gameObject);
    }
}
