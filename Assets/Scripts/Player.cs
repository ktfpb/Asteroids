using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 180f;

    private Rigidbody2D shipRigidbody;
    private bool isAlive = true;
    private bool isAccelerating = false;

    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private ParticleSystem destroyedParticles;
    [SerializeField] private Rigidbody2D laserPrefab;
    [SerializeField] private float laserSpeed = 12f;
    
    [SerializeField] private int laserCharge = 3;
    [SerializeField] private float laserCooldown = 10f;

    public Text speedText;
    public Text positionText;
    public Text rotationText;
    public Text laserChargeText;
    public Text laserCooldownText;
    
    private void Start()
    {
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAlive)
        {
            HandleShipAcceleration();
            HandleShipRotation();
            HandleShooting();
            HandleLaserShooting();
            
            speedText.text = "Speed: " + shipRigidbody.velocity.magnitude.ToString("F2");
            positionText.text = "Position: (" + transform.position.x.ToString("F2") + ", " + transform.position.y.ToString("F2") + ")";
            rotationText.text = "Rotation: " + transform.eulerAngles.z.ToString("F2");
            laserChargeText.text = "Laser Charges: " + laserCharge;

            if (laserCharge == 0)
            {
                StartCoroutine(LaserCooldown());
                laserCooldownText.text = "Laser Cooldown: " + Mathf.Round(laserCooldown) + "с";
                laserCooldown -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && isAccelerating)
        {
            shipRigidbody.AddForce(shipAcceleration * transform.up);
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
        }
    }

    private void HandleShipAcceleration()
    {
        isAccelerating = Input.GetKey(KeyCode.UpArrow);
    }

    private void HandleShipRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
        } 
        
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            
            Vector2 shipVelocity = shipRigidbody.velocity;
            Vector2 shipDirection = transform.up;
            float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);
            
            if (shipForwardSpeed < 0) 
            { 
                shipForwardSpeed = 0; 
            }

            bullet.velocity = shipDirection * shipForwardSpeed;
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }
    }
    
    private void HandleLaserShooting()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && laserCharge > 0)
        {
            Rigidbody2D laser = Instantiate(laserPrefab, bulletSpawn.position, Quaternion.identity);
            
            Vector2 shipVelocity = shipRigidbody.velocity;
            Vector2 shipDirection = transform.up;
            float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);
            
            if (shipForwardSpeed < 0) 
            { 
                shipForwardSpeed = 0; 
            }

            laser.transform.rotation = transform.rotation;
            laser.velocity = shipDirection * shipForwardSpeed;
            laser.AddForce(laserSpeed * transform.up, ForceMode2D.Impulse);

            laserCharge--;
        }
    }

    private IEnumerator LaserCooldown()
    {
        yield return new WaitForSeconds(laserCooldown);
        laserCharge = 3;
        laserCooldown = 10f;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid") || collision.CompareTag("Saucer"))
        {
            isAlive = false;

            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.GameOver();

            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}
