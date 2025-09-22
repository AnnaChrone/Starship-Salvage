using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShipParts : MonoBehaviour, IUsable
{
    public bool shipPart = true;
    public string Name;
    public int repairCount = 0;
    public GameObject successText;
    public void Use(GameObject user)
    {

        if (shipPart)
        {
            var hotbar = FindFirstObjectByType<Hotbar>(); //Finds hotbar in the scene
            repairCount++;
            if (hotbar.RemoveItemByID(Name))
            {
                Debug.Log("Correct item used on ship! Item removed from hotbar.");
                ShowText();

            }
            else
            {
                Debug.Log("You don't have the right item.");

            }
            Destroy(gameObject);
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