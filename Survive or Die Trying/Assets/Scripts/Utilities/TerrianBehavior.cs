using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianBehavior : MonoBehaviour
{
    Terrain map;    //Variable to hold refrence to the terrain
    TerrainData mapData;    //Variable to hold refrence to the terrain data
    TreePrototype[] treeProtoList;  //Variable to hold array of the tree prototypes
    GameObject[] treeList;  //Variable to hold array of the tree objects that were used to make the prototypes
    string[] maskLayers = { "Water", "Terrain", "Stone" };

    LayerMask mask;  //Variable to hold the mask for checking the object spanwer against
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Terrain>().drawTreesAndFoliage = false;
        //getting refernce to terrain data
        map = this.GetComponent<Terrain>();
        mapData = map.terrainData;
        mask = LayerMask.GetMask(maskLayers);

        //getting the tree prototypes
        treeProtoList = mapData.treePrototypes;
        //intializing the tree game object array
        treeList = new GameObject[treeProtoList.Length];

        //populating the tree game object array with refrences to the tree objects
        for (int i = 0; i < treeProtoList.Length; i++)
        {
            treeList[i] = treeProtoList[i].prefab;
        }

        //calling the method to replace the trees with game objects
        ReplaceTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// This method reads all the tree instances on the terrain and replaces them with their corresponding game object
    /// </summary>
    private void ReplaceTree()
    {

        //looping through all the trees on the terrain
        for(int i = 0; i < mapData.treeInstanceCount; i++)
        {
            //collecting all the position and game object information from the current tree
            TreeInstance tree = mapData.GetTreeInstance(i);
            int index = tree.prototypeIndex;
            Vector3 location = new Vector3(tree.position.x * mapData.size.x, tree.position.y * mapData.size.y, tree.position.z * mapData.size.z);
            float width = tree.widthScale;
            float height = tree.heightScale;
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(0, tree.rotation, 0);

            //Making the tree game object
            GameObject treeObject = Instantiate(treeList[index], location,rotation);
            treeObject.transform.localScale = new Vector3(width, height, width);
            
        }
    }

    public void PlaceObjects(GameObject placedObject)
    {
        Vector3 randLocation = SetRandomLocation();
        GameObject temp;
        int countToSpawn = Random.Range(10, 40);

        for (int i = 0; i < countToSpawn; i++)
        {
            do
            //Add code that checks to ensure there is not ground above the random location
            if (Physics.Raycast(randLocation, Vector3.up, Mathf.Infinity, mask))
            {
                randLocation = SetRandomLocation();
            }
            else
            {
                temp = Instantiate(placedObject, randLocation, new Quaternion());
                randLocation = SetRandomLocation();
                break;
            }
            while (true);
        }
    }

    private Vector3 SetRandomLocation()
    {
        map = this.GetComponent<Terrain>();
        mapData = map.terrainData;
        mask = LayerMask.GetMask(maskLayers);
        Vector3 centerMap = mapData.bounds.center;
        centerMap = this.transform.TransformPoint(centerMap);
        float halfMapX = mapData.bounds.size.x / 2;
        float halfMapZ = mapData.bounds.size.z / 2;
        Vector3 random = new Vector3(Random.Range(centerMap.x-halfMapX, centerMap.x + halfMapX), 0, Random.Range(centerMap.z - halfMapZ, centerMap.z + halfMapZ));
        random.y = Terrain.activeTerrain.SampleHeight(random);
        return random;
    }

}
