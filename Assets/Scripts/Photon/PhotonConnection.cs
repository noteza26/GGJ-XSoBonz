using System.Linq;
using System.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection instance;
    public string characterPrefName;
    public Transform spawnPoint = null;

    private GameObject myCharacter = null;
    [SerializeField] private GameObject playerCamera = null;
    private List<string> roomNameList = new List<string>();
    [Serializable]
    public enum RoomState
    {
        Login,
        Register,

        None,
        Connected,
        JoinedLobby,
        JoinedRoom,
        RoleCreate,
        RoleJoin,
    }
    [Serializable]
    public class SceneSystem
    {
        public RoomState state;
        public GameObject SceneUI;
    }
    public RoomState roomState;
    public string inputRoomName;
    public string inputPlayerName;
    public int inputHighScore;
    [SerializeField] private Button createRoomBut = null;
    [SerializeField] private Button joinBut = null;
    [SerializeField] private Button regisBut = null;
    [SerializeField] private TextMeshProUGUI helloName = null;
    [SerializeField] private TextMeshProUGUI highScoreText = null;

    [SerializeField] private TextMeshProUGUI dataServer = null;
    [SerializeField] private TextMeshProUGUI dataServer2 = null;

    [SerializeField] private List<SceneSystem> SceneList = new List<SceneSystem>();

    [SerializeField] private List<Transform> spawnItem = null;
    [SerializeField] private List<Transform> spawnEnemy = null;
    [SerializeField] private List<Transform> spawnEnvironment = null;

    [SerializeField] private Transform spawnBoss = null;

    void Awake()
    {
        instance = this;
        joinBut.onClick.AddListener(JoinRoom);
        regisBut.onClick.AddListener(regisRoom);
        ShowUI();
    }
    void Start()
    {
        loadSpawnPoint();

        /*var cf = playerCamera.GetComponent<CamFollow>();
        cf.enabled = false;*/
        /*  if (PhotonNetwork.CountOfRooms == 0)
              joinBut.interactable = false;
          else
              joinBut.interactable = true;*/



    }
    void Update()
    {

        UpdateScore();
        if (Input.GetMouseButtonDown(1))
            getScore();
        /*  if (PhotonNetwork.CountOfRooms == 0)
              joinBut.interactable = false;
          else
              joinBut.interactable = true;*/
        dataServer.text = "Ping " + PhotonNetwork.GetPing().ToString();
        dataServer2.text = "Version " + Application.version;

    }
    private void loadSpawnPoint()
    {
       /* spawnItem.Clear();
        var spawnItemget = GameObject.FindGameObjectsWithTag("MeteorPoint");
        for (int i = 0; i < spawnItemget.Length; i++)
        {
            spawnItem.Add(spawnItemget[i].transform);
        }
        spawnEnemy.Clear();
        var spawnEnemyget = GameObject.FindGameObjectsWithTag("EnemyPoint");
        for (int i = 0; i < spawnEnemyget.Length; i++)
        {
            spawnEnemy.Add(spawnEnemyget[i].transform);
        }
        spawnEnvironment.Clear();
        var spawnEnvironmentget = GameObject.FindGameObjectsWithTag("EnvironmentPoint");
        for (int i = 0; i < spawnEnvironmentget.Length; i++)
        {
            spawnEnvironment.Add(spawnEnvironmentget[i].transform);
        }*/
    }
    private void createScene()
    {
        roomState = RoomState.RoleCreate;
        changeState();

    }
    private void joinScene()
    {

        roomState = RoomState.RoleJoin;
        changeState();


    }
    private void createRoom()
    {
        roomState = RoomState.Connected;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, options, TypedLobby.Default);
        changeState();

    }
    public void JoinRoom()
    {
        roomState = RoomState.Connected;
        PhotonNetwork.JoinRandomRoom();

        changeState();


    }

    public void JoinLobby()
    {
        roomState = RoomState.JoinedLobby;
        PhotonNetwork.JoinLobby();
        changeState();
    }

    private void regisRoom()
    {
        roomState = RoomState.Register;

        changeState();


    }
    public void RegisterSuccess()
    {
        roomState = RoomState.Login;

        changeState();
    }
    private void OnGUI()
    {
        /*    switch (roomState)
            {
                case RoomState.JoinedLobby:
                    {
                        foreach (var roomName in roomNameList)
                        {
                            if (GUILayout.Button(roomName))
                            {
                                PhotonNetwork.JoinRoom(roomName);
                            }
                        }

                        ShowUI();
                        Debug.Log("Joinned lobby");
                        break;
                    }
            }*/
    }
    private void changeState()
    {
        switch (roomState)
        {

            case RoomState.JoinedLobby:
                {

                    ShowUI();
                    Debug.Log("Joinned lobby");
                    break;
                }
            case RoomState.Connected:
                {
                    ShowUI();
                    break;
                }
            case RoomState.Register:
                {
                    ShowUI();
                    break;
                }
            case RoomState.Login:
                {
                    ShowUI();
                    break;
                }
            case RoomState.RoleCreate:
                {
                    ShowUI();
                    break;
                }
            case RoomState.RoleJoin:
                {

                    break;
                }
            case RoomState.JoinedRoom:
                {

                    ShowUI();


                    break;
                }
            default:
                {

                    break;
                }
        }
    }
    private void ShowUI()
    {

        for (int i = 0; i < SceneList.Count; i++)
        {
            SceneList[i].SceneUI.SetActive(false);

            if (roomState == SceneList[i].state)
            {
                SceneList[i].SceneUI.SetActive(true);

            }
        }
    }
    private void spawnSystem()
    {
        PhotonNetwork.InstantiateSceneObject("Boss", spawnBoss.transform.position, spawnBoss.transform.rotation);

        int ran = UnityEngine.Random.Range(0, (PhotonNetwork.CountOfPlayersInRooms * 2) + (spawnEnemy.Count / 2));
        for (int i = 0; i < ran; i++)
        {
            PhotonNetwork.InstantiateSceneObject("EnemyController", spawnEnemy[i].transform.position, spawnEnemy[i].transform.rotation);

        }
        //   int ran2 = UnityEngine.Random.Range(0, spawnEnvironment.Count);
        for (int i = 0; i < spawnEnvironment.Count; i++)
        {
            int ranspawn = UnityEngine.Random.Range(0, 10);
            if (ranspawn == 0 || ranspawn == 2 || ranspawn == 4 || ranspawn == 8)
            {
                int ranitem = UnityEngine.Random.Range(0, 3);
                if (ranitem == 1)
                {
                    PhotonNetwork.InstantiateSceneObject("envir1", spawnEnvironment[i].transform.position, spawnEnvironment[i].transform.rotation);
                }
                else if (ranitem == 2)
                {
                    PhotonNetwork.InstantiateSceneObject("envir2", spawnEnvironment[i].transform.position, spawnEnvironment[i].transform.rotation);
                }
                else if (ranitem == 3)
                {
                    PhotonNetwork.InstantiateSceneObject("envir3", spawnEnvironment[i].transform.position, spawnEnvironment[i].transform.rotation);
                }
            }
        }
    }
    private void getScore()
    {
        //API.instance.GetScore(inputPlayerName);
        //        Debug.Log("Invoke");
    }
    public void UpdateScore()
    {
        highScoreText.text = inputHighScore.ToString("#,#");
    }

    IEnumerator Connect()
    {
        ShowUI();

        PhotonNetwork.ConnectUsingSettings();

        while (PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
        {
            yield return null;
        }

        roomState = RoomState.Connected;

        PhotonNetwork.JoinLobby();

        changeState();

        Debug.Log("Connected Server ");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("OnJoinedLobby");

        roomState = RoomState.JoinedLobby;

        changeState();

        helloName.text = "Hi, " + inputPlayerName;
        highScoreText.text = "" + inputHighScore.ToString("#,#");

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

        roomState = RoomState.JoinedRoom;
        changeState();

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master Client");
            int ranposi = 0;
            spawnSystem();

            for (int i = 0; i < spawnItem.Count; i++)
            {
                if (ranposi == 0)
                {
                    PhotonNetwork.InstantiateSceneObject("Meteor-LeveLUP", spawnItem[i].transform.position, spawnItem[i].transform.rotation);
                    ranposi++;

                }
                else
                {
                    PhotonNetwork.InstantiateSceneObject("Meteor-HealUP", spawnItem[i].transform.position, spawnItem[i].transform.rotation);
                    ranposi--;

                }
            }
            spawnItem.Clear();

        }
        InstantSystem();
    }
    public Transform getspawnposi()
    {
        return spawnPoint;
    }
    public void InstantSystem()
    {
        myCharacter = PhotonNetwork.Instantiate(characterPrefName, spawnPoint.position, spawnPoint.rotation);

        // var myCharacterMove = myCharacter.GetComponent<PlayerMovent>();
        var setPlayerName = myCharacter.GetComponent<PhotonView>();

        /*   myCharacterMove.CanControl(true);
           myCharacterMove.SetPlayerName(inputPlayerName, inputHighScore);
           setPlayerName.Owner.NickName = inputPlayerName;*/

        var playerCam = myCharacter;


        // var cf = playerCamera.GetComponent<CamFollow>();

        /*  cf.enabled = true;
          cf.Following(playerCam.transform);*/
        playerCamera.SetActive(true);
        myCharacter.name = inputPlayerName;


    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)

    {
        base.OnRoomListUpdate(roomList);

        roomNameList.Clear();

        for (int i = 0; i < roomList.Count; i++)
        {
            roomNameList.Add(roomList[i].Name);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }
    public void LoginSuccess(string playername, int highScore)
    {

        inputPlayerName = playername;
        inputHighScore = highScore;
        StartCoroutine(Connect());
    }
}
