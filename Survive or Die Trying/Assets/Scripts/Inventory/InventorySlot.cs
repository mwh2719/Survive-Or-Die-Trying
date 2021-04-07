using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Source : https://www.youtube.com/watch?v=YLhj7SfaxSE
public class InventorySlot : MonoBehaviour
{
    public Image icon;
    Item item;
    public GameObject useButton;
    public string hotkeyToUse;

    /// <summary>
    /// What item that this inventory slot will hold and display
    /// </summary>
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.GetItemImage();
        icon.enabled = true;
    }

    /// <summary>
    /// Remove item from slot and show nothing
    /// </summary>
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        ShowActions(false);
    }
    
    /// <summary>
    /// Show/hide the action buttons on this slot if the item is usable
    /// </summary>
    /// <param name="show">whether to show/hide the action buttons</param>
    public void ShowActions(bool show)
    {
        if(useButton != null)
        {
            if (show && item.CanBeUsed())
            {
                useButton.SetActive(!useButton.activeSelf);
            }
            else
            {
                useButton.SetActive(false);
            }
        }
    }

    /// <summary>
    /// TODO: add dropping functionality
    /// </summary>
    public void DropItem()
    {

    }

    /// <summary>
    /// Retrieve the item that this slot is holding. Null if no item.
    /// </summary>
    /// <returns>The item held</returns>
    public Item GetItem()
    {
        return item;
    }

    /// <summary>
    /// Consumes the item and calls the item's Use method.
    /// </summary>
    public void UseItem()
    {
        if(item != null)
        {
            // delete item if successfully consumed
            if (item.Use(PlayerInventory.instance.player))
            {
                PlayerInventory.instance.Remove(item);
                ShowActions(false);
            }
        }
    }
}
