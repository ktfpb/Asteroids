using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float laserLifetime = 1f;
    
    private void Awake() {
        Destroy(gameObject, laserLifetime);
    }
}
