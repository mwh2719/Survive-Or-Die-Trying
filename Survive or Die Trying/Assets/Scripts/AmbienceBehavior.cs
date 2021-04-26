using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AmbienceBehavior : MonoBehaviour
{
    public enum Region { Stone, Beach, Forest};

    private Region currentRegion;
    

    [FMODUnity.EventRef]
    public string forestPath;
    [FMODUnity.EventRef]
    public string birdPath;
    [FMODUnity.EventRef]
    public string beachPath;
    [FMODUnity.EventRef]
    public string seagullPath;
    [FMODUnity.EventRef]
    public string forestSnapPath;
    [FMODUnity.EventRef]
    public string beachSnapPath;


    private EventInstance forestRef;
    private EventInstance birdRef;
    private EventInstance beachRef;
    private EventInstance seagullRef;
    private EventInstance forestSnapRef;
    private EventInstance beachSanpRef;

    private EventInstance playerStepRef;
    // Start is called before the first frame update
    void Start()
    {
        forestRef = FMODUnity.RuntimeManager.CreateInstance(forestPath);
        birdRef = FMODUnity.RuntimeManager.CreateInstance(birdPath);
        beachRef = FMODUnity.RuntimeManager.CreateInstance(beachPath);
        seagullRef = FMODUnity.RuntimeManager.CreateInstance(seagullPath);
        forestSnapRef = FMODUnity.RuntimeManager.CreateInstance(forestSnapPath);
        beachSanpRef = FMODUnity.RuntimeManager.CreateInstance(beachSnapPath);

        playerStepRef = this.GetComponent<PlayerCharacterController>().StepRef;

        forestRef.start();
        birdRef.start();
        beachRef.start();
        seagullRef.start();
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
