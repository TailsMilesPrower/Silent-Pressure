using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class StaminaUI : MonoBehaviour
{
    [Header("References")]
    public PlayerStamina playerStamina;
    public Image fillImage;

    [Header("UI Settings")]
    public float smoothSpeed = 8f;
    public float fadeOutDelay = 1f;
    public float fadeSpeed = 4f;
    public Color fullColor = Color.green;
    public Color lowColor = Color.red;

    private CanvasGroup canvasGroup;
    private float displayedFill = 1f;
    private float lastShownTime = - 999f;
    //public bool IsFadingOut = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (fillImage != null)
        {
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            fillImage.fillAmount = 1f;
        }

        canvasGroup.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStamina == null || fillImage == null) return;

        float targetFill = Mathf.Clamp01(playerStamina.currentStamina / playerStamina.maxStamina);

        displayedFill = Mathf.Lerp(displayedFill, targetFill, Time.deltaTime * smoothSpeed);
        fillImage.fillAmount = displayedFill;

        fillImage.color = Color.Lerp(lowColor, fullColor, displayedFill);

        //bool isLow = playerStamina.currentStamina < playerStamina.maxStamina * 0.999f;
        //bool isSprinting = playerStamina.IsSprinting;
        bool shouldShow = playerStamina.IsSprinting || playerStamina.currentStamina < playerStamina.maxStamina - 0.001f;

        if (shouldShow)
        {
            lastShownTime = Time.time;
            //IsFadingOut = false;
        }

        float timeSinceShown = Time.time - lastShownTime;
        float targetAlpha = (timeSinceShown <= fadeOutDelay) ? 1f : 0f;

        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        //CanvasGroup group = GetComponent<CanvasGroup>();
        //if (group) group.alpha = (playerStamina.currentStamina < playerStamina.maxStamina) ? 1f : 0.3f;

        /*
        if (playerStamina.currentStamina < playerStamina.maxStamina)
        {
            ShowBar();
            lastVisibleTime = Time.time;
        }
        else if (Time.time - lastVisibleTime > fadeOutDelay)
        {
            HideBar();
        }
        */
    }

    /*
    void ShowBar()
    {
        visible = true;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime * fadeOutSpeed);
    }

    void HideBar()
    {
        visible = false;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, Time.deltaTime * fadeOutSpeed);
    }
    */

}
