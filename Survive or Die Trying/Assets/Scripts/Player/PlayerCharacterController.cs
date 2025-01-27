﻿using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using PolyPerfect;
using FMOD.Studio;

public class PlayerCharacterController : MonoBehaviour
{

    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;
    [SerializeField] private MouseLook m_MouseLook;
    [SerializeField] private bool m_UseFovKick;
    [SerializeField] private FOVKick m_FovKick = new FOVKick();
    [SerializeField] private bool m_UseHeadBob;
    [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();

    [SerializeField] private float m_StepInterval;

    private Camera m_Camera;
    private bool m_Jump;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;
    private bool isTouchingGround;

    [SerializeField] private float hungerWalkingThreshold;
    private PlayerMainController playerMainController;
    private Rigidbody playerRigidbody;
    private PlayerHealth health;
    private Vector3 slidingDirection;
    private bool isSliding = false;
    public float slidingStrength = 3.5f;
    private float slidingTime = 0;
    public bool inWater = false;
    public bool isSwimming = false;
    private bool wasSwimming = false;
    private float swimmingTime = 0;
    public float timeBeforeDrowning = 10;
    private bool wasDrowning;
    private float cameraOffset = 0;
    public float cameraBobbingStrength = 0;
    public float fallingSpeedForFallDamage = 5;
    private float previousYSpeed = 0;
    private float fallTimer = 0;

    public Animator playerAnim;     //Variable to hol refrence to player animator

    [SerializeField]
    private GameObject head;

    [FMODUnity.EventRef]
    public string stepPath;
    [FMODUnity.EventRef]
    public string jumpPath;
    [FMODUnity.EventRef]
    public string swimPath;
    [FMODUnity.EventRef]
    public string fallWithoutDamagePath;
    [FMODUnity.EventRef]
    public string fallWithDamagePath;
    [FMODUnity.EventRef]
    public string drowningPath;
    [FMODUnity.EventRef]
    public string vomitPath;
    [FMODUnity.EventRef]
    public string attackedPath;

    [FMODUnity.EventRef]
    public string collectPlantPath;
    [FMODUnity.EventRef]
    public string drinkPath;
    [FMODUnity.EventRef]
    public string eatPath;
    [FMODUnity.EventRef]
    public string hungerPath;
    [FMODUnity.EventRef]
    public string thirstPath;
    private EventInstance eatRef;

    private EventInstance stepRef;
    private EventInstance jumpRef;
    private EventInstance swimRef;
    private EventInstance fallWithoutDamageRef;
    private EventInstance fallWithDamageRef;
    private EventInstance drowningRef;
    private EventInstance vomitRef;
    private EventInstance attackedRef;

    private EventInstance collectPlantRef;
    private EventInstance drinkRef;

    private EventInstance hungerRef;
    private EventInstance thirstRef;



    // Use this for initialization
    private void Start()
    {
        playerAnim.fireEvents = false;
        playerMainController = GetComponent<PlayerMainController>();
        m_CharacterController = GetComponent<CharacterController>();
        playerRigidbody = GetComponent<Rigidbody>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_FovKick.Setup(m_Camera);
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_MouseLook.Init(transform, m_Camera.transform);
        health = GetComponent<PlayerHealth>();
        stepRef = FMODUnity.RuntimeManager.CreateInstance(stepPath);
        jumpRef = FMODUnity.RuntimeManager.CreateInstance(jumpPath);
        swimRef = FMODUnity.RuntimeManager.CreateInstance(swimPath);
        fallWithoutDamageRef = FMODUnity.RuntimeManager.CreateInstance(fallWithoutDamagePath);
        fallWithDamageRef = FMODUnity.RuntimeManager.CreateInstance(fallWithDamagePath);
        drowningRef = FMODUnity.RuntimeManager.CreateInstance(drowningPath);
        vomitRef = FMODUnity.RuntimeManager.CreateInstance(vomitPath);
        attackedRef = FMODUnity.RuntimeManager.CreateInstance(attackedPath);
        collectPlantRef = FMODUnity.RuntimeManager.CreateInstance(collectPlantPath);
        drinkRef = FMODUnity.RuntimeManager.CreateInstance(drinkPath);
        eatRef = FMODUnity.RuntimeManager.CreateInstance(eatPath);
        hungerRef = FMODUnity.RuntimeManager.CreateInstance(hungerPath);
        thirstRef = FMODUnity.RuntimeManager.CreateInstance(thirstPath);
    }

    public void SetPositionAndRotation(Vector3 newPosition, Quaternion newRotation)
    {
        m_CharacterController.enabled = false; // Disable and re-enable so character controller grabs new position and rotation.
        transform.SetPositionAndRotation(newPosition, newRotation);
        transform.localRotation = newRotation;
        m_Camera.transform.SetPositionAndRotation(newPosition, newRotation);
        m_Camera.transform.localRotation = newRotation;
        m_MouseLook.Init(transform, m_Camera.transform);
        m_CharacterController.enabled = true;
    }


    // Update is called once per frame
    private void Update()
    {
        playerAnim.SetInteger("Health", (int)health.CurrentHealth);

        RotateView();
        // the jump state needs to read here to make sure it is not missed
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!m_PreviouslyGrounded && isTouchingGround)
        {
            StartCoroutine(m_JumpBob.DoBobCycle());
            PlayLandingSound();
        }
        m_Camera.transform.position = head.transform.position;

    }

