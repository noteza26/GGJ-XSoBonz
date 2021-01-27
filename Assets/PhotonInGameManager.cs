using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Balloon.Photon
{
    [System.Serializable]
    public struct PlayerData
    {
        public string PlayerName;
        public GameObject PlayerObject;
    }
    public class PhotonInGameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static PhotonInGameManager instance;
        public string dataSend;
        public List<PlayerData> InRoomPlayer;

        public List<GameObject> InRoomObjPlayer;
        // Start is called before the first frame update
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }
        private void Start()
        {

        }
        void Update()
        {
            LoadPlayerInScene();
        }
        public void LoadPlayerInScene()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (InRoomObjPlayer.Count == PhotonNetwork.PlayerList.Length) return;

            Debug.Log("Load");
            InRoomObjPlayer.Clear();
            InRoomPlayer.Clear();

            var findPlayer = GameObject.FindGameObjectsWithTag("PlayerController");

            for (var i = 0; i < findPlayer.Length; i++)
            {
                InRoomObjPlayer.Add(findPlayer[i]);
            }

            for (var i = 0; i < InRoomObjPlayer.Count; i++)
            {
                var getDataFromObj = InRoomObjPlayer[i].GetComponent<PhotonPlayerManager>();
                var data = new PlayerData { PlayerName = getDataFromObj.GetPlayername(), PlayerObject = getDataFromObj.gameObject };
                if (photonView.IsMine)
                    dataSend = photonView.name;

                InRoomPlayer.Add(data);
            }
        }
        public void AddData(string newPlayer, GameObject playerObject)
        {
            //photonView.RPC("PunAddData", RpcTarget.AllBuffered, newPlayer, playerObject);
            /*   var data = new PlayerData { PlayerName = newPlayer, PlayerObject = playerObject };
               InRoomPlayer.Add(data);
               Debug.Log(data.PlayerName);*/
        }

        public void DelData(Player otherPlayer)
        {
            for (int i = 0; i < InRoomPlayer.Count; i++)
            {
                if (InRoomPlayer[i].PlayerName == otherPlayer.NickName)
                {
                    InRoomPlayer.RemoveAt(i);
                    break;
                }
            }
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (PhotonNetwork.IsMasterClient)
                if (stream.IsWriting)
                {
                    // We own this player: send the others our data
                    stream.SendNext(dataSend);
                }
                else
                {
                    // Network player, receive data
                    dataSend = (string)stream.ReceiveNext();
                }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            // photonView.RPC("AddData", RpcTarget.AllBuffered, newPlayer);
            //            Debug.Log(newPlayer.ToStringFull());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            // photonView.RPC("DelData", RpcTarget.AllBuffered, otherPlayer);

        }
    }
}

