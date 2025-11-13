using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime = 5;
    public float damage = 20;
    void Start()
    {
        
    }

    void Update()
    {
        if (lifeTime > 0) { //Deletes the bullet after 5 seconds without touching anything
            lifeTime -= Time.deltaTime;
            }
        else {
            Destroy(gameObject);
            }
    }
    
    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
