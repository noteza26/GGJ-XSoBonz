using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Balloon.Photon
{

    public class PhotonInGameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static PhotonInGameManager instance;
        public bool isGameStart;
        public bool isCountdown;
        public bool StopMovePlayer;
        public int PlayerInRoom;
        [SerializeField] List<string> namePlayerList;
        [Header("Camera Object")]
        public GameObject CameraOverview;

        [Header("PlayerCount Zone")]
        public TextMeshProUGUI textCount;
        public Image[] imageShowPlayer;
        public TextMeshProUGUI[] TextShowPlayerName
        ;
        [Header("Name Zone")]
        public TextMeshProUGUI textName;
        [Header("Countdown Zone")]
        public TextMeshProUGUI textShowCountdown;
        public GameObject CountdownObj;
        [SerializeField] float CountTime = 7f;
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
            textName.text = PhotonNetwork.LocalPlayer.NickName;
        }
        void Update()
        {
            StartCountdown();
            LoadPlayerInScene();
            FullRoom();
        }
        void FullRoom()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //photonView.RPC("StartCountdown", RpcTarget.AllBufferedViaServer);
            }

            if (!isGameStart)
                if (PlayerInRoom == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
                //    if (PlayerInRoom == 1 && PhotonNetwork.IsMasterClient)
                {
                    isCountdown = true;
                }
        }
        void StartCountdown()
        {
            if (!isCountdown) return;
            if (isGameStart) return;

            var manager = GameManager.instance;
            CountdownObj.SetActive(true);
            CountTime -= Time.deltaTime;
            var inttime = (int)CountTime;
            textShowCountdown.text = inttime.ToString();

            if (CountTime > 5f)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            else if (CountTime <= 5f)
            {
                if ((int)CountTime == 4)
                {
                    //  textShowCountdown.text = "Loading Scene ...";

                }
                else if ((int)CountTime == 2)
                {
                    // textShowCountdown.text = "Loading Player ...";

                    PhotonScene.Instance.SpawnPlayer();

                    // LoadPlayerTeam();
                }
                else if ((int)CountTime <= 0)
                {
                    if (PhotonNetwork.IsMasterClient && isCountdown && !isGameStart)
                        photonView.RPC("StartGame", RpcTarget.AllBufferedViaServer);

                }
            }
        }
        [PunRPC]
        void StartGame()
        {
            CameraOverview.SetActive(false);

            GameManager.instance.isStart = true;

            StopMovePlayer = false;

            isCountdown = false;
            isGameStart = true;
        }
        /*   void LoadPlayerTeam()
           {
               var manager = GameManager.instance;

               var findPlayer = GameObject.FindGameObjectsWithTag("PlayerController");

               if (findPlayer.Length == manager.PlayerInRoom) return;

               manager.ClearListTeam();

               for (int i = 0; i < findPlayer.Length; i++)
               {
                   var getPlayerData = findPlayer[i].GetComponent<PhotonPlayerManager>();
                   if (manager)
                   {
                       if (manager.TeamAData.PlayerTeamData.Count <= manager.TeamBData.PlayerTeamData.Count)
                       {
                           manager.AddDataToTeam("A", getPlayerData.PlayerName, getPlayerData.PlayerID, getPlayerData.gameObject);
                           getPlayerData.Team = PhotonPlayerManager.TeamList.A;
                           getPlayerData.LoadTeam();
                       }
                       else
                       {
                           manager.AddDataToTeam("B", getPlayerData.PlayerName, getPlayerData.PlayerID, getPlayerData.gameObject);
                           getPlayerData.Team = PhotonPlayerManager.TeamList.B;
                           getPlayerData.LoadTeam();
                       }
                       Debug.Log(getPlayerData.PlayerName);
                   }
               }

           }*/

        public void LoadPlayerInScene()
        {

            if (PlayerInRoom == PhotonNetwork.PlayerList.Length) return;

            PlayerInRoom = PhotonNetwork.PlayerList.Length;


            textCount.text = "Waiting for Player " + PlayerInRoom.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

            var manager = GameManager.instance;
            var playerList = PhotonNetwork.PlayerList;

            namePlayerList.Clear();

            foreach (var item in TextShowPlayerName)
            {
                item.text = "";
            }
            foreach (var item in imageShowPlayer)
            {
                item.gameObject.SetActive(false);
            }

            for (var i = 0; i < playerList.Length; i++)
            {
                imageShowPlayer[i].gameObject.SetActive(true);
                namePlayerList.Add(PhotonNetwork.PlayerList[i].NickName);
                TextShowPlayerName[i].text = namePlayerList[i];

                Debug.Log(PhotonNetwork.PlayerList[i].NickName);
            }


            Debug.Log("Player in room " + PlayerInRoom);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            base.OnPlayerEnteredRoom(newPlayer);


        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            base.OnPlayerLeftRoom(otherPlayer);


        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isGameStart);
                stream.SendNext(isCountdown);
                stream.SendNext(StopMovePlayer);

            }
            else
            {
                // Network player, receive data
                this.isGameStart = (bool)stream.ReceiveNext();
                this.isCountdown = (bool)stream.ReceiveNext();
                this.StopMovePlayer = (bool)stream.ReceiveNext();
            }
        }
    }
}



