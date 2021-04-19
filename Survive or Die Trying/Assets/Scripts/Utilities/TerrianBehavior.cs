using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianBehavior : MonoBehaviour
{
    TerrainData mapData;    //Variable to hold refrence to the terrain data
    TreePrototype[] treeProtoList;  //Variable to hold array of the tree prototypes
    GameObject[] treeList;  //Variable to hold array of the tree objects that were used to make the prototypes
    TreeInstance[] originalTrees;   //Array to hold where the trees were originally so that the map data will be restored when the aplication quits
    // Start is called before the first frame update
    void Start()
    {
        //saving the terrain data tree array 
        originalTrees = this.GetComponent<Terrain>().terrainData.treeInstances;

        //getting refernce to terrain data
        mapData = this.GetComponent<Terrain>().terrainData;

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

        //clearing the terrain tree instance array
        this.GetComponent<Terrain>().terrainData.treeInstances = new List<TreeInstance>().ToArray();
    }

    public void PlaceObjects(GameObject placedObject)
    {
        Vector3 randLocation;


        //Add code that checks to ensure there is not ground above the random location
        
 
    }

    private void OnApplicationQuit()
    {
        //reverting the tree instance terrain data
        this.GetComponent<Terrain>().terrainData.treeInstances = originalTrees;
    }
}
