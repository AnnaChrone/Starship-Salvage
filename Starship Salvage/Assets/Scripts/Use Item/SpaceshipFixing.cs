using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceshipFixing : MonoBehaviour
{

    [SerializeField] private string requiredItemID;
    public GameObject successText;
    public bool inRange = false;


    private void OnTriggerEnter(Collider collide)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider collide)
    {
        inRange = false;
    }


    public void ApplyFix()
    {
        ShowText();
        // any other logic for fixing
    }

    public bool TryUseItem()
    {
        var hotbar = FindFirstObjectByType<Hotbar>();
        if (hotbar != null && inRange)
        {
            // Try to use only the currently held item
            if (hotbar.TryUseSelectedItem(gameObject))
            {
                Debug.Log("Correct item used on ship! Item removed from hand.");
                ShowText();
                return true;
            }
            else
            {
                Debug.Log("You don't have the right item in your hand.");
                return false;
            }
        }

        Debug.LogWarning("No Hotbar found in scene!");
        return false;
    }

    public void ShowText()
    {
        if (successText != null)
        {
            successText.SetActive(true);
            StartCoroutine(HideTextAfterDelay(2f));
        }
        else
        {
            Debug.Log("NO TEXTB");
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (successText != null)
            successText.SetActive(false);
    }
}
