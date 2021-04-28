using cakeslice;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerMainController : MonoBehaviour
{
    private PlayerCharacterController fpsController;
    private EntityHealth playerHealth;
    private GameObject sun;
    [SerializeField]
    private LayerMask waterLayer;
    [SerializeField]
    private LayerMask interactableLayer;
    [SerializeField]
    private float playerReach = 9;
    private Quaternion spawnRotation;
    private Vector3 spawnPosition;
    private Interactable focus;


    [SerializeField] private float currentHunger;
    public float maxHunger;
    public float CurrentHunger
    {
        get { return currentHunger; }
        set { currentHunger = Mathf.Clamp(value, 0, maxThirst); }
    }

    [SerializeField] private float currentThirst;
    public float maxThirst;
    public float CurrentThirst
    {
        get { return currentThirst; }
        set { currentThirst = Mathf.Clamp(value, 0, maxThirst); }
    }


    void Start()
    {
        fpsController = GetComponent<PlayerCharacterController>();
        playerHealth = GetComponent<EntityHealth>();
        sun = GameObject.Find("Sun");
        spawnRotation = fpsController.transform.rotation;
        spawnPosition = fpsController.transform.position;
    }

    // Call this when you want to reset player back to default state
    public void ResetPlayer()
    {
        playerHealth.ResetHealth();
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        PlayerInventory.instance.Clear();
        fpsController.SetPositionAndRotation(spawnPosition, spawnRotation);
    }

    //Controls the changing of hunger/thirst/health for time-based mechanics.
    void Update()
    {
        if (PauseController.gameIsPaused) return; // Do not do anything if paused

        if(playerHealth.CurrentHealth <= 0) 
        {

            return;
        }
        

        /// hunger ///
        float hungerDrain = -0.0001f;

        if (!fpsController.getIsWalking()) // running
        {
            hungerDrain -= 0.0005f;
        }

        CurrentHunger += hungerDrain;


        /// thirst ///
        CurrentThirst += -0.0001f;

        // get thirstier faster when not in the shade
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, -sun.transform.forward, out hit, 100))
        {
            CurrentThirst += -0.0002f;
        }

        if (!fpsController.getIsWalking()) // running
        {
            CurrentThirst -= 0.0003f;
        }

        /// health ///
        if (currentHunger == 0)
        {
            playerHealth.TakeDamage(0.001f, Categories.DAMAGE_TYPE.HUNGER);
        }

        if (currentThirst == 0)
        {
            playerHealth.TakeDamage(0.001f, Categories.DAMAGE_TYPE.THIRST);
        }



        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hitInteractableLayer = Physics.Raycast(ray, out hit, playerReach, interactableLayer);
        if (hitInteractableLayer)
        {
            Interactable objInteractable = hit.collider.GetComponent<Interactable>();
            if (objInteractable)
            {
                SetFocus(objInteractable);
            }

            if (Input.GetButtonDown("Use"))
            {
                objInteractable.Interact();
            }
        }
        else
        {
            RemoveFocus();
        }


        //Environmental Drinking test
        if (Input.GetButtonDown("EnvironmentIteract"))
        {
            bool hitWaterLayer = Physics.Raycast(ray, out hit, playerReach, waterLayer);
            if (hitWaterLayer)
            {
                if (hit.collider && hit.distance < 0.5)
                {
                    GameObject obj = hit.collider.gameObject;
                    DrinkableEnvironment drinkableScript = obj.GetComponent<DrinkableEnvironment>();
                    if (drinkableScript)
                    {
                        drinkableScript.Drink(gameObject);
                    }
                }
            }
        }
    }

    
    void SetFocus(Interactable newFocus)
    {
        Outline outline;
        if (newFocus != focus)
        {
            if (focus != null)
            {
                outline = focus.GetComponent<Outline>();
                if (outline)
                {
                    outline.enabled = false;
                }
                focus.OnDefocused();
            }

            focus = newFocus;
            outline = focus.GetComponent<Outline>();
            if (outline)
            {
                outline.enabled = true;
            }
            newFocus.OnFocused(transform);
        }
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            Outline outline = focus.GetComponent<Outline>();
            if (outline)
            {
                outline.enabled = false;
            }
            focus.OnDefocused();
        }
        focus = null;
    }
}
