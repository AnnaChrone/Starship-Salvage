using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public RectTransform pointer;      // Assign your UI Image
    public float speed = 1000f;
    private Vector2 input;

    void Update()
    {
        // Read input from FPController or Input System
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input.sqrMagnitude < 0.01f) return;

        // Move pointer
        Vector2 pos = pointer.anchoredPosition;
        pos += input * speed * Time.deltaTime;

        // Clamp to screen
        Vector2 canvasSize = ((RectTransform)pointer.parent).sizeDelta;
        pos.x = Mathf.Clamp(pos.x, 0, canvasSize.x);
        pos.y = Mathf.Clamp(pos.y, 0, canvasSize.y);

        pointer.anchoredPosition = pos;
    }

    // Simulate click
    public void Click()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = pointer.position; // screen space

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var r in results)
        {
            ExecuteEvents.Execute(r.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
        }
    }
}