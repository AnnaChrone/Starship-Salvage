using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    [Header("Slides")]
    public Sprite[] slides; // Assign your 4 sprites here

    [Header("Settings")]
    public float slideDuration = 5f; // seconds per slide
    public float fadeDuration = 1.5f; // seconds for final fade-out

    [Header("Audio")]
    public AudioClip[] audioClips; // 1 audio clip per slide
    public AudioSource CutsceneAudio;

    [Header("UI Reference")]
    public Image Cutscene; // UI Image displaying the sprites

    [Header("State setting")]
    public GameObject MinLuCollider;

    private void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Play each slide with its matching audio
        for (int i = 0; i < slides.Length; i++)
        {
            Cutscene.sprite = slides[i];

            // Play corresponding audio 
            if (i < audioClips.Length && audioClips[i] != null)
            {
                CutsceneAudio.clip = audioClips[i];
                CutsceneAudio.Play();
            }

            yield return new WaitForSeconds(slideDuration);
        }

        // Fade out visuals and audio
        yield return StartCoroutine(FadeOut());
        Debug.Log("Cutscene finished");
        // trigger Zorb or next scene here
        MinLuCollider.SetActive(true);
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color originalColor = Cutscene.color;
        float originalVolume = CutsceneAudio.volume;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            // Fade alpha
            Cutscene.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                Mathf.Lerp(originalColor.a, 0f, t)
            );

            // Fade audio
            CutsceneAudio.volume = Mathf.Lerp(originalVolume, 0f, t);

            yield return null;
        }

        // Ensure completely faded
        Cutscene.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        CutsceneAudio.volume = originalVolume; // reset volume so next time plays normally
        CutsceneAudio.Stop();
    }
}
