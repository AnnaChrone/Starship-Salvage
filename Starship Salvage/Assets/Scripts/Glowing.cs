using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Glowing : MonoBehaviour
{
    
    public Light orbLight;
    public float pulseSpeed = 2f;
    public float minIntensity = 1f;
    public float maxIntensity = 3f;

    void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        orbLight.intensity = intensity;
    }
}

