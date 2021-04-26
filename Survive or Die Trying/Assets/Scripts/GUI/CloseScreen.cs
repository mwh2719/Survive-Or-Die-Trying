using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseScreen : MonoBehaviour
{
    public GameObject screenToClose;
    
    // Update is called once per frame
    public void CloseTheScreen()
    {
        screenToClose.SetActive(false);
    }
}
