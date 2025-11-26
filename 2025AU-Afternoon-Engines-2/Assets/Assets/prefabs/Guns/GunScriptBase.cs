using UnityEngine;
using System.Collections;
using TMPro;

public class GunScriptBase : MonoBehaviour
{
    public Rigidbody bullet;
    public float magazine = 7; // How much ammo the magazine starts with.
    public float magazineSize = 7; // The maximum amount of ammo that can be reloaded into the gun
    public static float reserve = 500; // The ammo that gets reloaded from the gun.
    public static float maxReserve = 500;

    public static bool isReloading = false; 
    public float reloadTime = 2.3f; 
    public float accuracy = 1.0f; 
    public float bulletForce = 1500f; 

    public TextMeshProUGUI ammoTex;

    public ParticleSystem muzzleFlash;
    public Transform Gun;

    // NEW sound system component
    private GunSound gunSound;

    void Start()
    {
        // assign sound wrapper
        gunSound = GetComponent<GunSound>();

        reserve = 500;
        SetText();
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused || Time.unscaledTime - PauseMenu.lastUnpauseTime < 0.1f)
            return;

        // SHOOT
        if (Input.GetMouseButtonUp(0)) { //fires the gun, with a certain accuracy.
            if (magazine > 0 && !isReloading)
            {
                // NEW — play via SoundManager
                if (gunSound != null) gunSound.PlayShoot();

                // old effect unchanged
                muzzleFlash.Play();
                float horizontalSpread = Random.Range(-accuracy, accuracy);
                float verticalSpread = Random.Range(-accuracy, accuracy);
                Quaternion bulletRotation = transform.rotation * Quaternion.Euler(verticalSpread, horizontalSpread, 0);
                Rigidbody instance = Instantiate(bullet, transform.position, bulletRotation) as Rigidbody;
                instance.AddForce(instance.transform.forward * bulletForce);
                magazine -= 1;
                SetText();
            }
            else if (!isReloading) {
                // NEW — empty click
                if (gunSound != null) gunSound.PlayEmpty();
            }
        }

        // RELOAD
        if (magazine < magazineSize && reserve > 0 && !isReloading){
            if (Input.GetKeyDown(KeyCode.R)){
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload() //reloads the gun. all of this is literally just so I can make the reload take time.
    {
        isReloading = true;
        Gun.Rotate(-45f, 0f, 0f);

        // NEW — reload sound
        if (gunSound != null) gunSound.PlayReload();

        // reload timing logic stays the same
        if (PerkChecker.hasSpeedReload)
        {
            yield return new WaitForSeconds(reloadTime / 2);
        }
        else {
            yield return new WaitForSeconds(reloadTime);
        }

        float ammoNeeded = magazineSize - magazine;
        float ammoToTransfer = Mathf.Min(ammoNeeded, reserve);
        magazine += ammoToTransfer;
        reserve -= ammoToTransfer;
        isReloading = false;
        Gun.Rotate(45f, 0f, 0f, Space.Self);
        SetText();
    }
    void SetText() {
        ammoTex.text = magazine.ToString() + "/" + reserve.ToString();
    }
}