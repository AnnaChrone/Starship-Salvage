using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISlide : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform menuPanel;
    public Button close;

    [Header("Slide Settings")]
    public float slideSpeed = 500f; 
    public float hiddenY = 400f;      // offscreen above the screen
    public float visibleY = 0f;       // fully visible position

    private bool isOpen = false;
    private Coroutine slideRoutine;

    private void Start()
    {
        // Start hidden above the screen
        Vector2 pos = menuPanel.anchoredPosition;
        pos.y = hiddenY;
        menuPanel.anchoredPosition = pos;

        // Optional toggle button hookup
        if (close != null)
            close.onClick.AddListener(ToggleMenu);
    }

    public void ToggleMenu()
    {
        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        isOpen = !isOpen;
        slideRoutine = StartCoroutine(Slide(isOpen ? visibleY : hiddenY));
    }

    private IEnumerator Slide(float targetY)
    {
        Vector2 pos = menuPanel.anchoredPosition;

        while (Mathf.Abs(pos.y - targetY) > 0.1f)
        {
            pos.y = Mathf.MoveTowards(pos.y, targetY, slideSpeed * Time.deltaTime);
            menuPanel.anchoredPosition = pos;
            yield return null;
        }

        pos.y = targetY;
        menuPanel.anchoredPosition = pos;
        slideRoutine = null;
    }
}
