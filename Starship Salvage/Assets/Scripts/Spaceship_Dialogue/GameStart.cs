using UnityEngine;

public class GameStart : MonoBehaviour
{
    
    public Spaceship spaceship;
    public SpaceshipDialogue crashLand;
    public GameObject CrashLandingScene;
    public AudioSource Siren;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       CrashLandingScene.SetActive(true);
      Siren.Play();
      spaceship.StartDialogue(crashLand, 0);
       
        
    }

  
}
