using System.Collections;
using UnityEngine;

public class SpaceshipFixing : MonoBehaviour
{

    [SerializeField] private string requiredItemID;
    public GameObject successText;

    public bool TryUseItem()
    {
        var hotbar = FindFirstObjectByType<Hotbar>(); //Finds hotbar in the scene
        if (hotbar.RemoveItemByID(requiredItemID))
        {
            Debug.Log("Correct item used on ship! Item removed from hotbar.");
            ShowText();
            return true;
        }
        else
        {
            Debug.Log("You don't have the right item.");
            return false;
        }
    }

    public void ShowText()
    {
        if (successText != null)
        {
            successText.SetActive(true);
            StartCoroutine(HideTextAfterDelay(2f));
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (successText != null)
            successText.SetActive(false);
    }
}
