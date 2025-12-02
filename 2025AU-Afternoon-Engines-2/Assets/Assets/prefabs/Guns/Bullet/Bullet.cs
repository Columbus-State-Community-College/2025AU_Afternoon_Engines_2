using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime = 5;
    public float damage = 20;

    public GameObject groundImpactFX;
    public GameObject zombieBloodFX;


    void Start()
    {
       
    }

    void Update()
    {   
        if (GunHandler.hasBolt){
            damage = 100;
        }
        else {
            damage = 20;
        }
        if (lifeTime > 0) { //Deletes the bullet after 5 seconds without touching anything
            lifeTime -= Time.deltaTime;
            }
        else {
            Destroy(gameObject);
            }
    }
    
    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            Instantiate(
                zombieBloodFX,
                collision.contacts[0].point,
                Quaternion.identity
            );
        }
        else if (collision.gameObject.CompareTag("Ground")) {
            Instantiate(
                groundImpactFX,
                collision.contacts[0].point,
                Quaternion.identity
            );
        }
        Destroy(gameObject);
    }
}
