using UnityEngine;
using UnityEngine.UIElements;

public class StrangeFruits : MonoBehaviour, IUsable
{
    public bool consumable = true;

    [Header("Special Ability")]
    public string AbilityName; 
    public CanvasGroup AbilityImage;
    public AudioSource EatAudio;
    public Hotbar inventory;

    public void Use(GameObject user)
    {

        // Unlock ability

        PlayerAbilities abilities = user.GetComponent<PlayerAbilities>();
        if (abilities != null && !string.IsNullOrEmpty(AbilityName))
        {
            abilities.UnlockAbility(AbilityName);
            AbilityImage.alpha = 1;
        }

        if (consumable)
        {
            EatAudio.Play();
            inventory.RemoveItemByID(AbilityName);
        }
    }
}