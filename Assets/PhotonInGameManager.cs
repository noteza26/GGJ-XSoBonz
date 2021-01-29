using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
        public bool isGameStart;
        public int PlayerInRoom;

        [Header("Camera Object")]
        public GameObject CameraOverview;
        [Header("PlayerCount Zone")]
        public TextMeshProUGUI textCount;

        [Header("Countdown Zone")]
        public TextMeshProUGUI textShowCountdown;
        public GameObject CountdownObj;

        public List<PlayerData> InRoomPlayer;

        public List<GameObject> InRoomObjPlayer;

        [SerializeField] float CountTime = 10f;
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
            CountdownObj.SetActive(false);
        }
        void Update()
        {

            LoadPlayerInScene();
            FullRoom();
        }
        void FullRoom()
        {
            if (isGameStart) return;
            if (PlayerInRoom == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("StartCountdown", RpcTarget.AllBufferedViaServer);
            }
        }
        [PunRPC]
        void StartCountdown()
        {

            CountdownObj.SetActive(true);
            CountTime -= Time.deltaTime;

            if (CountTime >= 5f)
            {
                textShowCountdown.text = "ARE YOU READY ?!";
            }
            else if (CountTime < 5f)
            {
                textShowCountdown.text = "START IN " + (int)CountTime;
                if ((int)CountTime == 0)
                {
                    CountTime = 10f;
                    isGameStart = true;
                    CameraOverview.SetActive(false);
                    PhotonScene.Instance.SpawnPlayer();
                }
            }
        }
        public void LoadPlayerInScene()
        {

            if (PlayerInRoom == PhotonNetwork.PlayerList.Length) return;

            Debug.Log("Load");

            PlayerInRoom = PhotonNetwork.PlayerList.Length;

            textCount.text = "Waiting for Player " + PlayerInRoom.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

            Debug.Log("Update " + PlayerInRoom);
            /*    InRoomObjPlayer.Clear();
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
                }*/
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
                    stream.SendNext(PlayerInRoom);
                }
                else
                {
                    // Network player, receive data
                    PlayerInRoom = (int)stream.ReceiveNext();
                }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            base.OnPlayerEnteredRoom(newPlayer);

            // photonView.RPC("AddData", RpcTarget.AllBuffered, newPlayer);
            //            Debug.Log(newPlayer.ToStringFull());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            base.OnPlayerLeftRoom(otherPlayer);

            // photonView.RPC("DelData", RpcTarget.AllBuffered, otherPlayer);

        }
    }
}

