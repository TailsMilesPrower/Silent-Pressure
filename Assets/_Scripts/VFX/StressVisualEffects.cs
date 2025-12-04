using System.IO;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class StressVisualEffects : MonoBehaviour
{
    [Header("References")]
    public StressMeter stressMeter;
    public Volume postProcessVolume;
    //public Camera camera;

    [Header("General Settings")]
    public float vignetteTreshold = 60f;
    public float extremeStressTreshold = 90f;
    public float smoothSpeed = 5f;

    [Header("Chromatic Aberration Settings")]
    //public float normalIntensity = 0.1f;
    public float maxChromaticIntensity = 0.8f;
    //public float stressTreshold = 60f;

    [Header("Vignette")]
    public float normalVignette = 0.25f;
    public float maxVignette = 0.5f;
    public Color calmVignetteColor = new Color(0.12f, 0.1f, 0.15f);
    public Color stressedVignetteColor = new Color(0.25f, 0.05f, 0.05f);
    public float pulseSpeed = 2.5f;

    [Header("Color Adjustments")]
    public float minSaturation = -25f;
    public float maxSaturation = 25f;
    public Color stressTintColor = new Color(1f, 0.85f, 0.85f);
    public Color calmColorFilter = new Color(0.9f, 0.95f, 1f);

    [Header("Lens Distortion")]
    public float maxDistortion = 0.2f;

    [Header("Camera Shake")]
    public float shakeAmplitude = 1.5f;
    public float shakeFrequency = 2f;

    private ChromaticAberration chromaticAberration;
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;
    private LensDistortion lensDistortion;
    private CinemachineBasicMultiChannelPerlin cameraNoise;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (postProcessVolume == null)
        {
            Debug.LogError("StressVisualEffects: Missing Post-Process Volume reference!");
            enabled = false;
            return;
        }

        postProcessVolume.profile.TryGet(out chromaticAberration);
        postProcessVolume.profile.TryGet(out vignette);
        postProcessVolume.profile.TryGet(out colorAdjustments);
        postProcessVolume.profile.TryGet(out lensDistortion);

        if (chromaticAberration == null)
        {
            Debug.LogError("StressVisualEffects: No ChromaticAberration found in Volume profile!");
            enabled = false;
            return;
        }

        //if (camera != null)
        //{
            //cameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //cameraNoise = camera.;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (stressMeter == null) return;

        float stress = stressMeter.anxiety;
        float stress01 = stress / 100f;

        HandleChromaticAberration(stress, stress01);
        HandleVignette(stress, stress01);
        HandleColorAdjustments(stress, stress01);
        HandleLensDistortion(stress, stress01);
        HandleCameraShake(stress);

        /* //Old (only chromaticAberration)
        float targetIntensity = normalIntensity;

        if (stressMeter.anxiety >= stressTreshold)
        {
            float t = Mathf.InverseLerp(stressTreshold, 100f, stressMeter.anxiety);
            targetIntensity = Mathf.Lerp(normalIntensity, maxIntensity, t);
        }

        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, targetIntensity, Time.deltaTime * smoothSpeed);
        */
    }

    void HandleChromaticAberration(float stress, float stress01)
    {
        if (chromaticAberration == null) return;

        float target = stress >= vignetteTreshold ? Mathf.Lerp(0f, maxChromaticIntensity, Mathf.InverseLerp(vignetteTreshold, 100f, stress)) : 0f;

        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, target, Time.deltaTime * smoothSpeed);
    }

    void HandleVignette(float stress, float stress01)
    {
        if (vignette == null) return;

        float baseIntensity = Mathf.Lerp(normalVignette, maxVignette, Mathf.InverseLerp(vignetteTreshold, 100f, stress));
        Color baseColor = Color.Lerp(calmVignetteColor, stressedVignetteColor, Mathf.InverseLerp(vignetteTreshold, 100f, stress));

        if (stress >= extremeStressTreshold)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
            baseIntensity += pulse * 0.03f;
            baseColor = Color.Lerp(baseColor, new Color(0.4f, 0f, 0f), pulse * 0.3f);
        }

        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, baseIntensity, Time.deltaTime * smoothSpeed);
        vignette.color.value = Color.Lerp(vignette.color.value, baseColor, Time.deltaTime * smoothSpeed);

    }

    void HandleColorAdjustments(float stress, float stress01)
    {
        if (colorAdjustments == null) return;

        float targetSaturation = Mathf.Lerp(maxSaturation, minSaturation, Mathf.InverseLerp(0f, 100f, stress));
        colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, targetSaturation, Time.deltaTime * smoothSpeed);

        Color targetTint = Color.Lerp(calmColorFilter, stressTintColor, Mathf.InverseLerp(vignetteTreshold, 100f, stress));
        colorAdjustments.colorFilter.value = Color.Lerp(colorAdjustments.colorFilter.value, targetTint, Time.deltaTime * smoothSpeed);

    }

    void HandleLensDistortion(float stress, float stress01)
    {
        if (lensDistortion == null) return;

        float targetDistortion = Mathf.Lerp(0f, -maxDistortion, Mathf.InverseLerp(extremeStressTreshold, 100f, stress));
        lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, targetDistortion, Time.deltaTime * smoothSpeed);

    }

    void HandleCameraShake(float stress)
    {
        if (cameraNoise == null) return;

        if (stress >= vignetteTreshold)
        {
            float t = Mathf.InverseLerp(vignetteTreshold, 100f, stress);
            cameraNoise.AmplitudeGain = Mathf.Lerp(0f, shakeAmplitude, t);
            cameraNoise.FrequencyGain = Mathf.Lerp(0f, shakeFrequency, t);
        }
        else
        {
            cameraNoise.AmplitudeGain = Mathf.Lerp(cameraNoise.AmplitudeGain, 0f, Time.deltaTime * smoothSpeed);
            cameraNoise.FrequencyGain = Mathf.Lerp(cameraNoise.FrequencyGain, 0f, Time.deltaTime * smoothSpeed);
        }
    }
}
