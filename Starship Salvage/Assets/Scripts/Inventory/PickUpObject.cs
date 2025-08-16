using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [Header("Item Data")]
    public string itemID; //unique value that coincides with questID
    public GameObject itemPrefab; 
    public Sprite icon;           

    private Rigidbody rb;
    private bool isHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    public void PickUp(Transform holdPoint)
    {
        isHeld = true;
        rb.useGravity = false;
        rb.isKinematic = true; 
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;

    }

    public void Drop()
    {
        isHeld = false;
        transform.SetParent(null, true);
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        if (isHeld)
        {
            rb.MovePosition(targetPosition);
        }
    }
}
