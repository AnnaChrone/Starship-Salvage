using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.XR;

public class ShipRepairs : MonoBehaviour
{
    public int repairCount = 0;
    public GameObject ship;
    public TMP_Text successText;
    public TMP_Text dayCount;
    public TMP_Text bigDay;
    public AudioSource Nextday;
    public GameObject TEMP;
    public Objectives objective;

    [Header("Ship Fix Materials")]
    public Material Fix1;
    public Material Fix2;
    public Material Fix3;

    [Header("NPCs Day 1")]
    public GameObject NPCZorb;
    public GameObject NPCChoo;
    public GameObject NPCRakoo;
    public GameObject NPCLucoo;
    [Header("NPCs Day 2")]
    public GameObject NPCZinnia;
    public GameObject NPCLuLu;
    public GameObject NPCRaLu;
    public GameObject NPCCoLu;
    public GameObject NPCMinLu;
    [Header("NPCs Day 3")]
    public GameObject NPCCook;
    public GameObject NPCChef;

    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float HoldDuration = 3f;

    void Awake()
    {
        successText.SetText("");
        canvasGroup.alpha = 0;
    }

    public void Fix()
    {
        if (successText != null)
        {
            repairCount++;
            Nextday.Play();
            successText.SetText("Repair " + repairCount + "/3 completed" );
            if (repairCount == 1)
            {
                NPCZinnia.SetActive(true);
                NPCZorb.SetActive(false);
                NPCChoo.SetActive(false);
                NPCLucoo.SetActive(false);
                NPCRakoo.SetActive(false);
                objective.GetObjective("ZINNIA");
                GetComponent<Renderer>().material = Fix1;

            }
            else if (repairCount == 2)
            {
                NPCChef.SetActive(true);
                NPCCook.SetActive(true);
                NPCZinnia.SetActive(false);
                NPCMinLu.SetActive(false);
                NPCCoLu.SetActive(false);
                NPCLuLu.SetActive(false);
                NPCRaLu.SetActive(false);
                objective.GetObjective("RAMI");
                GetComponent<Renderer>().material = Fix2;
            }
            else if (repairCount == 3)
            {
                objective.GetObjective("ZORB");
                GetComponent<Renderer>().material = Fix3;
            }
            dayCount.SetText("DAY " + (repairCount + 1));
            StartCoroutine(FadeIn());

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
        successText.SetText("Repair " + repairCount + "/3 completed");
        dayCount.SetText("DAY " + (repairCount + 1));
        bigDay.SetText("DAY " + (repairCount + 1));
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

