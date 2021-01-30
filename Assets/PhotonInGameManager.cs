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
    public class PhotonInGameManager : MonoBehaviourPunCallbacks
    {
        public static PhotonInGameManager instance;
        public bool isGameStart;
        public int PlayerInRoom;
        [SerializeField] Player[] playerList;
        [Header("Camera Object")]
        public GameObject CameraOverview;
        [Header("PlayerCount Zone")]
        public TextMeshProUGUI textCount;
        [Header("Name Zone")]
        public TextMeshProUGUI textName;
        [Header("Countdown Zone")]
        public TextMeshProUGUI textShowCountdown;
        public GameObject CountdownObj;
        float CountTime = 7f;
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

            LoadPlayerInScene();
            FullRoom();
        }
        void FullRoom()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                photonView.RPC("StartCountdown", RpcTarget.AllBufferedViaServer);
            }

            if (!isGameStart)
                if (PlayerInRoom == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
                //if (PlayerInRoom == 1 && PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("StartCountdown", RpcTarget.AllBufferedViaServer);
                }
        }
        [PunRPC]
        void StartCountdown()
        {
            var manager = GameManager.instance;

            CountdownObj.SetActive(true);
            CountTime -= Time.deltaTime;

            if (CountTime > 5f)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                textShowCountdown.text = "ARE YOU READY ?!";
            }
            else if (CountTime <= 5f)
            {
                if ((int)CountTime == 4)
                {
                    textShowCountdown.text = "Loading Scene ...";

                }
                else if ((int)CountTime == 2)
                {
                    textShowCountdown.text = "Loading Player ...";

                    PhotonScene.Instance.SpawnPlayer();

                    // LoadPlayerTeam();
                }
                else if ((int)CountTime == 1)
                {
                    textShowCountdown.text = "Loading Team ...";

                    GameManager.instance.LoadTeam();
                    //LoadPlayerTeam();
                }
                else if ((int)CountTime == 0)
                {
                    CountTime = 10;
                    isGameStart = true;
                    CameraOverview.SetActive(false);

                    GameManager.instance.PlayerMove(true);


                }
            }
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

            Debug.Log("Player IN ROOM " + PlayerInRoom);

            /*manager.ClearListTeam();

            for (int i = 0; i < PlayerInRoom; i++)
            {
                if (GameManager.instance)
                {
                    if (manager.TeamAData.PlayerTeamData.Count <= manager.TeamBData.PlayerTeamData.Count)
                    {
                        manager.AddDataToTeam("A", playerList[i].NickName, playerList[i].UserId, null);
                    }
                    else
                    {
                        manager.AddDataToTeam("B", playerList[i].NickName, playerList[i].UserId, null);

                    }
                    Debug.Log(playerList[i].NickName);
                }
                else
                {
                    break;
                }
            }*/
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
    }
}



