using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianBehavior : MonoBehaviour
{

    TerrainData mapData;
    TreePrototype[] treeProtoList;
    GameObject[] treeList;
    TreeInstance[] originalTrees;
    // Start is called before the first frame update
    void Start()
    {
        originalTrees = this.GetComponent<Terrain>().terrainData.treeInstances;
        mapData = this.GetComponent<Terrain>().terrainData;
        treeProtoList = mapData.treePrototypes;
        treeList = new GameObject[treeProtoList.Length];
        for (int i = 0; i < treeProtoList.Length; i++)
        {
            treeList[i] = treeProtoList[i].prefab;
        }
        ReplaceTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void ReplaceTree()
    {
        for(int i = 0; i < mapData.treeInstanceCount; i++)
        {
            TreeInstance tree = mapData.GetTreeInstance(i);
            int index = tree.prototypeIndex;
            Debug.Log(index);
            Vector3 location = new Vector3(tree.position.x * mapData.size.x, tree.position.y * mapData.size.y, tree.position.z * mapData.size.z);
            float width = tree.widthScale;
            float height = tree.heightScale;
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(0, tree.rotation, 0);
            GameObject treeObject = Instantiate(treeList[index], location,rotation);
            treeObject.transform.localScale = new Vector3(width, height, width);
            
        }
        this.GetComponent<Terrain>().terrainData.treeInstances = new List<TreeInstance>().ToArray();
    }

    public void PlaceObjects(GameObject placedObject)
    {
        Vector3 randLocation;


        //Add code that checks to ensure there is not ground above the random location
        
 
    }

    private void OnApplicationQuit()
    {
        this.GetComponent<Terrain>().terrainData.treeInstances = originalTrees;
    }
}
