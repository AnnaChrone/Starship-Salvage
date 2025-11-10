using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [Header("Item Data")]
    public string itemID; 
    public GameObject itemPrefab; 
    public Sprite icon;

    [Header("Audios")]
    public AudioSource Collect;
    public AudioSource DropAudio;

    [Header("Consumable / Special")]
    public bool consumable = false; 
    public string unlockAbilityName; 


    private Rigidbody rb;
    private bool isHeld = false;
    private Collider col; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>(); // get the collider
    }

    public void PickUp(Transform holdPoint)
    {
        isHeld = true;
        Collect.Play();
        rb.useGravity = false;
        rb.isKinematic = true;

        // Disable the collider while held
        col.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;

    }

    public void Drop()
    {
        DropAudio.Play();
        isHeld = false;
        transform.SetParent(null, true);
        rb.useGravity = true;
        rb.isKinematic = false;

        // Re-enable the collider when dropped
        col.enabled = true;
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        if (isHeld)
        {
            rb.MovePosition(targetPosition);
        }
    }

    public void Use(GameObject user)
    {
        if (consumable)
        {
            if (!string.IsNullOrEmpty(unlockAbilityName))
            {
                var abilities = user.GetComponent<PlayerAbilities>();
                abilities.UnlockAbility(unlockAbilityName);
            }

            Destroy(gameObject);
            return;
        }

        var spaceship = FindFirstObjectByType<SpaceshipPartItem>();

        if (spaceship != null && itemID == spaceship.ShipPartID)
        {
            spaceship.ShowText(itemID);
        }
            
    }
}
