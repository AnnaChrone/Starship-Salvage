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
    public GameObject GameOverScreen;
    public GameObject WinScreen;

    [Header("Fall Settings")]
    [Tooltip("Drag applied to Rigidbody to slow the fall (higher = slower).")]
    public float fallDrag = 2f;

    [Tooltip("Multiplier for gravity. 1 = normal gravity, 0.5 = half as strong, etc.")]
    public float gravityScale = 0.5f;
    void Start()
    {
        _timer = timer;
    }

    void Update()
    {
        // countdown
        _timer -= Time.deltaTime;

        // spawn when countdown reaches zero
        if (_timer <= 0f)
        {
            Spawn();
            _timer = timer; // reset timer AFTER spawning
        }
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
}
