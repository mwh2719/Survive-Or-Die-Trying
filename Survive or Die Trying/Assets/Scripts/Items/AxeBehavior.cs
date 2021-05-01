using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will make this show up in the asset context menu so people can quickly make more instances.
[CreateAssetMenu(fileName = "Axe", menuName = "Inventory/Tool/Axe")]
public class AxeBehavior : ToolItem
{

    public override void InWorldUse(GameObject user)
    {
        base.InWorldUse(user);
        RaycastHit hit;
        TreeBehavior treeHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, attackRange))
        {
            Debug.Log(hit.collider.name);
            if (hit.transform.TryGetComponent(out treeHit))
            {
                if(treeHit.ChopsToCut <= 0)
                {
                    treeHit.ChopDown(user);
                }
                else
                {
                    treeHit.Chop();
                }
            }
        }
    }
}