    private void PlayLandingSound()
    {
        /*fallWithoutDamageRef.setParameterByName("Step Material", this.GetComponent<AmbienceBehavior>().ConvertRegionToInt());
        fallWithoutDamageRef.start();*/
    }

    public bool getIsWalking()
    {
        return m_IsWalking;
    }

    private void FixedUpdate()
    {

        // use isTouchingGround instead of m_CharacterController.isGrounded as that is bugged with upward movement while touching ground.
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius + 0.0000001f, Vector3.down, out hitInfo,
                           m_CharacterController.height / 2f - 0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        if (hitInfo.collider && (fallTimer == 0 || fallTimer > 0.1f))
        {
            isTouchingGround = true;
        }
        else
        {
            isTouchingGround = false;
        }

        // Get speed that player specifies
        float speed;
        GetInput(out speed);

        if (isTouchingGround)
        {
            fallTimer = 0;
            m_Jumping = false;

            // Prevent players sticking to mountainside by making them slide down it
            if (isSliding)
            {
                slidingTime += Time.fixedDeltaTime;
                // speed up sliding over time
                slidingDirection *= ((slidingTime / 1.5f) + 1);

                // Make running/walking not be able to surpass sliding speed.
                if (m_IsWalking)
                {
                    speed /= 2;
                }
                else
                {
                    speed /= 4;
                }
            }
        }
        else
        {
            // Prevents player from being damaged if a glitch stutters the player insanely fast into the ground when they really haven't fell
            // Keep track of how long we have been falling for.
            fallTimer += Time.deltaTime;

            slidingDirection = Vector3.zero;
            isSliding = false;
        }

        // Move the player in the specified input direction.
        // This allows changing movement in mid-air as it feels more natural in games and prevents "sticking" on trees.
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;
        if (!m_Jumping)
        {
            // get a normal for the surface that is being touched to move along it
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
        }
        
        m_MoveDir = new Vector3(
            desiredMove.x * speed,
            isTouchingGround ? -m_StickToGroundForce : previousYSpeed, 
            desiredMove.z * speed);

        playerAnim.SetFloat("Speed", speed);

        // Apply sliding force. This is a zero vector if not sliding
        m_MoveDir += slidingDirection;

        // Apply regular land gravity
        if (!inWater)
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }

        // move slower when in water
        if (inWater)
        {
            //stop sliding/jumping when in water
            isSliding = false;
            m_Jump = false;
            m_Jumping = false;
            slidingTime = 0;
            
            // Holding running while swimming gives a smaller speed boost
            if (m_IsWalking)
            {
                m_MoveDir *= 0.45f;
            }
            else
            {
                m_MoveDir *= 0.35f;
            }


            // we are now in swimming mode. Sink slowly...
            if (isSwimming)
            {
                if (!wasSwimming)
                {
                    swimRef.start();
                }

                // slower gravity pull
                m_MoveDir += (Physics.gravity * 0.075f * Time.fixedDeltaTime);

                // move up and down to simulate swimming
                float bobbingSpeed = Mathf.Sin(Time.realtimeSinceStartup * 3);
                m_MoveDir += Vector3.up * bobbingSpeed * cameraBobbingStrength * Time.fixedDeltaTime;
                swimmingTime += Time.deltaTime;


                // swimming takes hunger
                playerMainController.CurrentHunger += -0.01f;
            }
            // sink into water so we can touch bottom and aren't in swimming mode yet
            else
            {
                if (wasSwimming)
                {
                    swimRef.stop(STOP_MODE.ALLOWFADEOUT);
                }
                swimmingTime = 0;
                m_MoveDir += Physics.gravity * m_GravityMultiplier * 0.75f * Time.fixedDeltaTime;
            }

            // Drown if player is swimming too long
            if (swimmingTime >= timeBeforeDrowning)
            {
                if (!wasDrowning)
                {
                    drowningRef.start();
                }
                health.TakeDamage(0.1f, Categories.DAMAGE_TYPE.DROWNING);
                wasDrowning = true;
            }
            else if(wasDrowning)
            {
                drowningRef.stop(STOP_MODE.ALLOWFADEOUT);
                wasDrowning = false;
            }
        }

