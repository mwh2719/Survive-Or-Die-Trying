using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBox : MonoBehaviour
{
    private Camera playerCamera;
    private PlayerCharacterController player;
    private BoxCollider waterbox;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCharacterController>();
        playerCamera = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
        waterbox = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Tells player when they touched water
    /// </summary>
    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag.Equals("Player"))
        {
            player.inWater = true;
        }
    }

    /// <summary>
    /// Tells player they are deep enough into water that they need to swim now
    /// </summary>
    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag.Equals("Player"))
        {
            if (waterbox.bounds.Contains(playerCamera.transform.position - new Vector3(0, 0.45f, 0)))
            {
                player.isSwimming = true;
            }
            else
            {
                player.isSwimming = false;
            }
        }
    }

    /// <summary>
    /// Tells player when they exit water
    /// </summary>
    void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag.Equals("Player"))
        {
            player.inWater = false;
            player.isSwimming = false;
        }
    }
}
