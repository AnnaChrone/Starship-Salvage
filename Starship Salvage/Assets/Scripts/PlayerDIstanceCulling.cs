using UnityEngine;

public class PlayerDistanceCulling : MonoBehaviour
{
    public Transform player;     
    public float cullDistance = 200f; // Distance at which the object disappears

    private Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        bool shouldBeVisible = distance < cullDistance;

        foreach (Renderer r in renderers)
        {
            r.enabled = shouldBeVisible;
        }
    }
}
