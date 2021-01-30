using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Balloon.Photon;
public class BulletManager : MonoBehaviour
{
    public PhotonPlayerManager photonPlayerManager;
    public string Owner;
    public float SpeedBullet;
    public float TimeToDestroy;
    // Update is called once per frame
    private void Start()
    {
        Destroy(this.gameObject, TimeToDestroy);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * SpeedBullet * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var triggerTag = other.tag;
        if (triggerTag == "PlayerController")
        {
            var player = other.GetComponent<PhotonPlayerManager>();
            player.Hurt(Owner);
        }
        else if (triggerTag == "AI")
        {
            if (photonPlayerManager == null) return;

            GameManager.instance.BoardKilled(Owner, Owner);
            Destroy(photonPlayerManager.gameObject);
            Debug.Log("Killed self");
        }
        Destroy(this.gameObject);
        // Debug.Log(triggerTag);

    }
}
