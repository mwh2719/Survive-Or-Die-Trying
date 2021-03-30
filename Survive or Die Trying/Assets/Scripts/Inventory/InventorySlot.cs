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

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.GetItemImage();
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        ShowActions(false);
    }
    

    public void ShowActions(bool show)
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

    public void DropItem()
    {

    }

    public Item GetItem()
    {
        return item;
    }

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
