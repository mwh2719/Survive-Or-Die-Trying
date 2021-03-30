using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source : https://www.youtube.com/watch?v=HQNl3Ff2Lpo
public class PlayerInventory : MonoBehaviour
{
    #region Singleton
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


    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public GameObject player;
    public int space = 20;
    public List<Item> items = new List<Item>();

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

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Clear()
    {
        items.Clear();
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
