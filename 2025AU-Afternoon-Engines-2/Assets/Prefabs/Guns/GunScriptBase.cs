using UnityEngine;
using System.Collections;
using TMPro;

public class GunScriptBase : MonoBehaviour
{
    public Rigidbody bullet;
    public float magazine = 7; // How much ammo the magazine starts with.
    public float magazineSize = 7; // The maximum amount of ammo that can be reloaded into the gun
    public float reserve = 50; // The ammo that gets reloaded from the gun.

    private bool isReloading = false; 
    public float reloadTime = 2.3f; 
    public float accuracy = 1.0f; 
    public float bulletForce = 1500f; 

    public AudioSource gunshot;
    public AudioSource reload;
    public AudioSource gunshotEmpty;
    public TextMeshProUGUI ammoTex;
   

    void Start()
    {
        SetText();
    }

    void Update()
    { 
        
        if (Input.GetMouseButtonUp (0)) { //fires the gun, with a certain accuracy.
            if (magazine > 0 && !isReloading) {
                gunshot.Play();
                float horizontalSpread = Random.Range(-accuracy, accuracy);
                float verticalSpread = Random.Range(-accuracy, accuracy);
                Quaternion bulletRotation = transform.rotation * Quaternion.Euler(verticalSpread, horizontalSpread, 0);
                Rigidbody instance = Instantiate (bullet, transform.position, bulletRotation) as Rigidbody;
                instance.AddForce (instance.transform.forward * bulletForce);
                magazine -= 1;
                SetText();
                }
            else if (!isReloading) {
                gunshotEmpty.Play();
            }
            }
        
        if (magazine < magazineSize && reserve > 0 && !isReloading) {
            if (Input.GetKeyDown(KeyCode.R)) {
                StartCoroutine(Reload());
            }
        }
        
    }
    
    IEnumerator Reload() //reloads the gun. all of this is literally just so I can make the reload take time.
    {
        isReloading = true;
        reload.Play();
        if (PerkChecker.hasSpeedReload) {
            reload.pitch = 2f;
            yield return new WaitForSeconds(reloadTime / 2); 
        }
        else {
            reload.pitch = 1f;
            yield return new WaitForSeconds(reloadTime); 
        }
        
        float ammoNeeded = magazineSize - magazine;
        float ammoToTransfer = Mathf.Min(ammoNeeded, reserve);
        magazine += ammoToTransfer; 
        reserve -= ammoToTransfer;  
        isReloading = false;
        SetText();
    }
    void SetText() {
        ammoTex.text = magazine.ToString() + "/" + reserve.ToString(); 
    }
}