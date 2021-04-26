using UnityEngine;
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

    [SerializeField] private float playerAttackRange;
    [SerializeField] private float punchDamage;

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
    private float swimmingTime = 0;
    public float timeBeforeDrowning = 10;
    private float cameraOffset = 0;
    public float cameraBobbingStrength = 0;
    public float fallingSpeedForFallDamage = 5;
    private float previousYSpeed = 0;
    private float fallTimer = 0;

    public Animator playerAnim;     //Variable to hol refrence to player animator

    [FMODUnity.EventRef]
    public string stepPath;
    [FMODUnity.EventRef]
    public string jumpPath;
    private EventInstance stepRef;
    private EventInstance jumpRef;


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
        m_Camera.transform.localPosition = new Vector3(m_Camera.transform.localPosition.x, m_Camera.transform.localPosition.y, .18f);//Fixing camera moving back inside player head

        playerAnim.SetInteger("Health", (int)health.CurrentHealth);

        if (Input.GetButtonDown("Action"))
        {
            //Add code here to see if the player is holding an item
            if (0==1)
            {

            }
            else
            {
                Punch();
            }
            
        }

        RotateView();
        // the jump state needs to read here to make sure it is not missed
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            StartCoroutine(m_JumpBob.DoBobCycle());
            PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }
        
        // Prevents player from being damaged if a glitch stutters the player insanely fast into the ground when they really haven't fell
        if (!m_CharacterController.isGrounded)
        {
            fallTimer += Time.deltaTime;
        }
        else
        {
            fallTimer = 0;
        }

        previousYSpeed = m_CharacterController.velocity.y;
        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }

    private void PlayLandingSound()
    {
        /*m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;*/
    }

    public bool getIsWalking()
    {
        return m_IsWalking;
    }

    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);

        // Move the player in the specified input direction.
        // This allows changing movement in mid-air as it feels more natural in games and prevents "sticking" on trees.
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;
        if (!m_Jumping)
        {
            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
        }

        // Prevent players sticking to mountainside by running to it
        if (isSliding)
        {
            if (!m_CharacterController.isGrounded)
            {
                slidingDirection = slidingDirection.normalized * 0.1f;
            }
            else
            {
                slidingTime += Time.fixedDeltaTime;
                // speed up sliding over time
                slidingDirection *= ((slidingTime / 2) + 1);
                if (m_IsWalking)
                {
                    speed /= 3;
                }
                else
                {
                    speed /= 6;
                }
            }
        }
        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;

        playerAnim.SetFloat("Speed", speed);

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
                m_MoveDir += (Physics.gravity * 0.075f * Time.fixedDeltaTime);

                // move up and down to simulate swimming
                float bobbingSpeed = Mathf.Sin(Time.realtimeSinceStartup * 3);
                m_MoveDir += Vector3.up * bobbingSpeed * cameraBobbingStrength * Time.fixedDeltaTime;
                swimmingTime += Time.deltaTime;


                // swimming takes hunger
                playerMainController.CurrentHunger += -0.01f;
            }
            // sink into water if we can touch bottom
            else
            {
                swimmingTime = 0;
                m_MoveDir += Physics.gravity * m_GravityMultiplier * 0.75f * Time.fixedDeltaTime;
            }

            // Down if player is swimming too long
            if (swimmingTime >= timeBeforeDrowning)
            {
                health.TakeDamage(0.1f, Categories.DAMAGE_TYPE.DROWNING);
            }
        }
        // Disallow jumping in mid-air or sliding. If sliding longer than 3 seconds, allow jumping as player may be stuck.
        else if ((!isSliding && m_CharacterController.isGrounded) || (isSliding && slidingTime > 3))
        {
            if (m_PreviouslyGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;
            }

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
        else
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }

        // Apply sliding force. This is a zero vector if not sliding
        if (!m_Jumping)
        {
            m_MoveDir += slidingDirection;
        }
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
    }
    

    private void PlayJumpSound()
    {
        /*m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();*/
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
        if (!m_CharacterController.isGrounded)
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
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
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
        if (fallTimer > 0.35f && collisionAngle < 75f && previousYSpeed < -fallingSpeedForFallDamage)
        {
            health.TakeDamage(Mathf.Pow(previousYSpeed, 2) / fallingSpeedForFallDamage, Categories.DAMAGE_TYPE.KINETIC);
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

        if (body == null)
        {
            return;
        }

        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f + slidingDirection, hit.point, ForceMode.Impulse);
    }
    
    private void Punch()
    {
        playerAnim.SetTrigger("Punch");
        RaycastHit hit;
        AnimalBehavior animalHit;
        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, playerAttackRange))
        {
            if (hit.transform.TryGetComponent<AnimalBehavior>(out animalHit))
            {
                animalHit.TakeDamage(punchDamage);
            }
        }
    }

    public EventInstance StepRef
    {
        get { return stepRef; }
    }
}
