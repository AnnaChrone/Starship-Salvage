using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [Header("Item Data")]
    public GameObject itemPrefab; // Assign prefab (usually self) in inspector
    public Sprite icon;           // Assign icon sprite in inspector

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    public void PickUp(Transform holdPoint)
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void Drop()
    {
        rb.useGravity = true;
        transform.SetParent(null);
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        rb.MovePosition(targetPosition);
    }
}
