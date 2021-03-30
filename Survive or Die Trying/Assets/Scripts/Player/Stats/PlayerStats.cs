using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerStats : MonoBehaviour
{
    private PlayerCharacterController fpsController;
    private EntityHealth playerHealth;
   // private Inventory inventory;
    private GameObject sun;
    [SerializeField]
    private LayerMask waterLayer;

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
    }
    
    //Controls the changing of hunger/thirst/health for time-based mechanics.
    void Update()
    {
        if (PauseController.gameIsPaused) return; // Do not do anything if paused

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


        //Food test
        //if (Input.GetKeyDown("c") && inventory.foodInv[0].CanBeUsed())
        //{
        //    inventory.UseFood(0, gameObject);
        //}


        //Environmental Drinking test
        if (Input.GetKeyDown("v"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.transform.forward);
            if (Physics.Raycast(ray, out hit, waterLayer))
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
}
