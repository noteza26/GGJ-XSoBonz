using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Balloon.Photon;
public class PlayerWeapon : MonoBehaviour
{
    PhotonPlayerManager photonPlayerManager;
    PhotonView pv;

    public int MaxAmmo;
    public int AmmoCount;
    public int ReloadTimer;
    public bool onReload;
    public GameObject bulletPrefab;
    public Transform bulletOut;

    public TextMeshProUGUI textAmmo;
    void Awake()
    {
        //  photonPlayerManager = this.GetComponent<PhotonPlayerManager>();
        //  pv = this.GetComponent<PhotonView>();
    }
    private void Start()
    {
        AmmoCount = MaxAmmo;
        photonPlayerManager = this.GetComponent<PhotonPlayerManager>();
        pv = this.GetComponent<PhotonView>();
        UpdateAmmo();

    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
        UpdateAmmo();

    }
    void CheckInput()
    {
        if (!photonPlayerManager.StopMove)
        {
            if (pv.IsMine)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (onReload) return;

                    if (AmmoCount <= 0)
                        StartCoroutine("ReloadAmmo");
                    else
                    {
                        var bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletOut.position, bulletOut.rotation);

                        //  bullet.GetComponent<BulletManager>().photonPlayerManager = photonPlayerManager;
                        bullet.GetComponent<BulletManager>().Owner = pv.name;

                        AudioManager.instance.SoundShoot(bullet.transform);

                        AmmoCount--;

                        UpdateAmmo();

                        Debug.Log("Shoot");
                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    onReload = true;

                    StartCoroutine("ReloadAmmo");
                }
            }
        }
    }
    IEnumerator ReloadAmmo()
    {
        AmmoCount = 0;
        UpdateAmmo();
        yield return new WaitForSecondsRealtime(ReloadTimer);
        AmmoCount = MaxAmmo;
        onReload = false;
        UpdateAmmo();

    }
    void UpdateAmmo()
    {
        textAmmo.text = AmmoCount.ToString();
    }
}
