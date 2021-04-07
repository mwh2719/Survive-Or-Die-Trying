using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source : https://www.youtube.com/watch?v=HQNl3Ff2Lpo
public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    Transform interactionTransform;
    Transform player;

    /// <summary>
    /// Sets up the shader. 
    /// </summary>
    public void Start()
    {
        Outline outlineShader = gameObject.AddComponent(typeof(Outline)) as Outline;
        outlineShader.color = 33973;
        outlineShader.enabled = false;
        interactionTransform = transform;
    }

    /// <summary>
    /// To be called when player wishes to interact with the object.
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("Interacting with " + transform.name);
    }


    /// <summary>
    /// Gains reference to the player that was looking at this object.
    /// Call this when object is being looked at
    /// </summary>
    public void OnFocused(Transform playerTransform)
    {
        player = playerTransform;
    }

    /// <summary>
    /// Loses reference to the player that was looking at this object.
    /// Call this when object is no longer being looked at
    /// </summary>
    public void OnDefocused()
    {
        player = null;
    }

    /// <summary>
    /// Shows the range for how close the player has to be to trigger the outline.
    /// CURRENTLY UNUSED. Player's reach is what controls how far to grab objects from.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if(interactionTransform == null)
        {
            interactionTransform = transform;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
