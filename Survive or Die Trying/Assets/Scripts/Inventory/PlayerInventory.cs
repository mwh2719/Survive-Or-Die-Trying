using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source : https://www.youtube.com/watch?v=HQNl3Ff2Lpo
public class PlayerInventory : MonoBehaviour
{
    #region Singleton
    /// <summary>
    /// The single player inventory instance that can be accessed from anywhere.
    /// There will never be more than one.
    /// </summary>
    public static PlayerInventory instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one inventory instance detected.");
            return;
        }
        instance = this;
    }
    #endregion

    /// <summary>
    /// The event that will be fired every time an item is added or removed from inventory.
    /// 
    /// Add methods to invoke in the event by doing this:
    /// PlayerInventory.instance.onItemChangedCallback += MethodToCall;
    /// 
    /// Never do an = operation. Always += so that other assigned listeners do not get removed.
    /// </summary>
    public OnItemChanged onItemChangedCallback;
    public delegate void OnItemChanged();

    public GameObject player;
    public int space = 20;
    public List<Item> items = new List<Item>();

    /// <summary>
    /// Adds the item to the inventory if there is enough room.
    /// Will trigger onItemChangedCallback event.
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <returns>Whether the item was successfully added or not to inventory</returns>
    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room.");
            return false;
        }


        items.Add(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        return true;
    }

    /// <summary>
    /// Removes the specified item from the inventory.
    /// Will trigger onItemChangedCallback event.
    /// </summary>
    /// <param name="item">item to remove</param>
    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    /// <summary>
    /// Clears out all items from the inventory. 
    /// This should only be called if player dies or something as EVERYTHING will be gone.
    /// Will trigger onItemChangedCallback event.
    /// </summary>
    public void Clear()
    {
        items.Clear();
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
