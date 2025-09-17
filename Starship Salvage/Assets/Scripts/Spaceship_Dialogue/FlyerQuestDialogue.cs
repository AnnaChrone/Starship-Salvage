using UnityEngine;

public class FlyerQuestDialogur : MonoBehaviour
{
    public Spaceship spaceship;
    public SpaceshipDialogue FlyerQuest;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spaceship.StartDialogue(FlyerQuest);
    }

    
}
