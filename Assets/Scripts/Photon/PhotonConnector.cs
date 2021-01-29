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
        [SerializeField] private Button createRoomBut = null;
        [SerializeField] private Button joinRoomBut = null;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (PhotonConnector.instance == null)
                instance = this;
            else
                Destroy(this);
        }
        // Start is called before the first frame update
        void Start()
        {

            ServerState = ServerState.None;
            InitButton();
            ConnectToServer();
        }

        public void LoginSuccess(string playername)
        {
            PlayerName = playername;
            LobbyState.instance.LoadPlayerName(playername);

            PhotonMainMenu.instance.ChangeRoomState(RoomState.Lobby);
        }
        void ConnectToServer()
        {
            if (ServerState == ServerState.None)
                StartCoroutine(Connect());
            else
                Debug.LogWarning("Already Connect to server");
        }

        void InitButton()
        {
            // createRoomBut.onClick.AddListener(createRoom);
            //createRoomBut.onClick.AddListener(JoinRoom);
            joinRoomBut.onClick.AddListener(JoinRoom);
        }


        void ChangeServerState(ServerState newState)
        {
            if (ServerState != newState)
                ServerState = newState;
        }

        #region Photon
        private void createRoom()
        {
            PhotonMainMenu.instance.RoomState = RoomState.onCreateRoom;
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = (byte)MaxPlayers;
            PhotonNetwork.CreateRoom(null, options, TypedLobby.Default);

        }

        public void JoinRoom()
        {

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

            InitGameServer();

        }

        #endregion

        void InitGameServer()
        {
            PhotonNetwork.LoadLevel("GamePlay");

            //  StartCoroutine("LoadGameScene");
            //var obj = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), new Quaternion());


        }

        IEnumerator LoadGameScene()
        {
            var random = UnityEngine.Random.Range(3, 7);

            PhotonMainMenu.instance.ChangeRoomState(RoomState.SearchingRoom);

            yield return new WaitForSecondsRealtime(random);

            PhotonNetwork.LoadLevel("GamePlay");

            PhotonMainMenu.instance.ChangeRoomState(RoomState.JoinedRoom);

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

