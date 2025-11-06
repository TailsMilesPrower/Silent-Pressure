using UnityEngine;

[ExecuteAlways]
public class StressFogController : MonoBehaviour
{
    [Header("References")]
    public StressMeter stressMeter;

    [Header("Fog Activation Thresholds")]
    public float startStressThreshold = 60f;
    public float fullStressThreshold = 90f;

    [Header("Fog Settings")]
    public bool useExp2Fog = true;
    public Color calmFogColor = new Color(0.75f, 0.8f, 0.85f, 1f);
    public Color stressFogColor = new Color(0.25f, 0.05f, 0.05f, 1f);
    public float calmFogDensity = 0.0025f;
    public float maxFogDensity = 0.03f;

    [Header("Distance Fog")]
    public float calmFogStart = 0f;
    public float calmFogEnd = 300f;
    public float stressFogStart = 5f;
    public float stressFogEnd = 75f;

    [Header("Transition")]
    public float smoothSpeed = 1.5f;

    private bool initialized = false;
    private float targetFogDensity;
    private Color targetFogColor;
    private float targetFogStart;
    private float targetFogEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeFog();
    }

    void InitializeFog()
    {
        if (useExp2Fog)
        {
            RenderSettings.fogMode = FogMode.ExponentialSquared;
        }
        else
        {
            RenderSettings.fogMode = FogMode.Linear;
        }

        RenderSettings.fog = true;
        RenderSettings.fogDensity = calmFogDensity;
        RenderSettings.fogColor = calmFogColor;
        RenderSettings.fogStartDistance = calmFogStart;
        RenderSettings .fogEndDistance = calmFogEnd;
        initialized = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) InitializeFog();
        if (stressMeter == null) return;

        float stress = stressMeter.anxiety;
        float t = Mathf.InverseLerp(startStressThreshold, fullStressThreshold, stress);
        t = Mathf.Clamp01(t);

        targetFogDensity = Mathf.Lerp(calmFogDensity, maxFogDensity, t);
        targetFogColor = Color.Lerp(calmFogColor, stressFogColor, t);
        targetFogStart = Mathf.Lerp(calmFogStart, stressFogStart, t);
        targetFogEnd = Mathf.Lerp(calmFogEnd, stressFogEnd, t);

        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, Time.deltaTime * smoothSpeed);
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, Time.deltaTime * smoothSpeed);
        RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, targetFogStart, Time.deltaTime * smoothSpeed);
        RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, targetFogEnd, Time.deltaTime * smoothSpeed);

        /* //Attempt to make it more dynamic (make it pulse) too aggressive
        if (stress >= fullStressThreshold - 5f)
        {
            float pulse = (Mathf.Sin(Time.time * 1.5f) + 1f) * 0.5f;
            RenderSettings.fogDensity += pulse * 0.005f;
        }
        */

    }
}
