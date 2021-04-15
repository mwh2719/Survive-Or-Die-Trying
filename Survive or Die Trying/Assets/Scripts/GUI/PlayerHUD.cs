using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    bool isHidden = false;

    /// <summary>
    /// Will hide the PlayerHUD screen if we are in a GUI.
    /// </summary>
    void Update()
    {
        if (PauseController.gameIsPaused)
        {
            if (!isHidden)
            {
                SetChildrenActiviness(false);
            }
        }
        else
        {
            if (isHidden)
            {
                SetChildrenActiviness(true);
            }
        }
    }

    /// <summary>
    /// Will show/hide all children on the gameobject that this script is on.
    /// </summary>
    /// <param name="isActive">Whether the children should be active or not</param>
    private void SetChildrenActiviness(bool isActive)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
        isHidden = !isActive;
    }
}
