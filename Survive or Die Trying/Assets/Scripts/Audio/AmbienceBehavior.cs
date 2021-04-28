using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AmbienceBehavior : MonoBehaviour
{
    public enum Region { Stone, Beach, Forest};

    private Region currentRegion;
    

    [FMODUnity.EventRef]
    public string forestSnapPath;
    [FMODUnity.EventRef]
    public string beachSnapPath;


    private EventInstance forestSnapRef;
    private EventInstance beachSanpRef;

    private EventInstance playerStepRef;
    // Start is called before the first frame update
    void Start()
    {
        forestSnapRef = FMODUnity.RuntimeManager.CreateInstance(forestSnapPath);
        beachSanpRef = FMODUnity.RuntimeManager.CreateInstance(beachSnapPath);

        playerStepRef = this.GetComponent<PlayerCharacterController>().StepRef;

    }

    // Update is called once per frame
    void Update()
    {
        if(currentRegion == Region.Beach)
        {
            beachSanpRef.start();

            //add code to change material step type here
            playerStepRef.setParameterByName("Step Material", 2);
        }
        else if(currentRegion == Region.Forest)
        {
            forestSnapRef.start();

            //add code to change material step type here
            playerStepRef.setParameterByName("Step Material", 1);
        }
        else if(currentRegion == Region.Stone)
        {

            //add code to change material step type here
            playerStepRef.setParameterByName("Step Material", 0);
        }
    }

    public Region CurrentRegion
    {
        get { return currentRegion; }
        set { currentRegion = value; }
    }
}
