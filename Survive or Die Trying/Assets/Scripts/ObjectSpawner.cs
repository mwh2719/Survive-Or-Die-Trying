using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private TerrianBehavior mapScript;

    [SerializeField]
    private GameObject[] itemsToSpawn;


    // Start is called before the first frame update
    void Start()
    {
        mapScript.PlaceObjects(itemsToSpawn[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
