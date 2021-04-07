using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CraftingInput : MonoBehaviour
{
    List<InventorySlot> selectedSlots = new List<InventorySlot>();
    string currentRecipe = "";
    public GameObject craftOutputPanel;
    public InventorySlot craftOutputSlot;
    PlayerInventory playerInventory;
    
    void Start()
    {
        playerInventory = PlayerInventory.instance;
    }

    /// <summary>
    /// Call when slot is dragged into craft area to add it to the crafting list and show result recipe
    /// </summary>
    /// <param name="addedSlot"> Slot moved into craft area </param>
    public void UpdateCraftItem(InventorySlot addedSlot)
    {
        if(addedSlot != null)
        {
            selectedSlots.Add(addedSlot);
        }

        // Sort the slots by item name so order of dropping items into crafting doesn't matter
        selectedSlots.Sort((slot1, slot2) => string.Compare(
            slot1.GetItem().physicalPrefab.name, 
            slot2.GetItem().physicalPrefab.name
        ));

        // Converts the sorted selected slots into a String of their item names for dictionary
        currentRecipe = selectedSlots
            .Select(slot => slot.GetItem().physicalPrefab.name)
            .Aggregate((x, y) => x + y);

        // Gets resultant recipe and displays it.
        if (CraftingRecipes.craftingRecipesDict.ContainsKey(currentRecipe))
        {
            string resultantRecipe = CraftingRecipes.craftingRecipesDict[currentRecipe];
            craftOutputPanel.SetActive(true);
            GameObject loadedPrefab = Instantiate(Resources.Load("Prefabs/"+resultantRecipe)) as GameObject;
            craftOutputSlot.AddItem(loadedPrefab.GetComponent<ItemPickupable>().item);
            Destroy(loadedPrefab); // remove the in-world instance of the gameobject. We only need the item instance from it.
        }
        else
        {
            craftOutputSlot.ClearSlot();
            craftOutputPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Call when slot is dragged outside craft area to remove it from the crafting list
    /// </summary>
    /// <param name="removedSlot"> Slot moved out of craft area </param>
    public void RemoveCraftItem(InventorySlot removedSlot)
    {
        selectedSlots.Remove(removedSlot);
    }

    /// <summary>
    /// For consuming all items in crafting area. 
    /// They are all removed from inventory as well.
    /// </summary>
    public void DestroyAllInputItems()
    {
        selectedSlots.ForEach(delegate (InventorySlot slot)
        {
            playerInventory.Remove(slot.GetItem());
            Destroy(slot.gameObject);
        });
        selectedSlots.Clear();
    }
}
