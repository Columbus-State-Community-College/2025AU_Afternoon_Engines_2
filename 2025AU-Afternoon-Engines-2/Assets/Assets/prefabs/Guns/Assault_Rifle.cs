using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class AssaultRifle : MonoBehaviour
{
    public Rigidbody bullet;
    public float magazine = 20; // How much ammo the magazine starts with.
    public float magazineSize = 3; // The maximum amount of ammo that can be reloaded into the gun
    public static float reserve = 300; // The ammo that gets reloaded from the gun.
    public static float maxReserve = 300;

    public static bool isReloading = false; 
    public float reloadTime = 2.3f; 
    public float accuracy = 1.0f; 
    public float bulletForce = 1500f; 
    private float cooldown = 0f;

    public TextMeshProUGUI ammoTex;

    public ParticleSystem muzzleFlash;
    public Transform Gun;

    // NEW sound system component
    private GunSound gunSound;

    private InputSystems controls; // Added controller support
    private bool firePressed;
    private bool reloadPressed;

    void Start()
    {
        // assign sound wrapper
        gunSound = GetComponent<GunSound>();
        reserve = 300;
        SetText();
    }

    private void Awake()
    {
        controls = new InputSystems();

        // Set booleans in callbacks for controller buttons
        controls.Player.Fire.performed += ctx => firePressed = ctx.ReadValue<float>() > 0.5f;
        controls.Player.Fire.canceled += ctx => firePressed = false;


        controls.Player.Reload.performed += ctx => reloadPressed = true;
        controls.Player.Reload.canceled += ctx => reloadPressed = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        SetText();
        if (PauseMenu.GameIsPaused || Time.unscaledTime - PauseMenu.lastUnpauseTime < 0.1f)
            return;

        // SHOOT
        if (firePressed) { //fires the gun, with a certain accuracy.
            if (magazine > 0 && !isReloading && cooldown <= 0)
            {
                if (gunSound != null) gunSound.PlayShoot();

                // old effect unchanged
                cooldown = 0.1f;
                muzzleFlash.Play();
                float horizontalSpread = Random.Range(-accuracy, accuracy);
                float verticalSpread = Random.Range(-accuracy, accuracy);
                Quaternion bulletRotation = transform.rotation * Quaternion.Euler(verticalSpread, horizontalSpread, 0);
                Rigidbody instance = Instantiate(bullet, transform.position, bulletRotation) as Rigidbody;
                instance.AddForce(instance.transform.forward * bulletForce);
                magazine -= 1;
                SetText();
            }
        }
        if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
        // RELOAD
        if (magazine < magazineSize && reserve > 0 && !isReloading){
            if (reloadPressed)
            {
                StartCoroutine(Reload());
                reloadPressed = false; //reset controller input
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