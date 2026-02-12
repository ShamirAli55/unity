using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;
    private float nextFire = 0f;
    
    private float xMin, xMax, yMin, yMax;
    private float padding = 0.5f;
    
    public bool isAlive = true;
    
    // Audio
    public AudioClip laserSound;
    public AudioClip deathSound;
    private AudioSource audioSource;
    
    // UI/Menu
    public GameObject gameOverPanel;
    public string menuSceneName = "MainMenu";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CalculateScreenBounds();
        
        // Audio setup
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        
        Time.timeScale = 1f;
    }

    void CalculateScreenBounds()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    void Update()
    {
        if (!isAlive)
        {
            HandleGameOverInput();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
            return;
        }
        
        HandleMovement();
        HandleShooting();
    }
    
    void HandleGameOverInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }
    
    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(moveX, moveY) * speed;
        ClampPlayerPosition();
    }
    
    void HandleShooting()
    {
        if ((Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space)) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        PlaySound(laserSound, 0.7f);
    }
    
    void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    void ClampPlayerPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        transform.position = pos;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive) return;
        
        // ANY HIT = INSTANT DEATH
        if (other.CompareTag("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("EnemyBullets"))
        {
            Die();
        }
    }
    
    public void Die()
    {
        isAlive = false;
        Debug.Log("PLAYER DIED!");
        
        // Play death sound
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 0.7f);
        }
        
        // Stop moving
        rb.velocity = Vector2.zero;
        
        // Visual feedback
        GetComponent<SpriteRenderer>().color = Color.red;
        
        // Disable collider
        GetComponent<Collider2D>().enabled = false;
        
        // Clean up scene
        CleanUpScene();
        
        // Show game over UI if exists
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        Debug.Log("GAME OVER - Press R to Restart, M for Menu");
    }
    
    void CleanUpScene()
    {
        // Destroy all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        
        // Destroy all bullets
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        
        // Destroy all enemy bullets
        EnemyBullet[] enemyBulletScripts = FindObjectsOfType<EnemyBullet>();
        foreach (EnemyBullet bullet in enemyBulletScripts)
        {
            Destroy(bullet.gameObject);
        }
        
        // Stop enemy spawner
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
        }
    }
    
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void ReturnToMenu()
    {
        Debug.Log("Returning to Main Menu...");
        CleanUpScene();
        SceneManager.LoadScene(menuSceneName);
    }
}