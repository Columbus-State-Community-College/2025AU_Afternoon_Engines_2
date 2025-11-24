using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime = 5;
    public float damage = 20;
    public GameObject groundImpactFX;
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
        if (collision.gameObject.CompareTag("Enemy")) {
            damage = 20;
        }
        else if (collision.gameObject.CompareTag("Ground")) {
            Instantiate(
                groundImpactFX,
                collision.contacts[0].point,
                Quaternion.identity
            );
            Debug.Log("log"); 
        }
        Destroy(gameObject);
    }
    void Destroy() {
        Destroy(gameObject);
    }
}
