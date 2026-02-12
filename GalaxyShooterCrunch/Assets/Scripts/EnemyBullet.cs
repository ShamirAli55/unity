using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public AudioClip hitSound;
    
    void Start()
    {
        Destroy(gameObject, 3f);
    }
    
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Play hit sound
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, 0.3f);
            }
            
            Destroy(gameObject); // Destroy bullet
            
            // KILL player (not damage - you removed TakeDamage)
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die(); // Call Die() instead of TakeDamage()
            }
        }
        
        // Optional: Destroy bullet if it hits player bullet
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}