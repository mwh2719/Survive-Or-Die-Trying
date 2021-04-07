using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingOutput : MonoBehaviour
{
    public GameObject craftOutputPanel;
    public InventorySlot craftOutputSlot;
    public CraftingInput craftInput;
    PlayerInventory playerInventory;
    
    void Start()
    {
        playerInventory = PlayerInventory.instance;
    }
    
    /// <summary>
    /// Consumes all items in crafting area and adds the created item to the player inventory.
    /// </summary>
    public void CraftItem()
    {
        playerInventory.Add(craftOutputSlot.GetItem());
        craftInput.DestroyAllInputItems();
        craftOutputPanel.SetActive(false);
    }
}
