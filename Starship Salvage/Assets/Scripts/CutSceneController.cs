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
    public AudioSource Rumbling;

    [Header("UI Reference")]
    public Image Cutscene; // UI Image displaying the sprites

    [Header("State setting")]
    public GameObject MinLuCollider;
    public bool Intro;
    public NPC Zorb;

    private void Start()
    {
        StartCoroutine(PlayCutscene());
        Intro = true;
    }

    private IEnumerator PlayCutscene()
    {
        for (int i = 0; i < slides.Length; i++)
        {
            Cutscene.sprite = slides[i];

            // Play corresponding audio 
            if (i < audioClips.Length && audioClips[i] != null)
            {
                CutsceneAudio.clip = audioClips[i];
                CutsceneAudio.Play();
            }

            // Make slide 3 last 3 seconds, others 2 seconds
            float currentDuration = (i == 2) ? 3f : 2f;  // index 2 = 3rd slide
            yield return new WaitForSeconds(currentDuration);
        }

        // Fade out visuals and audio
        yield return StartCoroutine(FadeOut());
        Debug.Log("Cutscene finished");
        Intro = false;
        MinLuCollider.SetActive(true);
        Zorb.StartDialogue();
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
            Rumbling.volume = Mathf.Lerp(originalVolume, 0f, t);
            CutsceneAudio.volume = Mathf.Lerp(originalVolume, 0f, t);

            yield return null;
        }

        // Ensure completely faded
        Cutscene.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        CutsceneAudio.volume = originalVolume; // reset volume so next time plays normally
        CutsceneAudio.Stop();
    }
}
