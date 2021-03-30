using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source : https://www.youtube.com/watch?v=HQNl3Ff2Lpo
public class ItemPickupable : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }
    
    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        PlayerInventory.instance.Add(item);
        Destroy(gameObject);
    }
}
