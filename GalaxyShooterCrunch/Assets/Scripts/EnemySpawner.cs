using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 2f;
    private float nextSpawn = 0f;
    public bool isSpawning = true; // Add this
    
    void Start()
    {
        isSpawning = true;
    }
    
    void Update()
    {
        if (!isSpawning) return; // Stop spawning if game over
        
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            
            // Spawn at random X at top of screen
            float randomX = Random.Range(-8f, 8f);
            Vector2 spawnPos = new Vector2(randomX, 6f);
            
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
    
    // Call this when player dies to stop spawning
    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log("Enemy Spawning Stopped");
    }
    
    // Optional: Wave system for increasing difficulty
    public void IncreaseDifficulty()
    {
        spawnRate = Mathf.Max(0.5f, spawnRate - 0.1f); // Faster spawning, min 0.5s
        Debug.Log("Difficulty Increased! Spawn Rate: " + spawnRate);
    }
}