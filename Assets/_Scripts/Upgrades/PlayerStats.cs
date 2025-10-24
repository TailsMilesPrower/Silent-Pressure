using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Default Stats")]
    public float defaultWalkSpeed = 30f;
    public float defaultRunSpeed = 50f;
    public float defaultCrouchSpeed = 2.5f;
    public float defaultStaminaLimit = 100f;

    [Header("Current Stats")]
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float staminaLimit;

    [Header("Progression")]
    public int upgradesVisited = 0;

    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetStats();

    }

    public void ResetStats()
    {
        walkSpeed = defaultWalkSpeed;
        runSpeed = defaultRunSpeed;
        crouchSpeed = defaultCrouchSpeed;
        staminaLimit = defaultStaminaLimit;

        upgradesVisited = 0;
    }

    public void ApplyUpgrade(string upgradeType)
    {
        switch (upgradeType)
        {
            case ("Increase Walk Speed") : walkSpeed += 10f; 
                break;
            case ("Increase Run Speed") : runSpeed += 10f;
                break;
            case ("Increase Crouch Speed") : crouchSpeed += 10f;
                break;
            case ("Increase Stamina Limit") : staminaLimit += 25f;
                break;
            default: Debug.LogWarning("Unknown upgrade type: " + upgradeType);
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
