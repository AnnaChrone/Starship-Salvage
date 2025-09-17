using UnityEngine;

public class GameStart : MonoBehaviour
{
    
    public Spaceship spaceship;
    public SpaceshipDialogue crashLand;
    public GameObject CrashLandingScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            
            spaceship.StartDialogue(crashLand);
       
        
    }

  
}
