using UnityEngine;

public class SpaceshipFixing : MonoBehaviour
{

    [SerializeField] private string requiredItemID;

    public bool TryUseItem()
    {
        var hotbar = FindFirstObjectByType<Hotbar>(); // updated API
        if (hotbar.RemoveItemByID(requiredItemID))
        {
            Debug.Log("Correct item used on ship! Item removed from hotbar.");
            // Add repair/quest logic here
            return true;
        }
        else
        {
            Debug.Log("You don't have the right item.");
            return false;
        }
    }
}
