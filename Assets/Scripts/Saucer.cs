using UnityEngine;

public class Saucer : MonoBehaviour
{ 
    [SerializeField] private ParticleSystem destroyedParticles;
    
    public int hitPoints = 3;

    public GameManager gameManager;

    [SerializeField] private float pursuitSpeed = 2f;

    private Rigidbody2D rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        gameManager.saucerCount++;
    }

    private void Update()
    {
        Transform enemy = GameObject.FindGameObjectWithTag("Player").transform; 
        Vector2 direction = (enemy.position - transform.position).normalized; 
        rb.velocity = direction * pursuitSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            hitPoints--;
            Destroy(collision.gameObject);

            if (hitPoints <= 0)
            {
                gameManager.saucerCount--;
                Instantiate(destroyedParticles, transform.position, Quaternion.identity);
                gameManager.destroyedSaucers++;
                Destroy(gameObject);
            }
        }
        
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            
            gameManager.saucerCount--;
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            gameManager.destroyedSaucers++;
            Destroy(gameObject);
        }
    }
}
