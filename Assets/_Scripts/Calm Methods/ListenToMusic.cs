using System.Collections;
using UnityEngine;

public class ListenToMusic : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song3;

    [Header("Stress System Reference")]
    public StressMeter stressMeter;

    [Header("Music Settings")]
    public float stressReductionAmount = 2f;
    public float requiredListeningTime = 15f;

    private Coroutine listeningRoutine;
    
    public void PlaySong(int id)
    {
        if (audioSource == null || stressMeter == null)
        {
            Debug.LogError("Music or Stress Meter reference missing!");
            return;
        }

        AudioClip selected = null;
        if (id == 1) selected = song1;
        if (id == 2) selected = song2;
        if (id == 3) selected = song3;

        if (selected == null)
        {
            Debug.LogWarning("Selected song is missing!");
            return;
        }

        audioSource.clip = selected;
        audioSource.Play();

        if (listeningRoutine != null) StopCoroutine(listeningRoutine);

        listeningRoutine = StartCoroutine(ListenTimer());
    }

    private IEnumerator ListenTimer()
    {
        float timer = 0f;

        while (timer < requiredListeningTime)
        {
            if(!audioSource.isPlaying) yield break;
            
            timer += Time.deltaTime;
            yield return null;
        }

        stressMeter.anxiety -= stressReductionAmount;
        stressMeter.anxiety = Mathf.Clamp(stressMeter.anxiety, 0f, 100f);

        stressMeter.calming = true;
        yield return new WaitForSeconds(1f);
        stressMeter.calming = false;
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
