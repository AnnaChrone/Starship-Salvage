using UnityEngine;
using UnityEngine.EventSystems;
public class FlowerSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
        GameObject dropped = eventData.pointerDrag;
        draggableItem draggableItem = dropped.GetComponent<draggableItem>();
        draggableItem.parentAfterDrag = transform;
        }
    }
}
