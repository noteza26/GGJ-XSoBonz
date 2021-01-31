using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Balloon.Photon
{
    public class PhotonScene : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        static public PhotonScene Instance;

        #endregion

        #region Private Fields

        private GameObject instance;

        [Tooltip("The prefab to use for representing the player")]
        [SerializeField] private GameObject playerPrefab;

        [Header("AI Zone")]
        [SerializeField] GameObject AIPrefab;
        [SerializeField] List<Transform> AIspawnPosition;

        [SerializeField] List<Transform> spawnPosition;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Awake()
        {
            AIspawnPosition = spawnPosition;

            Instance = this;

            // in case we started this demo with the wrong scene being active, simply load the menu scene
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Mainmenu");

                return;
            }
        }
        public void SpawnPlayer(bool respawn)
        {
            if (playerPrefab == null)
            { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

                Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {


                if (PhotonPlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    //var randomPosi = Random.Range(0, spawnPosition.Count);
                    var randomPosi = Random.Range(0, 0);

                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    var playerObj = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPosition[randomPosi].position, Quaternion.identity, 0);

                    spawnPosition.RemoveAt(randomPosi);

                    var name = PhotonConnector.instance.PlayerName;
                    var pv = playerObj.GetComponent<PhotonView>();

                    pv.name = name;
                    PhotonNetwork.NickName = name;
                    playerObj.name = name;

                    if (PhotonConnector.instance)
                        PhotonConnector.instance.PlayerModel = playerObj;
                    else
                        Debug.LogError("Photonconnector LOST");


                    var playerManager = playerObj.GetComponent<PhotonPlayerManager>();
                    var gamemanager = GameManager.instance;

                    if (playerManager && PhotonConnector.instance)
                        if (gamemanager)
                        {

                            playerManager.SetPlayer(PhotonConnector.instance.PlayerName);

                        }
                        else
                            Debug.LogError("Can't get GameManager");
                    else
                        Debug.LogError("Cant find playerManager or PhotonConnector");

                    if (respawn)

                        GameManager.instance.UpdatePlayerRespawn(name, playerObj);

                    if (PhotonNetwork.IsMasterClient)
                        SpawnAI();

                }
                else
                {

                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }


            }

        }

        public void SpawnPlayer()
        {
            if (playerPrefab == null)
            { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

                Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {


                if (PhotonPlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    //var randomPosi = Random.Range(0, spawnPosition.Count);
                    var randomPosi = Random.Range(0, 0);

                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    var playerObj = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPosition[randomPosi].position, Quaternion.identity, 0);

                    spawnPosition.RemoveAt(randomPosi);

                    var name = PhotonConnector.instance.PlayerName;
                    var pv = playerObj.GetComponent<PhotonView>();

                    pv.name = name;
                    PhotonNetwork.NickName = name;
                    playerObj.name = name;

                    if (PhotonConnector.instance)
                        PhotonConnector.instance.PlayerModel = playerObj;
                    else
                        Debug.LogError("Photonconnector LOST");


                    var playerManager = playerObj.GetComponent<PhotonPlayerManager>();
                    var gamemanager = GameManager.instance;

                    if (playerManager && PhotonConnector.instance)
                        if (gamemanager)
                        {

                            playerManager.SetPlayer(PhotonConnector.instance.PlayerName);

                        }
                        else
                            Debug.LogError("Can't get GameManager");
                    else
                        Debug.LogError("Cant find playerManager or PhotonConnector");

                    if (PhotonNetwork.IsMasterClient)
                        SpawnAI();
                }
                else
                {

                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }


            }

        }


        void SpawnAI()
        {
            var spawnVal = Random.Range(5, 10);
            for (int i = 0; i < spawnVal; i++)
            {
                var ranSpawn = Random.Range(0, spawnPosition.Count);
                var AIObj = PhotonNetwork.Instantiate(AIPrefab.name, spawnPosition[ranSpawn].position, Quaternion.identity, 0);
                //   spawnPosition.RemoveAt(ranSpawn);

            }

        }
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            // "back" button of phone equals "Escape". quit app if that's pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //     QuitApplication();
            }
        }

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when a Photon Player got connected. We need to then load a bigger scene.
        /// </summary>
        /// <param name="other">Other.</param>
        public override void OnPlayerEnteredRoom(Player other)
        {
            /*  Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

              if (PhotonNetwork.IsMasterClient)
              {
                  Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                  // LoadArena();
              }*/
        }

        /// <summary>
        /// Called when a Photon Player got disconnected. We need to load a smaller scene.
        /// </summary>
        /// <param name="other">Other.</param>
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                // LoadArena();
            }
        }

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("PunBasics-Launcher");
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

            // PhotonNetwork.LoadLevel("GamePlay");
        }

        #endregion


    }
}

