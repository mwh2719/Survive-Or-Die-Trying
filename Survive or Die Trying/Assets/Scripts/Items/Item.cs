using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public GameObject physicalPrefab = null;

    [SerializeField]
    private Categories.ITEM_TYPE type;
    public Categories.ITEM_TYPE Type
    {
        get { return type; }
    }

    [SerializeField]
    private Sprite image;
    public Sprite GetItemImage()
    {
        return image;
    }


    public virtual bool CanBeUsed()
    {
        return false;
    }

    public virtual bool Use(GameObject user)
    {
        // Do nothing
        return false;
    }
}
