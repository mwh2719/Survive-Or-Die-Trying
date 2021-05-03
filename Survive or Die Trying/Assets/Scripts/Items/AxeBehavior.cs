using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

// Will make this show up in the asset context menu so people can quickly make more instances.
[CreateAssetMenu(fileName = "Axe", menuName = "Inventory/Tool/Axe")]
public class AxeBehavior : ToolItem
{
    [FMODUnity.EventRef]
    public string axeSwingPath;


    private EventInstance axeSwingRef;



    public override void InWorldUse(GameObject user)
    {

        axeSwingRef = FMODUnity.RuntimeManager.CreateInstance(axeSwingPath);
        base.InWorldUse(user);
        RaycastHit hit;
        TreeBehavior treeHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, attackRange))
        {
            Debug.Log(hit.collider.name);
            if (hit.transform.TryGetComponent(out treeHit))
            {
                axeSwingRef.setParameterByName("Target", 1);
                axeSwingRef.start();
                if (treeHit.ChopsToCut <= 0)
                {
                    treeHit.ChopDown(user);
                }
                else
                {
                    treeHit.Chop();
                }
            }
            else
            {
                axeSwingRef.setParameterByName("Target", 2);
                axeSwingRef.start();
            }
        }
        else
        {
            axeSwingRef.setParameterByName("Target", 0);
            axeSwingRef.start();
        }
    }
}
