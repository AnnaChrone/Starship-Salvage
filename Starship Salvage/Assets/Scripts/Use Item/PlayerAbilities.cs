using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool FloatAquired = false;
    public bool GlowAquired = false;
    public bool GrowAquired = false;
    public GameObject GlowEffect;

    public void UnlockAbility(string abilityName)
    {
        switch (abilityName)
        {
            case "Float":
                FloatAquired = true;
                Debug.Log("Unlocked Float!");
                break;
            case "Glow":
                GlowAquired = true;
                Debug.Log("Unlocked Glow!");
                GlowEffect.SetActive(true);
                break;
            case "Grow":
                GrowAquired = true;
                Debug.Log("Unlocked Grow!");
                break;
            default:
                break;
        }
    }
}