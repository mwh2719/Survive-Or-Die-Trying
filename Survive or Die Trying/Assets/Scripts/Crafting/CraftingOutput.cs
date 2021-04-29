using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class CraftingOutput : MonoBehaviour
{
    public GameObject craftOutputPanel;
    public InventorySlot craftOutputSlot;
    public CraftingInput craftInput;
    PlayerInventory playerInventory;

    [FMODUnity.EventRef]
    public string craftingPath;

    private EventInstance craftingRef;
    
    void Start()
    {
        craftingRef = FMODUnity.RuntimeManager.CreateInstance(craftingPath);
        playerInventory = PlayerInventory.instance;
    }
    
    /// <summary>
    /// Consumes all items in crafting area and adds the created item to the player inventory.
    /// </summary>
    public void CraftItem()
    {
        craftingRef.start();
        playerInventory.Add(craftOutputSlot.GetItem());
        craftInput.DestroyAllInputItems();
        craftOutputPanel.SetActive(false);
    }
}
