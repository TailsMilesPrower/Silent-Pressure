using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 25f;
    public float staminaRegenRate = 15f;
    public float regenDelay = 1.2f;

    //public bool IsExhausted => currentStamina <= 0f;

    [HideInInspector] public float currentStamina;
    public bool IsSprinting { get; private set; } = false;
    
    private float lastSprintTime = -999f;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        //HandleRegeneration();  
        if (!IsSprinting && Time.time - lastSprintTime >= regenDelay)
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
            }
        }
    }

    public void Drain(float deltaTime)
    {
        if(deltaTime <= 0f) return;

        currentStamina -= staminaDrainRate * deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0f);
        lastSprintTime = Time.time;
    }

    public void SetSprinting(bool sprinting)
    {
        if (sprinting && !IsSprinting)
        {
            IsSprinting = true;
            lastSprintTime = Time.time;
        }
        else if (!sprinting && IsSprinting)
        {
            IsSprinting = false;
            lastSprintTime = Time.time;
        }
        else
        {
            IsSprinting = sprinting;    
        }
    }

    /*
    private void HandleRegeneration()
    {
        if (Time.time - lastSprintTime < regenDelay) return;

        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

    }
    */

    public bool IsExhausted()
    {
        return currentStamina <= 0f;
    }

    public void ResetStamina()
    {
        currentStamina = maxStamina;
        IsSprinting = false;
        lastSprintTime = -999f;
    }
}
