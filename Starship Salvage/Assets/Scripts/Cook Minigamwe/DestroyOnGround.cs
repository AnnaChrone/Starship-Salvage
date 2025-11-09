using UnityEngine;

public class DestroyOnGround : MonoBehaviour
{
    private SpawnItems Game;

    private void Start()
    {
        Game = FindAnyObjectByType<SpawnItems>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KitchenFloor"))
        {
           Debug.Log("Landed on floor");
            Destroy(gameObject);
        } else if (collision.gameObject.CompareTag("Basket"))
        {
            if (gameObject.tag == "Good")
            {
                Debug.Log("Player collected good!");
                Game.collected++;
                if (Game.collected == Game.WinCount)
                {
                    Game.GameOver = true;
                    Game.WinScreen.SetActive(true);
                }
            } else if (gameObject.tag == "Bad")
            {
                if (Game.chances > 1)
                {
                    Game.chances--;
                    Debug.Log("Player collected bad!");
                    Debug.Log(Game.chances + " chances left");
                } else
                {
                    Debug.Log("GAME OVER");
                    Game.GameOverScreen.SetActive(true);
                    Game.GameOver = true;
                }
               
                
            }
            Destroy(gameObject);
        }
    }
}


