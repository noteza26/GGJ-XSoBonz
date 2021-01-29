using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        onCreateRoom,
        JoinedRoom,
        SearchingRoom,
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

        [SerializeField] TextMeshProUGUI textOnSearch;

        // Start is called before the first frame update
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
        public void ChangeText(string data)
        {
            textOnSearch.text = data;
        }
        public void ChangeRoomState(RoomState newState)
        {
            RoomState = newState;
            ChangeUI();
        }

        void ChangeUI()
        {

            for (int i = 0; i < SceneList.Count; i++)
            {
                SceneList[i].SceneUI.SetActive(false);

                if (RoomState == SceneList[i].state)
                {
                    SceneList[i].SceneUI.SetActive(true);

                }
            }
            if (RoomState == RoomState.Lobby)
                LobbyState.instance.LoadPlayerName(PhotonConnector.instance.PlayerName);
        }
    }
}


