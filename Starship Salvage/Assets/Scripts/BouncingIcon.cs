using UnityEngine;

public class BouncingIcon : MonoBehaviour
{
    public float bounceHeight = 0.2f;
    public float bounceSpeed = 3f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = startPos + Vector3.up * newY;
        transform.LookAt(Camera.main.transform); 
    }
}

