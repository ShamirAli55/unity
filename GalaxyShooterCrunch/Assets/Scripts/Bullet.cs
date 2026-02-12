using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    
    void Start()
    {
        Destroy(gameObject, 2f);
    }
    
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Destroy enemy
            Destroy(gameObject); // Destroy bullet
        }
    }
}