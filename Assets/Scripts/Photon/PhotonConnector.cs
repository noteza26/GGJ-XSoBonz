using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Balloon.Photon
{

    [Serializable]
    public enum ServerState
    {
        None,
        Connected,
        LostConnect,
    }


    public class PhotonConnector : MonoBehaviourPunCallbacks
    {
        public static PhotonConnector instance;
        [Header("Player")]
        public string PlayerName;
        public GameObject PlayerModel;
        [Space(10)]
        [Header("Server Side")]
        public ServerState ServerState;
        public int MaxPlayers = 4;

        [Header("Button")]
        [SerializeField] private Button readyButMain = null;

        private void Awake()
        {
            DontDestroyOnLoad(this);


        }
        // Start is called before the first frame update
        void Start()
        {
            if (PhotonConnector.instance == null)
                instance = this;
            else
                Destroy(this);

            ServerState = ServerState.None;
            ConnectToServer();
        }

        public void LoginSuccess(string playername)
        {

            PhotonNetwork.NickName = playername;
            PlayerName = playername;
            //LobbyState.instance.LoadPlayerName(playername);

            PhotonMainMenu.instance.ChangeRoomState(RoomState.Lobby);


        }
        void ConnectToServer()
        {
            if (ServerState == ServerState.None)
                StartCoroutine(Connect());
            else
                Debug.LogWarning("Already Connect to server");
        }

        public void InitButton(Button readyBut)
        {
            // createRoomBut.onClick.AddListener(createRoom);
            //createRoomBut.onClick.AddListener(JoinRoom);

            readyButMain = readyBut;

            readyButMain.onClick.AddListener(JoinRoom);
        }


        void ChangeServerState(ServerState newState)
        {
            if (ServerState != newState)
                ServerState = newState;
        }

        #region Photon
        private void createRoom()
        {
            PhotonLobby.instance.LobbyStateRoom = LobbyStateRoom.onCreateRoom;
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = (byte)MaxPlayers;
            PhotonNetwork.CreateRoom(null, options, TypedLobby.Default);

        }

        public void JoinRoom()
        {
             PhotonLobby.instance.ChangeRoomState(LobbyStateRoom.JoinedRoom);

            // roomState = RoomState.Connected;
            PhotonNetwork.JoinRandomRoom();


            //   changeState();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            createRoom();
        }

        public override void OnJoinedRoom()
        {


            base.OnJoinedRoom();

            Debug.Log("OnJoinedRoom");

            // PhotonMainMenu.instance.ChangeRoomState(RoomState.JoinedRoom);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Master Client");
            }

            PhotonNetwork.NickName = PlayerName;

            PhotonNetwork.LocalPlayer.NickName = PlayerName;



            InitGameServer();

        }

        #endregion

        void InitGameServer()
        {
            PhotonNetwork.LoadLevel("GamePlay");

            Debug.Log("Load Scene Gameplay ");



        }

        IEnumerator LoadGameScene()
        {
            var random = UnityEngine.Random.Range(3, 7);

            PhotonLobby.instance.ChangeRoomState(LobbyStateRoom.SearchingRoom);

            yield return new WaitForSecondsRealtime(random);

            PhotonNetwork.LoadLevel("GamePlay");

            PhotonLobby.instance.ChangeRoomState(LobbyStateRoom.JoinedRoom);

            yield return null;
        }











        IEnumerator Connect()
        {
            //  ShowUI();

            PhotonNetwork.ConnectUsingSettings();

            while (PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
            {
                yield return null;
            }

            //  roomState = RoomState.Connected;

            PhotonNetwork.JoinLobby();

            ChangeServerState(ServerState.Connected);

            PhotonMainMenu.instance.ChangeRoomState(RoomState.Login);

            Debug.Log("Connected Server ");
        }

    }
}

