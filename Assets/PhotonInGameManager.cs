using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public Button CancelButton;
        public Button CancelButton2;
        public GameObject CameraOverview;
        public GameObject WaitingObj;
        public GameObject GameOverObj;
        public HightScoreManager hightScoreManager;
        [Header("Respawn Zone")]
        public float WaitForRespawn;
        public GameObject RespawnObj;
        public TextMeshProUGUI TextRespawn;

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
            CancelButton2.onClick.AddListener(CancelWaitingRoom);
            CancelButton.onClick.AddListener(CancelWaitingRoom);
            CameraOverview.SetActive(true);
            WaitingObj.SetActive(true);
            RespawnObj.SetActive(false);
            CountdownObj.SetActive(false);
            textName.text = PhotonNetwork.LocalPlayer.NickName;
        }
        void Update()
        {
            if (!PhotonNetwork.InRoom) return;

            StartCountdown();
            LoadPlayerInScene();
            FullRoom();
        }
        void CancelWaitingRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }
        public void GameOver()
        {
            StopMovePlayer = true;

            CameraOverview.SetActive(true);
            WaitingObj.SetActive(false);
            RespawnObj.SetActive(false);
            GameOverObj.SetActive(true);

            hightScoreManager.LoadData();
        }
        void FullRoom()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //photonView.RPC("StartCountdown", RpcTarget.AllBufferedViaServer);
            }
            if (!isGameStart)
                //if (PlayerInRoom == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
                if (PlayerInRoom == 1 && PhotonNetwork.IsMasterClient)
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
            PhotonNetwork.CurrentRoom.IsOpen = false;

            if (inttime == 2)
            {
                PhotonScene.Instance.SpawnPlayer();
            }
            else if ((int)CountTime <= 0)
            {
                if (PhotonNetwork.IsMasterClient && isCountdown && !isGameStart)
                    photonView.RPC("StartGame", RpcTarget.AllBufferedViaServer);

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
            if (!PhotonNetwork.InRoom) return;

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
        public void Respawn()
        {

            StartCoroutine("Spawning");
        }
        IEnumerator Spawning()
        {
            CountdownObj.SetActive(false);
            RespawnObj.SetActive(true);
            WaitingObj.SetActive(false);
            GameOverObj.SetActive(false);
            CameraOverview.SetActive(true);

            AudioManager.instance.SoundClickTime();

            TextRespawn.text = "5";
            yield return new WaitForSecondsRealtime(1);
            AudioManager.instance.SoundClickTime();

            TextRespawn.text = "4";
            yield return new WaitForSecondsRealtime(1);
            AudioManager.instance.SoundClickTime();

            TextRespawn.text = "3";
            yield return new WaitForSecondsRealtime(1);
            AudioManager.instance.SoundClickTime();

            TextRespawn.text = "2";
            yield return new WaitForSecondsRealtime(1);
            AudioManager.instance.SoundClickTime();

            TextRespawn.text = "1";
            yield return new WaitForSecondsRealtime(1);
            TextRespawn.text = "";

            PhotonScene.Instance.SpawnPlayer(true);
            CameraOverview.SetActive(false);
            WaitingObj.SetActive(false);
            RespawnObj.SetActive(false);
            CountdownObj.SetActive(false);

        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {

            base.OnPlayerEnteredRoom(newPlayer);
            AudioManager.instance.SoundWhenPlayerJoin();


        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {

            base.OnPlayerLeftRoom(otherPlayer);
            AudioManager.instance.SoundWhenPlayerLeft();


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





