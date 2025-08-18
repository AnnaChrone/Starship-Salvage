using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [Header("Slot assignment")]
    public GameObject[] slots = new GameObject[7];
    public Image[] slotIcons = new Image[7];
    public Transform holdPoint;  // Assign this from FPController or scene
    private GameObject[] heldItemInstances = new GameObject[7]; // one per slot



    private int previousIndex = -1;


    private int index = 0;
    public int CurrentIndex
    {
        get => index;
        set
        {
            if (value >= 0 && value < slots.Length)
            {
                index = value;
                UpdateSelection();
            }
        }
    }


    private float scrollAccumulator = 0f; // To fix multiple slot skips
    private float lastScrollTime = 0f;
    private float scrollDebounceTime = 0.2f;

    private GameObject[] storedItemPrefabs = new GameObject[7]; // Prefab references per slot
    private GameObject currentHeldItem = null;

    private void Awake()
    {
        previousIndex = 0;
        index = 0;

        UpdateHeldItemPosition();

        // Update highlights and icons
        UpdateHighlightsItems();
    }
    public void StoreItemInSlot(int slotIndex, GameObject itemPrefab)
    {
        storedItemPrefabs[slotIndex] = itemPrefab;

        if (itemPrefab != null && slotIcons[slotIndex] != null)
        {
            PickUpObject item = itemPrefab.GetComponent<PickUpObject>();
            if (item != null)
            {
                slotIcons[slotIndex].sprite = item.icon;
                slotIcons[slotIndex].enabled = true;
            }
        }
        else if (slotIcons[slotIndex] != null)
        {
            slotIcons[slotIndex].enabled = false;
        }
    }




    // Finds the first child with tag "Pickup" inside the slot GameObject
    private Transform FindPickupInSlot(GameObject slot)
    {
        foreach (Transform child in slot.transform)
        {
            if (child.CompareTag("Pickup"))
                return child;
        }
        return null;
    }

    public bool hasItem(string questID) //This checks if the item in the hot bar shares the same id as the quesy
    {
        for (int i = 0; i < heldItemInstances.Length; i++)
        {
            GameObject heldItem = heldItemInstances[i];
            if (heldItem != null)
            {
                PickUpObject item = heldItem.GetComponent<PickUpObject>();
                if (item != null && item.itemID == questID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void HandleScroll(float scrollValue)
    {
        float currentTime = Time.time;
        if (currentTime - lastScrollTime < scrollDebounceTime)
            return;  // Ignore too-close events

        scrollAccumulator += scrollValue;

        while (scrollAccumulator >= 1f)
        {
            index--;
            if (index < 0) index = slots.Length - 1;
            scrollAccumulator -= 1f;
            UpdateHighlightsItems();
            lastScrollTime = currentTime;
        }

        while (scrollAccumulator <= -1f)
        {
            index++;
            if (index >= slots.Length) index = 0;
            scrollAccumulator += 1f;
            UpdateHighlightsItems();
            lastScrollTime = currentTime;
        }
    }

    private void UpdateHeldItemPosition()
    {
        for (int i = 0; i < heldItemInstances.Length; i++)
        {
            if (heldItemInstances[i] != null)
                heldItemInstances[i].SetActive(i == index);
        }
        previousIndex = index;
    }





    private void UpdateHighlightsItems()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Transform highlight = slots[i].transform.Find("Highlight");
            if (highlight != null)
                highlight.gameObject.SetActive(i == index);
        }

        UpdateHeldItemPosition();
    }

    public void UpdateSelection()
    {
        UpdateHighlightsItems();
    }

    public int FindFirstFreeSlot()
    {
        for (int i = 0; i < heldItemInstances.Length; i++)
        {
            if (heldItemInstances[i] == null)
                return i;
        }
        return -1; // no free slots
    }

    public void SetHeldItemInstance(int slotIndex, GameObject instance)
    {
        if (slotIndex >= 0 && slotIndex < heldItemInstances.Length)
        {
            if (heldItemInstances[slotIndex] != null)
                Destroy(heldItemInstances[slotIndex]);

            heldItemInstances[slotIndex] = instance;
            instance.transform.SetParent(holdPoint);
            instance.transform.localPosition = Vector3.zero;


            // Update selection so that if this is current index, it becomes active
            if (slotIndex == index)
                UpdateHeldItemPosition();
            else
                instance.SetActive(false);
        }
    }


    public void DropCurrentItem()
    {
        int slotIndex = index;
        GameObject heldItem = heldItemInstances[slotIndex];

        if (heldItem == null) return;

        PickUpObject pickUpScript = heldItem.GetComponent<PickUpObject>();
        if (pickUpScript != null)
            pickUpScript.Drop();

        // Remove from hotbar
        heldItemInstances[slotIndex] = null;
        storedItemPrefabs[slotIndex] = null;

        // Clear UI
        if (slotIcons[slotIndex] != null)
        {
            slotIcons[slotIndex].sprite = null;
            slotIcons[slotIndex].enabled = false;
        }

        UpdateSelection();
    }

    public int FindItemSlot(string itemID)
    {
        for (int i = 0; i < heldItemInstances.Length; i++)
        {
            GameObject heldItem = heldItemInstances[i];
            if (heldItem != null)
            {
                PickUpObject item = heldItem.GetComponent<PickUpObject>();
                if (item != null && item.itemID == itemID)
                {
                    return i; // found it
                }
            }
        }
        return -1; // not found
    }

    public void RemoveItemAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= heldItemInstances.Length)
            return;

        GameObject heldItem = heldItemInstances[slotIndex];
        if (heldItem == null) return;

        // Destroy the item object in hand
        Destroy(heldItem);

        // Clear hotbar references
        heldItemInstances[slotIndex] = null;
        storedItemPrefabs[slotIndex] = null;

        // Clear UI
        if (slotIcons[slotIndex] != null)
        {
            slotIcons[slotIndex].sprite = null;
            slotIcons[slotIndex].enabled = false;
        }

        // Update selection in case we removed the current slot
        UpdateSelection();
    }


    public bool RemoveItemByID(string itemID)
    {
        int slot = FindItemSlot(itemID);
        if (slot != -1)
        {
            // Remove the item from the slot
            if (heldItemInstances[slot] != null)
                Destroy(heldItemInstances[slot]);
            heldItemInstances[slot] = null;
            storedItemPrefabs[slot] = null;

            if (slotIcons[slot] != null)
            {
                slotIcons[slot].sprite = null;
                slotIcons[slot].enabled = false;
            }

            UpdateSelection();
            return true; // successfully removed
        }
        return false; // item not found
    }


}