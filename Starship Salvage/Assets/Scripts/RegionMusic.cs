using UnityEngine;
using System.Collections;

public class RegionMusic : MonoBehaviour
{
    [Header("Region Music Sources")]
    public AudioSource RaLuMusic;
    public AudioSource CoLuMusic;
    public AudioSource LuLuMusic;
    public AudioSource MinLuMusic;

    private AudioSource currentMusic;
    private bool isFading = false;
    private float fadeDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (isFading) return; // prevent spam

        if (other.CompareTag("RaLu"))
        {
            Debug.Log("RaLu");
            StartCoroutine(SwitchMusic(RaLuMusic));
        }
        else if (other.CompareTag("CoLu"))
        {
            Debug.Log("CoLu");
            StartCoroutine(SwitchMusic(CoLuMusic));
        }
        else if (other.CompareTag("LuLu"))
        {
            Debug.Log("LuLu");
            StartCoroutine(SwitchMusic(LuLuMusic));
        }
        else if (other.CompareTag("MinLu"))
        {
            Debug.Log("MinLu");
            StartCoroutine(SwitchMusic(MinLuMusic));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RaLu"))
        {
            Debug.Log("RaLu exit");
            if (currentMusic != null)
                StartCoroutine(FadeOutMusic(currentMusic));
            currentMusic = null;
        }
        else if (other.CompareTag("CoLu"))
        {
            Debug.Log("CoLu exit");
            if(currentMusic != null)
                StartCoroutine(FadeOutMusic(currentMusic));
            currentMusic = null;
        }
        else if (other.CompareTag("LuLu"))
        {
            Debug.Log("LuLu exit");
            if (currentMusic != null)
                StartCoroutine(FadeOutMusic(currentMusic));
            currentMusic = null;
        }
        else if (other.CompareTag("MinLu"))
        {
            Debug.Log("MinLu exit");
            
            if (currentMusic != null)
                StartCoroutine(FadeOutMusic(currentMusic));
            currentMusic = null;
        }
    }

    private IEnumerator SwitchMusic(AudioSource newMusic)
    {
        if (newMusic == currentMusic)
            yield break; 

        isFading = true;

        AudioSource oldMusic = currentMusic;
        currentMusic = newMusic;

        if (newMusic != null)
        {
            newMusic.volume = 0f;
            newMusic.Play();
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            if (oldMusic != null)
                oldMusic.volume = Mathf.Lerp(1f, 0f, t);

            if (newMusic != null)
                newMusic.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // resetting old and new
        if (oldMusic != null)
        {
            oldMusic.Stop();
            oldMusic.volume = 0f;
        }

        if (newMusic != null)
            newMusic.volume = 1f;

        isFading = false;
    }

    private IEnumerator FadeOutMusic(AudioSource audio)
    {
        if (audio == null) yield break;
        isFading = true;

        float startVolume = audio.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audio.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audio.volume = 0f;
        audio.Stop();
        isFading = false;
    }
}
