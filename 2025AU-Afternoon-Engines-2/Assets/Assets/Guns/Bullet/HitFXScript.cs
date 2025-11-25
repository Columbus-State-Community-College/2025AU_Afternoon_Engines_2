using UnityEngine;
using UnityEngine.VFX;

public class GroundHitFXScript : MonoBehaviour
{
    public VisualEffect vfx;
    private float lifeTime = 0.5f;
    
    void Start()
    {
       vfx = GetComponent<VisualEffect>(); 
       vfx.SendEvent("OnPlay");
    }

    void Update()
    {
        if (lifeTime > 0) { //Deletes the object after 5 seconds without touching anything
            lifeTime -= Time.deltaTime;
            }
        else {
            Destroy(gameObject);
            }
    }
}
