using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private Saucer saucerPrefab;
  
    public int asteroidCount = 0;
    public int saucerCount = 0;

    private int level = 0;
    
    public int destroyedAsteroids = 0;
    public int destroyedSaucers = 0;
    
    public Text score;
    public GameObject gameOverPanel;

    private void Update() 
    {
        if (asteroidCount == 0) {
            level++;
            
            int numAsteroids = 2 + (2 * level);
            for (int i = 0; i < numAsteroids; i++) {
                SpawnAsteroid();
            }
        }
        
        if (saucerCount == 0) {
            level++;
            
            int numSaucer = 1;
            for (int i = 0; i < numSaucer; i++) {
                SpawnSaucer();
            }
        }
    }

    private void SpawnAsteroid() 
    {
        float offset = Random.Range(0f, 1f);
        Vector2 viewportSpawnPosition = Vector2.zero;
        
        int edge = Random.Range(0, 4);
        if (edge == 0) {
            viewportSpawnPosition = new Vector2(offset, 0);
        } else if (edge == 1) {
            viewportSpawnPosition = new Vector2(offset, 1);
        } else if (edge == 2) {
            viewportSpawnPosition = new Vector2(0, offset);
        } else if (edge == 3) {
            viewportSpawnPosition = new Vector2(1, offset);
        }
        
        Vector2 worldSpawnPosition = Camera.main.ViewportToWorldPoint(viewportSpawnPosition);
        Asteroid asteroid = Instantiate(asteroidPrefab, worldSpawnPosition, Quaternion.identity);
        asteroid.gameManager = this;
    }
    
    private void SpawnSaucer() 
    {
        float offset = Random.Range(0f, 1f);
        Vector2 viewportSpawnPosition = Vector2.zero;
        
        int edge = Random.Range(0, 4);
        if (edge == 0) {
            viewportSpawnPosition = new Vector2(offset, 0);
        } else if (edge == 1) {
            viewportSpawnPosition = new Vector2(offset, 1);
        } else if (edge == 2) {
            viewportSpawnPosition = new Vector2(0, offset);
        } else if (edge == 3) {
            viewportSpawnPosition = new Vector2(1, offset);
        }
        
        Vector2 worldSpawnPosition = Camera.main.ViewportToWorldPoint(viewportSpawnPosition);
        Saucer saucer = Instantiate(saucerPrefab, worldSpawnPosition, Quaternion.identity);
        saucer.gameManager = this;
    }

    public void GameOver()
    {
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(2f);
        
        gameOverPanel.SetActive(true);
        score.text = "Score: " + (destroyedAsteroids * 10) + (destroyedSaucers * 20);

        while (!Input.GetKeyDown(KeyCode.Space)) 
        {
            yield return null; 
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }
}
