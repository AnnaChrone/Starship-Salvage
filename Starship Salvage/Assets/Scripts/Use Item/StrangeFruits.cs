using UnityEngine;

public class StrangeFruits : MonoBehaviour, IUsable
{
    public bool consumable = true;

    [Header("Special Ability")]
    public string AbilityName; 

    public void Use(GameObject user)
    {

        // Unlock ability
        PlayerAbilities abilities = user.GetComponent<PlayerAbilities>();
        if (abilities != null && !string.IsNullOrEmpty(AbilityName))
        {
            abilities.UnlockAbility(AbilityName);
        }

        if (consumable)
        {
            Destroy(gameObject);
        }
    }
}