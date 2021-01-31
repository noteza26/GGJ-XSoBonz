using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Balloon.Photon;
using Photon;
using Photon.Pun;
using Photon.Realtime;
public class BulletManager : MonoBehaviourPunCallbacks, IPunObservable
{

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
            /*



            TO DO DESTORY SLEF TO CUT SCORE PLAYER;



            */


            //  if (photonPlayerManager == null) return;

            GameManager.instance.BoardKilled(Owner, Owner);
            // PhotonNetwork.Destroy(photonPlayerManager.gameObject);

            Debug.Log("Killed self");
        }
        Destroy(this.gameObject);
        // Debug.Log(triggerTag);

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(Owner);
            stream.SendNext(SpeedBullet);
            //   stream.SendNext(photonPlayerManager);

        }
        else
        {
            // Network player, receive data
            this.Owner = (string)stream.ReceiveNext();
            this.SpeedBullet = (float)stream.ReceiveNext();
            // this.photonPlayerManager = (PhotonPlayerManager)stream.ReceiveNext();
        }
    }
}