        // Disallow jumping in mid-air or sliding. If sliding longer than 3 seconds, allow jumping as player may be stuck.
        else if ((!isSliding && isTouchingGround) || (isSliding && slidingTime > 3))
        {
            if (m_Jump)
            {
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
                isSliding = false;
                slidingTime = 0; // reset so players need to wait 3 seconds before jumping again if doing it while sliding

                // make jumping cost hunger
                playerMainController.CurrentHunger += -0.1f;
            }
        }
        //Debug.Log(m_MoveDir + " isSliding: " + isSliding + " isGrounded: " + isTouchingGround);
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        // reset camera bobbing when exiting water
        if(!inWater && cameraOffset != 0)
        {
            m_Camera.transform.Translate(0, cameraOffset, 0, Space.World);
            cameraOffset = 0;
        }
        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);
        m_MouseLook.UpdateCursorLock();

        previousYSpeed = m_CharacterController.velocity.y;
        m_PreviouslyGrounded = isTouchingGround;

        wasSwimming = isSwimming;
    }
    

    private void PlayJumpSound()
    {
        jumpRef.start();
    }

    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                         Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        if (!isTouchingGround || isSwimming)
        {
            return;
        }
        stepRef.start();
    }


    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!m_UseHeadBob)
        {
            return;
        }
        if (m_CharacterController.velocity.magnitude > 0 && isTouchingGround)
        {
            m_Camera.transform.localPosition =
                m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                  (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }
        m_Camera.transform.localPosition = newCameraPosition;
    }


    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool standingStill = true;
        if(horizontal != 0 || vertical != 0)
        {
            standingStill = false;
        }

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey(KeyCode.LeftShift) || playerMainController.CurrentHunger < hungerWalkingThreshold;
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        if (standingStill) { speed = 0; }
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
        }
    }


    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Deals fall damage if it hits ground at high speed
        float collisionAngle = Vector3.Angle(hit.normal, Vector3.up);
        if (fallTimer > 0.35f && collisionAngle < 80f && previousYSpeed < -fallingSpeedForFallDamage)
        {
            health.TakeDamage(Mathf.Pow(Mathf.Abs(previousYSpeed), 2f) / fallingSpeedForFallDamage, Categories.DAMAGE_TYPE.KINETIC);
            fallWithDamageRef.setParameterByName("Step Material", this.GetComponent<AmbienceBehavior>().ConvertRegionToInt());
            fallWithDamageRef.start();
        }
        else if (fallTimer > .1f)
        {
            fallWithoutDamageRef.setParameterByName("Step Material", this.GetComponent<AmbienceBehavior>().ConvertRegionToInt());
            fallWithoutDamageRef.start();
        }
        Rigidbody body = hit.collider.attachedRigidbody;

        // detects if player should be sliding and will create the sliding vector if so.
        // FixedUpdate will take that vector to actually apply sliding
        if (!inWater && collisionAngle > m_CharacterController.slopeLimit)
        {
            isSliding = true;
            Vector3 normal = hit.normal;
            Vector3 c = Vector3.Cross(Vector3.up, normal);
            Vector3 u = Vector3.Cross(c, normal);
            u.y = Mathf.Clamp(u.y, -0.75f, -0.1f);
            if (c.magnitude < 0.2f)
            {
                u *= 0.1f;
            }
            slidingDirection = u * slidingStrength;
            /*
            //source: http://answers.unity.com/answers/1387707/view.html
            slidingDirection = new Vector3
            {
                x = ((1f - normal.y) * normal.x) * slidingStrength,
                z = ((1f - normal.y) * normal.z) * slidingStrength
            };
            */
        }
        // Is not sliding, reset variables
        else
        {
            isSliding = false;
            slidingTime = 0;
            slidingDirection = Vector3.zero;
        }
    }

    public EventInstance StepRef
    {
        get { return stepRef; }
    }

    public float PlayerStepMaterial
    {
        set { stepRef.setParameterByName("Step Material", value); }
    }

    public float WaterDepth
    {
        set { stepRef.setParameterByName("Water Depth", value); }
    }

    public void PlayEatSound() { eatRef.start(); }
    public void PlayDrinkSound() { drinkRef.start(); }

    public void PlayAttackedSound() { attackedRef.start(); }

    public void PlayCollectPlantSound() { collectPlantRef.start(); }

    public void PlayHungerDialog() { hungerRef.start(); }

    public void PlayThirstDialog() { thirstRef.start(); }
}
