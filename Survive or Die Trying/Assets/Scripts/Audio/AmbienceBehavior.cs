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

        playerStepRef.setParameterByName("Step Material", ConvertRegionToInt());

        switch (currentRegion)
        {
            case Region.Beach:
                beachSanpRef.start();
                break;
            case Region.Forest:
                forestSnapRef.start();
                break;
            case Region.Stone:
                break;
            default:
                forestSnapRef.start();
                break;
        }

    }

    public Region CurrentRegion
    {
        get { return currentRegion; }
        set { currentRegion = value; }
    }

    public int ConvertRegionToInt()
    {
        switch (currentRegion)
        {
            case Region.Beach:
                return 2;
            case Region.Forest:
                return 1;
            case Region.Stone:
                return 0;
            default:
                return 1;
        }
        
    }

}
