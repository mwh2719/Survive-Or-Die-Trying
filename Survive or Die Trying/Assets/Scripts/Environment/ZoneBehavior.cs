using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        Debug.Log(this.transform.parent.tag);
        switch (this.transform.parent.tag)
        {
            case "Beach Zone":
                other.gameObject.GetComponent<AmbienceBehavior>().CurrentRegion = AmbienceBehavior.Region.Beach;
                break;
            case "Stone Zone":
                other.gameObject.GetComponent<AmbienceBehavior>().CurrentRegion = AmbienceBehavior.Region.Stone;
                break;
            case "Forest Zone":
                other.gameObject.GetComponent<AmbienceBehavior>().CurrentRegion = AmbienceBehavior.Region.Forest;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
