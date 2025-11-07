using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public bool FloatAquired = false;
    public bool GlowAquired = false;
    public bool GrowAquired = false;
    public bool CommAquired = false;
    public Image Controls;
    public Sprite GrowFloat;
    public Sprite Grow;
    public Sprite Float;
    public GameObject GlowEffect;

    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float HoldDuration = 3f;

    void Awake()
    {
        canvasGroup.alpha = 0;
    }
    public void UnlockAbility(string abilityName)
    {
        switch (abilityName)
        {
            case "Float":
                FloatAquired = true;
                Debug.Log("Unlocked Float!");
                StartCoroutine(FadeIn());
                break;
            case "Glow":
                GlowAquired = true;
                Debug.Log("Unlocked Glow!");
                GlowEffect.SetActive(true);
                break;
            case "Grow":
                GrowAquired = true;
                Debug.Log("Unlocked Grow!");
                StartCoroutine(FadeIn());
                break;
            case "Comm":
                CommAquired = true;
                Debug.Log("Unlocked Comm!");
                break;
            default:
                break;
        }

        if (FloatAquired && !GrowAquired)
        {
            Controls.sprite = Float;
            Debug.Log("Float Image");

        }
        else if (FloatAquired && GrowAquired)
        {
            Controls.sprite = GrowFloat;
            Debug.Log("GrowFLoat image");

        } else if (!FloatAquired && GrowAquired)
        {
            Controls.sprite = Grow;
            Debug.Log("Grow image");

        }


    }

    public IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        
        StartCoroutine(Wait());
    }

    public IEnumerator Wait()
    {
        float elapsed = 0f;
        while (elapsed < HoldDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FadeOut());
    }
    public IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}