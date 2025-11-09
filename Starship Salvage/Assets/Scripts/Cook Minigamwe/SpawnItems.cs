using TMPro;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public float timer = 2f; // time between spawns
    private float _timer;
    [Header("Prefabs to Spawn")]
    public GameObject[] prefabs; // assign multiple prefabs in the Inspector
    public bool GameOver =false;
    public GameObject[] spawnpoints;
    public int chances = 3;
    public int collected = 0;
    public int WinCount;
    [Header("Lose Screen")]
    public GameObject GameOverScreen;
    public TextMeshProUGUI Reason;

    [Header("Win Screen")]
    public GameObject WinScreen;
    public TextMeshProUGUI Fruits;
    public TextMeshProUGUI Rocks;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI stats;
    public GameObject CounterPanel;
   

    [Header("Fall Settings")]
    [Tooltip("Drag applied to Rigidbody to slow the fall (higher = slower).")]
    public float fallDrag = 2f;

    [Tooltip("Multiplier for gravity. 1 = normal gravity, 0.5 = half as strong, etc.")]
    public float gravityScale = 0.5f;

    public float startTime = 45f; 
    public float currentTime;
    public float winTime;






    void Start()
    {
        CounterPanel.SetActive(true);
        GameOver = false;
        _timer = timer;
        currentTime = startTime;
        chances = 3;
        collected = 0;
        winTime = 0;
        timerText.text = Mathf.CeilToInt(startTime).ToString();
        Fruits.text = "Fruits caught: 0/15";
        Rocks.text = "Lives left: 3/3";
}


    void Update()
    {
        // countdown
        _timer -= Time.deltaTime;

        // spawn when countdown reaches zero
        if (_timer <= 0f)
        {
            Spawn();
            _timer = timer; // reset timer after spawning
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) currentTime = 0;
        }
        if (GameOver)
        {
            winTime = currentTime;
        }
        // Update the UI if assigned
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }

        if (currentTime <= 0)
        {
            TimerEnded();
        }
    }
    void TimerEnded()
    {
        // Add whatever should happen when the timer ends
        if (!GameOver)
        {
            GameOver = true;
            GameOverScreen.SetActive(true);
        }
        Debug.Log(" Time’s up!");
    }

    void Spawn()
    {
        if (!GameOver)
        {
            int chosenPrefab = Random.Range(0, prefabs.Length);
            int chosenSpawnpoint = Random.Range(0, spawnpoints.Length);
            GameObject spawned = Instantiate(prefabs[chosenPrefab], spawnpoints[chosenSpawnpoint].transform.position, Quaternion.identity);

            // Apply slower fall physics
            Rigidbody rb = spawned.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearDamping = fallDrag; // increases air resistance
                rb.useGravity = true;
                // Custom gravity scaling
                rb.linearVelocity = Physics.gravity * gravityScale * Time.deltaTime;
            }
        }
        
    }

    public void ResetMinigame()
    {
        GameOver = false;
        currentTime = startTime;
        _timer = timer;
        chances = 3;
        collected = 0;
        winTime = 0;

        CounterPanel.SetActive(true);
        GameOverScreen.SetActive(false);
        WinScreen.SetActive(false);
        timerText.text = Mathf.CeilToInt(startTime).ToString();
        Fruits.text = "Fruits caught: 0/" + WinCount;
        Rocks.text = "Lives left: 3/3";
        stats.text = "";
    }
}
