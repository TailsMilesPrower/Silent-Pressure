using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TouchControlsKit;
using Xasu.HighLevel;

public class BreathingMechanic : MonoBehaviour
{
    [Header("References")]
    public BreathingUI breathingUI;
    public Image progressBar;
    public StressMeter stressMeter;
    public PlayerController playerController;

    [Header("PC Prompts")]
    public GameObject aPrompt;
    public GameObject dPrompt;

    [Header("Touch ControlsKit")]
    public string touchButtonName = "BreathButton";

    [Header("Gameplay Settings")]
    public int requiredPresses = 10;
    public float maxTime = 6f;
    public float stressReductionTime = 1.5f;

    [Header("Safe Zone Settings")]
    public bool inSafeZone = false;

    private int currentPresses = 0;
    private float timer = 0f;
    private bool active = false;
    private bool expectingA = true;
    private bool isAndroid;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if UNITY_ANDROID
        isAndroid = true;
#else
        isAndroid = false;
#endif

        if (progressBar)
        {
            progressBar.type = Image.Type.Filled;
            progressBar.fillMethod = Image.FillMethod.Radial360;
            progressBar.fillOrigin = (int)Image.Origin360.Top;
            progressBar.fillClockwise = true;
            progressBar.fillAmount = 0f;
        }

        if (aPrompt) aPrompt.SetActive(false);
        if (dPrompt) dPrompt.SetActive(false);
        if (breathingUI && breathingUI.canvasGroup) breathingUI.canvasGroup.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inSafeZone) return;

        bool touchStart = false;

        if (TCKInput.isActive && !string.IsNullOrEmpty(touchButtonName))
        {
            touchStart = TCKInput.CheckController(touchButtonName) && TCKInput.GetAction(touchButtonName, EActionEvent.Down);
        }

        if (!active)
        {
            if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
            {
                StartBreathing();
            }
            else if (touchStart)
            {
                StartBreathing();
            }

            return;
        }

        HandleBreathingInput();
        timer += Time.deltaTime;

        if (timer >= maxTime)
        {
            EndBreathing(false);
        }

        if (progressBar)
        {
            progressBar.fillAmount = Mathf.Clamp01((float)currentPresses / (float)requiredPresses);
        }


        /* //Old method
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
        */
    }

    void StartBreathing()
    {
        if (!inSafeZone) return;

        CompletableTracker.Instance.Initialized("breathing");

        active = true;
        currentPresses = 0;
        timer = 0f;
        expectingA = true;

        if (playerController) playerController.enabled = false;
        if (breathingUI) breathingUI.FadeIn(); //SetActive(true)

        if (aPrompt) aPrompt.SetActive(!isAndroid);
        if (dPrompt) dPrompt.SetActive(false);

        if (progressBar) progressBar.fillAmount = 0f;

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
        bool countedThisFrame = false;

        if (!isAndroid && Keyboard.current != null)
        {
            if (expectingA && Keyboard.current.aKey.wasPressedThisFrame)
            {
                currentPresses++;
                expectingA = false;
                countedThisFrame = true;
            }
            else if (!expectingA && Keyboard.current.dKey.wasPressedThisFrame)
            {
                currentPresses++;
                expectingA = true;
                countedThisFrame = true;
            }
        }


        if (isAndroid && TCKInput.isActive && !string.IsNullOrEmpty(touchButtonName))
        {
            if (TCKInput.CheckController(touchButtonName))
            {
                bool touchDown = TCKInput.GetAction(touchButtonName, EActionEvent.Down);

                if (touchDown)
                {
                    currentPresses++;
                    //expectingA = !expectingA;
                    countedThisFrame = true;
                }
                
            }
        }

        if (countedThisFrame)
        {
            if (!isAndroid)
            {
                if (aPrompt) aPrompt.SetActive(expectingA);
                if (dPrompt) dPrompt.SetActive(!expectingA);
            }

            if (currentPresses >= requiredPresses)
            {
                EndBreathing(true);
            }   
        }

        /* //Old method
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
        */

    }

    void EndBreathing(bool success)
    {
        active = false;

        CompletableTracker.Instance.Completed("breathing", timer).WithSuccess(success);
        
        if (playerController) playerController.enabled = true;
        if (breathingUI) breathingUI.FadeOut(); //SetActive(false)
        
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        
        if (aPrompt) aPrompt.SetActive(false);
        if (dPrompt) dPrompt.SetActive(false);

        if (progressBar) progressBar.fillAmount = 0f;

        if (stressMeter)
        {
            if (success)
            {
                StartCoroutine(CalmRoutine());
            }
            else
            {
                StartCoroutine(StressRoutine());
            }
        }
    }

    System.Collections.IEnumerator CalmRoutine()
    {
        stressMeter.calming = true;
        yield return new WaitForSeconds(stressReductionTime);
        stressMeter.calming = false;
    }

    System.Collections.IEnumerator StressRoutine()
    {
        stressMeter.stressing = true;
        yield return new WaitForSeconds(stressReductionTime);
        stressMeter.stressing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SafeZone>())
        {
            Debug.Log("Entered Safe Zone!");
            inSafeZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SafeZone>())
        {
            Debug.Log("Exited Safe Zone!");
            inSafeZone = false;
            if (active) EndBreathing(false);
        }
    }

    public void SetSafeZone(bool value)
    {
        inSafeZone = value;

        if (!value && active)
        {
            EndBreathing(false);
        }
    }

}
