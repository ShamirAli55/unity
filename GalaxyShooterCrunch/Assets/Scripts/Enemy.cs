using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public GameObject enemyBulletPrefab;
    public float shootRate = 1.5f;
    private float nextShoot = 0f;
    
    public int scoreValue = 100; // Points for killing this enemy

    public int TotalScore = 0;
    // Audio variables for shooting
    public AudioClip enemyLaserSound;
    private AudioSource audioSource;

    void Start()
    {
        nextShoot = Time.time + Random.Range(1f, 3f);

        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Movement
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Destroy if off screen
        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }

        // Shooting
        if (Time.time > nextShoot && enemyBulletPrefab != null)
        {
            nextShoot = Time.time + shootRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Get the sprite height
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Spawn bullet at bottom edge of sprite
            float halfHeight = sr.bounds.extents.y;
            Vector3 bulletSpawnPosition = transform.position + (Vector3.down * halfHeight);
            
            Instantiate(enemyBulletPrefab, bulletSpawnPosition, Quaternion.identity);
        }
        else
        {
            // Fallback if no sprite renderer
            Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        }
    
        // Play sound
        if (enemyLaserSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(enemyLaserSound, 0.5f);
        }
    }

    void PlayEnemyLaserSound()
    {
        if (enemyLaserSound != null && audioSource != null)
        {
            audioSource.pitch = 0.7f; // Lower pitch for enemy lasers
            audioSource.PlayOneShot(enemyLaserSound, 0.5f);
            audioSource.pitch = 1f; // Reset pitch
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // ADD SCORE when killed
            AddScore();
            
            // Play explosion sound on death
            AudioSource.PlayClipAtPoint(enemyLaserSound, Camera.main.transform.position, 0.3f);

            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
    
    void AddScore()
    {
        // Try to find ScoreDisplay and add points
        ScoreDisplay scoreSystem = FindObjectOfType<ScoreDisplay>();
        if (scoreSystem != null)
        {
            TotalScore  =  TotalScore + scoreValue;
            scoreSystem.AddScore(TotalScore);
            Debug.Log( TotalScore + " points!");
        }
        else
        {
            Debug.Log("ScoreDisplay not found! +" + TotalScore + " points");
        }
    }
}