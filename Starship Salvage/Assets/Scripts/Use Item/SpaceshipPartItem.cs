using System.Collections;
using UnityEngine;

public class SpaceshipPartItem : MonoBehaviour
{
    [Header("Item Settings")]
     public string ShipPartID;                 // the ID of this part
     private GameObject successText;        // UI feedback
     private float textDisplayTime = 2f;

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(textDisplayTime);
        if (successText != null)
            successText.SetActive(false);
    }
    public void ShowText(string itemID)
    {
        
            Debug.Log($"Used spaceship part: {ShipPartID} successfully!");

            // Show UI feedback
            if (successText != null)
            {
                successText.SetActive(true);
                // Start coroutine to hide it after a delay
                StartCoroutine(HideTextAfterDelay());
            }
        }
    }
