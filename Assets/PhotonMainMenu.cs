using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

namespace Balloon.Photon
{
    [Serializable]
    public enum RoomState
    {
        Login,
        Register,

        None,
        Lobby,
    }
    [Serializable]
    public class SceneSystem
    {
        public RoomState state;
        public GameObject SceneUI;
    }
    public class PhotonMainMenu : MonoBehaviour
    {
        public static PhotonMainMenu instance;

        [Space(10)]
        [Header("Server State")]
        public RoomState RoomState;

        [Header("Config")]
        [Space(10)]
        [SerializeField] private List<SceneSystem> SceneList = new List<SceneSystem>();

        void Start()
        {
            ChangeRoomState(RoomState.None);

            if (PhotonMainMenu.instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeRoomState(RoomState newState)
        {
            RoomState = newState;
            ChangeUI();
        }

        void ChangeUI()
        {
            if (RoomState == RoomState.Lobby)
            {
                SceneManager.LoadSceneAsync("Lobby");
            }
            else
                for (int i = 0; i < SceneList.Count; i++)
                {
                    SceneList[i].SceneUI.SetActive(false);

                    if (RoomState == SceneList[i].state)
                    {
                        SceneList[i].SceneUI.SetActive(true);
                        /* if (SceneList[i].state == RoomState.Lobby)
                         {
                             SceneManager.LoadSceneAsync("Lobby");
                         }*/
                    }
                }
            /*  if (RoomState == RoomState.Lobby)
                  LobbyState.instance.LoadPlayerName(PhotonConnector.instance.PlayerName);*/
        }
    }
}


