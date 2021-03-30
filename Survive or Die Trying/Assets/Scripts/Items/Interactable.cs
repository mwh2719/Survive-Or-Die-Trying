using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source : https://www.youtube.com/watch?v=HQNl3Ff2Lpo
public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;
    
    Transform player;

    public void Start()
    {
        Outline outlineShader = gameObject.AddComponent(typeof(Outline)) as Outline;
        outlineShader.color = 33973;
        outlineShader.enabled = false;
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + transform.name);
    }

    public void OnFocused(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void OnDefocused()
    {
        player = null;
    }

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
