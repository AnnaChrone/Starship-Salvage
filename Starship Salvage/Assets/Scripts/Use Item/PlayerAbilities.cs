using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public bool FloatAquired = false;
    public bool GlowAquired = false;
    public bool GrowAquired = false;
    public bool CommAquired = false;
    public Image Controls;
    public Sprite GrowFloat;
    public Sprite Grow;
    public Sprite Float;
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
            case "Comm":
                CommAquired = true;
                Debug.Log("Unlocked Comm!");
                break;
            default:
                break;
        }

        if (FloatAquired && !GrowAquired)
        {
            Controls.sprite = Float;
            Debug.Log("Float Image");
        }
        else if (FloatAquired && GrowAquired)
        {
            Controls.sprite = GrowFloat;
            Debug.Log("GrowFLoat image");
        } else if (!FloatAquired && GrowAquired)
        {
            Controls.sprite = Grow;
            Debug.Log("Grow image");
        }

    }
}