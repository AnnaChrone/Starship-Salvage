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
                Game.Fruits.text = "Fruits caught: " + Game.collected + "/15";
                if (Game.collected == Game.WinCount)
                {
                    // Destroy all previously spawned items
                    foreach (var item in GameObject.FindGameObjectsWithTag("Good"))
                        Destroy(item);
                    foreach (var item in GameObject.FindGameObjectsWithTag("Bad"))
                        Destroy(item);
                    Game.GameOver = true;
                    Game.WinScreen.SetActive(true);
                    Game.CounterPanel.SetActive(false);
                    Game.stats.text = "Time taken: " + Mathf.CeilToInt(60f - Game.currentTime).ToString();

                }
            } else if (gameObject.tag == "Bad")
            {
                if (Game.chances > 1)
                {
                    Game.chances--;
                    Debug.Log("Player collected bad!");
                    Game.Rocks.text = "Lives left: " + Game.chances + "/3"; 
                    Debug.Log(Game.chances + " chances left");
                } else
                {
                    // Destroy all previously spawned items
                    foreach (var item in GameObject.FindGameObjectsWithTag("Good"))
                        Destroy(item);
                    foreach (var item in GameObject.FindGameObjectsWithTag("Bad"))
                        Destroy(item);
                    Debug.Log("GAME OVER");
                    if (Game.chances == 0)
                    {
                        Game.Reason.text = "You caught too many rocks!";
                    } else
                    {
                        Game.Reason.text = "You ran out of time!";
                    }
                        Game.CounterPanel.SetActive(false);
                    Game.GameOverScreen.SetActive(true);
                    Game.GameOver = true;
                }
               
                
            }
            Destroy(gameObject);
        }
    }
}


