using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AmbienceBehavior : MonoBehaviour
{

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
    // Start is called before the first frame update
    void Start()
    {
        forestRef = FMODUnity.RuntimeManager.CreateInstance(forestPath);
        birdRef = FMODUnity.RuntimeManager.CreateInstance(birdPath);
        beachRef = FMODUnity.RuntimeManager.CreateInstance(beachPath);
        seagullRef = FMODUnity.RuntimeManager.CreateInstance(seagullPath);
        forestSnapRef = FMODUnity.RuntimeManager.CreateInstance(forestSnapPath);
        beachSanpRef = FMODUnity.RuntimeManager.CreateInstance(beachSnapPath);
        forestRef.start();
        birdRef.start();
        beachRef.start();
        seagullRef.start();
        beachSanpRef.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
