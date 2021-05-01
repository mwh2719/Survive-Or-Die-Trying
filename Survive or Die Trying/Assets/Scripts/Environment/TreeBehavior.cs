using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehavior : MonoBehaviour
{

    private int chopsToCut;     //Variable to hold the amount of chops it takes to cut the tree down

    public GameObject log;  //Variable to hold the log that drops when tree is cut down

    // Start is called before the first frame update
    void Start()
    {
        //Setting the amount of chops it takes to a random number between 4 and 7
        chopsToCut = Random.Range(4, 7);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Chop()
    {
        chopsToCut--;
    }

    public void ChopDown(GameObject user)
    {
        /*Debug.Log("fall");
        //creating a push force to make sure the tree falls down
        Vector3 push = user.transform.position - this.transform.position;
        push = push.normalized;

        //Adding a rigibody to the tree  so that gravity can be enabled on it and the push force can be added
        Rigidbody treeBody = this.gameObject.AddComponent<Rigidbody>();
        treeBody.isKinematic = false;
        treeBody.useGravity = true;*/


        int logsToDrop = (int)((this.gameObject.transform.GetChild(0).GetComponent<Renderer>().bounds.max.y - this.gameObject.transform.GetChild(0).GetComponent<Renderer>().bounds.min.y) / 5);

        for (int i = 0; i < logsToDrop; i++)
        {
            float movedX;
            float movedZ;
            float movedY;

            Vector3 newPos;

            movedY = this.transform.position.y + Random.Range(1f, 3f);
            movedX = this.transform.position.x + Random.Range(-3f, 3f);
            movedZ = this.transform.position.z + Random.Range(-3f, 3f);

            newPos = new Vector3(movedX, movedY, movedZ);

            Instantiate(log, newPos, new Quaternion());

            log.GetComponent<Rigidbody>().WakeUp();
        }

        GameObject.Destroy(this.gameObject);
    }


    public int ChopsToCut
    {
        get { return chopsToCut; }
    }
}
