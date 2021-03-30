using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGame : MonoBehaviour
{
    // Start is called before the first frame update
    Button playGameBtn;
    void Start()
    {
        playGameBtn = GameObject.Find("playGameBtn").GetComponent<Button>();
        playGameBtn.onClick.AddListener(LoadGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadGame()
    {
        SceneManager.LoadScene("Map");
    }
}
