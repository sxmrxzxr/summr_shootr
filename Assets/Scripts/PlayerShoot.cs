using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private GameObject weaponGFX;    

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon weapon;
    private WeaponManager weaponManager;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No Camera Referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        weapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.isOn)
            return;

        if (weapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/weapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }        
    }

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    [Client]
    void Shoot()
    {
        RaycastHit hit;

        if (!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag.Equals(PLAYER_TAG))
            {
                CmdPlayerShot(hit.collider.name, weapon.damage);               
            }
        }
        else
        {
            Debug.Log("You hit nothing!");
        }
    }

    [Command]
    void CmdPlayerShot(string PlayerID, int damage)
    {
        Debug.Log(PlayerID + " has been shot!");
        Player p = GameManager.GetPlayer(PlayerID);
        p.RpcTakeDamage(damage);
    }
}
