using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CraftingRecipes : MonoBehaviour
{
    #region Singleton
    /// <summary>
    /// The single crafting recipes dictionary instance that can be accessed from anywhere.
    /// There will never be more than one.
    /// </summary>
    public static Dictionary<string, string> craftingRecipesDict;
    void Awake()
    {
        if (craftingRecipesDict != null)
        {
            Debug.LogWarning("More than one craftingRecipesDict instance detected.");
            return;
        }
        craftingRecipesDict = new Dictionary<string, string>() { };
    }
    #endregion

    /// <summary>
    /// Hack to make Unity show an editable Dictionary in the Inspector window
    /// Source: http://answers.unity.com/answers/814629/view.html
    /// </summary>
    [Serializable]
    public struct DictionaryEntries
    {
        public GameObject[] inputItems;
        public GameObject outputItem;
    }
    public DictionaryEntries[] craftingRecipes;
    PlayerInventory playerInventory;

    /// <summary>
    /// Converts the inspector crafting entries into a usable crafting dictionary to access from anywhere.
    /// </summary>
    void Start()
    {
        if(craftingRecipesDict.Count != 0)
        {
            Debug.LogWarning("More than one CraftingRecipes script is added to the scene! Remove one of them as they will override each other's recipes.");
        }

        playerInventory = PlayerInventory.instance;
        craftingRecipesDict.Clear();
        foreach (DictionaryEntries entries in craftingRecipes)
        {
            craftingRecipesDict.Add(
                entries.inputItems.Select((val) => val.name).Aggregate((x, y) => x + y),
                entries.outputItem.name
            );
        }
    }
}
