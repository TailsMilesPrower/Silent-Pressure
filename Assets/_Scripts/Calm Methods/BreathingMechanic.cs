using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BreathingMechanic : MonoBehaviour
{
    [Header("References")]
    public GameObject breathingUI;
    public Image progressBar;
    public StressMeter stressMeter;
    public PlayerController playerController;

    [Header("Gameplay Settings")]
    public int requiredPresses = 10;
    public float maxTime = 6f;
    public float stressReductionTime = 1.5f;

    private int currentPresses = 0;
    private float timer = 0f;
    private bool active = false;
    private bool expectingA = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame && !active)
        {
            StartBreathing();
        }

        if (active)
        {
            HandleBreathingInput();
            timer += Time.deltaTime;

            if (timer >= maxTime)
            {
                EndBreathing(false);
            }

            if (progressBar)
            {
                progressBar.fillAmount = (float)currentPresses / requiredPresses;
            }
        }
    }

    void StartBreathing()
    {
        if (breathingUI) breathingUI.SetActive(true);
        if (playerController) playerController.enabled = false;

        currentPresses = 0;
        timer = 0f;
        active = true;
        expectingA = true;

        if (stressMeter)
        {
            stressMeter.stressing = false;
            stressMeter.calming = false;
        }

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;

    }

    void HandleBreathingInput()
    {
        if (expectingA && Keyboard.current.aKey.wasPressedThisFrame)
        {
            currentPresses++;
            expectingA = false;
        }
        else if (!expectingA && Keyboard.current.dKey.wasPressedThisFrame)
        {
            currentPresses++;
            expectingA = true;
        }

        if (currentPresses >= requiredPresses)
        {
            EndBreathing(true);
        }

    }

    void EndBreathing(bool success)
    {
        active = false;
        if (breathingUI) breathingUI.SetActive(false);
        if (playerController) playerController.enabled = true;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        if (stressMeter)
        {
            if (success)
            {
                StartCoroutine(CalmRoutine());
            }
        }
    }

    System.Collections.IEnumerator CalmRoutine()
    {
        stressMeter.calming = true;
        yield return new WaitForSeconds(stressReductionTime);
        stressMeter.calming = false;
    }

}
