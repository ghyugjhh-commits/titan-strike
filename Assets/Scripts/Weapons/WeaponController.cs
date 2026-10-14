using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Config")]
    [SerializeField] private float fireRate = 0.12f;
    [SerializeField] private float damage = 22f;
    [SerializeField] private float weaponRange = 150f;
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private int reserveAmmo = 90;
    [SerializeField] private float reloadTime = 1.2f;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private LayerMask hitMask = -1;

    private float shootTimer;
    private bool isReloading;
    private bool fireHeld;
    private int currentAmmo;

    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;
    public bool IsReloading => isReloading;

    private void Awake()
    {
        currentAmmo = magazineSize;
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (isReloading)
        {
            return;
        }

        if (fireHeld && shootTimer <= 0f && currentAmmo > 0)
        {
            Fire();
        }
    }

    public void SetCamera(Camera camera)
    {
        playerCamera = camera;
    }

    public void SetFireHeld(bool isHeld)
    {
        fireHeld = isHeld;
    }

    public void TriggerReload()
    {
        if (isReloading || currentAmmo == magazineSize || reserveAmmo <= 0)
        {
            return;
        }

        isReloading = true;
        Invoke(nameof(CompleteReload), reloadTime);
    }

    private void CompleteReload()
    {
        int needed = Mathf.Min(magazineSize - currentAmmo, reserveAmmo);
        currentAmmo += needed;
        reserveAmmo -= needed;
        isReloading = false;
    }

    private void Fire()
    {
        if (playerCamera == null)
        {
            return;
        }

        shootTimer = fireRate;
        currentAmmo--;

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, weaponRange, hitMask))
        {
            Transform hitRoot = hit.transform.root;
            EnemyHealth enemyHealth = hitRoot.GetComponentInChildren<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
