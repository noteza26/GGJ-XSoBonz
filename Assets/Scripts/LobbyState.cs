using System.Linq;
using System.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Balloon.Photon;

namespace Balloon
{
    public class LobbyState : MonoBehaviourPunCallbacks
    {
        public static LobbyState instance;
        [SerializeField] TextMeshProUGUI nameShow;

        [SerializeField] Button ReadyButton;
        string playerName;
        void Start()
        {
            instance = this;
            nameShow.text = "";

            var getPhoton = PhotonConnector.instance;
            if (getPhoton)
            {
                LoadPlayerName(getPhoton.PlayerName);
                getPhoton.InitButton(ReadyButton);
            }
            else
                SceneManager.LoadSceneAsync("Mainmenu");

        }

        public void LoadPlayerName(string PlayerName)
        {
            if (PlayerName != playerName)
            {
                playerName = PlayerName;
                nameShow.text = "Hi, " + PlayerName;
                OnJoinedLobby();
            }
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();

            Debug.Log("OnJoinedLobby");

            /* roomState = RoomState.JoinedLobby;

             changeState();

             helloName.text = "Hi, " + inputPlayerName;
             highScoreText.text = "" + inputHighScore.ToString("#,#");*/

        }
    }
}

