using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xasu.HighLevel;

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
    public float stressReductionShort = 2f;
    public float stressReductionLong = 4f;
    public float shortListeningTime = 15f;
    public float longListeningTime = 30f;

    [Header("Quick Music Setting")]
    private KeyCode quickMusicKey = KeyCode.M;

    [Header("Now Playing UI")]
    public CanvasGroup nowPlayingGroup;
    public TMP_Text nowPlayingText;
    public Image songProgressFill;
    public float uiSlideTime = 0.4f;
    public float uiVisibleTime = 2f;

    private Coroutine shortListeningRoutine;
    private Coroutine longListeningRoutine;
    private Coroutine nowPlayingRoutine;
    private Coroutine progressRoutine;

    private AudioClip[] playlist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playlist = new AudioClip[] {song1, song2, song3};

        if (nowPlayingGroup)
        {
            nowPlayingGroup.alpha = 0f;
            //nowPlayingGroup.transform.localPosition = new Vector3(0f, -70f, 0f);
        }

        if (songProgressFill) songProgressFill.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(quickMusicKey))
        {
            PlayRandomSong();
        }
    }

    public void PlaySong(int id)
    {
        AudioClip selected = GetClipByID(id);
        if (selected == null) return;

        StartMusic(selected, isQuick: false);
    }

    private AudioClip GetClipByID(int id)
    {
        if (id == 1) return song1;
        if (id == 2) return song2;
        if (id == 3) return song3;
        return null;
    }

    public void PlayRandomSong()
    {
        AudioClip clip = playlist[Random.Range(0, playlist.Length)];
        StartMusic(clip, isQuick: true);
    }

    public void StartMusic(AudioClip clip, bool isQuick)
    {
        if (clip == null || audioSource == null) return;
        
        audioSource.clip = clip;
        audioSource.Play();

        CompletableTracker.Instance.Initialized("listening to mp3");

        if (songProgressFill)
        {
            //songProgressFill.fillAmount = 0f;

            if (progressRoutine != null) StopCoroutine(progressRoutine);

            progressRoutine = StartCoroutine(SongProgressTimer(clip.length));
        }

        if (shortListeningRoutine != null) StopCoroutine(shortListeningRoutine);
        if (longListeningRoutine != null) StopCoroutine(longListeningRoutine);

        shortListeningRoutine = StartCoroutine(ShortListenTimer());

        if (isQuick) longListeningRoutine = StartCoroutine(LongListenTimer());

        ShowNowPlaying(clip.name);

        /*
        if (audioSource == null || stressMeter == null)
        {
            Debug.LogError("Music or Stress Meter reference missing!");
            return;
        }
        
        if (selected == null)
        {
            Debug.LogWarning("Selected song is missing!");
            return;
        }
        */
    }

    private IEnumerator ShortListenTimer()
    {
        float timer = 0f;

        while (timer < shortListeningTime)
        {
            if(!audioSource.isPlaying) yield break;
            
            timer += Time.deltaTime;
            yield return null;
        }

        stressMeter.anxiety -= stressReductionShort;
        stressMeter.anxiety = Mathf.Clamp(stressMeter.anxiety, 0f, 100f);

        stressMeter.calming = true;
        yield return new WaitForSeconds(1f);
        stressMeter.calming = false;
    }

    private IEnumerator LongListenTimer()
    {
        float timer = 0f;

        while (timer < longListeningTime)
        {
            if (!audioSource.isPlaying) yield break;

            timer += Time.deltaTime;
            yield return null;
        }

        stressMeter.anxiety -= stressReductionLong;
        stressMeter.anxiety = Mathf.Clamp(stressMeter.anxiety, 0f, 100f);

        stressMeter.calming = true;
        yield return new WaitForSeconds(1f);
        stressMeter.calming = false;
    }

    private IEnumerator SongProgressTimer(float songLength)
    {
        float t = 0f;

        if (songProgressFill) songProgressFill.fillAmount = 0f;

        while (audioSource.isPlaying && t < songLength)
        {
            t += Time.deltaTime;
            
            if (songProgressFill) songProgressFill.fillAmount = Mathf.Clamp01(t / songLength);

            yield return null;
        }

        if (songProgressFill) songProgressFill.fillAmount = 1f;

        if (nowPlayingRoutine != null) StopCoroutine(nowPlayingRoutine);
        nowPlayingRoutine = StartCoroutine(FadeOutNowPlaying());
    }

    private void ShowNowPlaying(string text)
    {
        if (nowPlayingGroup == null || nowPlayingText == null) return;

        nowPlayingText.text = "Now Playing: " + text;
        
        if (nowPlayingRoutine != null) StopCoroutine(nowPlayingRoutine);

        nowPlayingRoutine = StartCoroutine(AnimatorNowPlaying());
    }

    private IEnumerator AnimatorNowPlaying()
    {
        float t = 0f;

        //Vector3 hiddenPos = new Vector3(0f, -70f, 0f);
        //Vector3 visiblePos = new Vector3(0f, -10f, 0f);

        while (t < uiSlideTime)
        {
            t += Time.deltaTime;
            float p = t / uiSlideTime;
            nowPlayingGroup.alpha = Mathf.Lerp(0f, 1f, p);
            //nowPlayingGroup.transform.localPosition = Vector3.Lerp(hiddenPos, visiblePos, p);
            yield return null;
        }

        nowPlayingGroup.alpha = 1f;
        //yield return new WaitForSeconds(uiVisibleTime);
    }

    private IEnumerator FadeOutNowPlaying()
    {
        float t = 0f;
        while (t < uiSlideTime)
        {
            t += Time.deltaTime;
            float p = t / uiSlideTime;
            nowPlayingGroup.alpha = Mathf.Lerp(1f, 0f, p);
            //nowPlayingGroup.transform.localPosition = Vector3.Lerp(visiblePos, hiddenPos, p);
            yield return null;
        }

        nowPlayingGroup.alpha = 0f;
        //nowPlayingGroup.transform.localPosition = hiddenPos;
    }

    public bool IsSongPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }
}
