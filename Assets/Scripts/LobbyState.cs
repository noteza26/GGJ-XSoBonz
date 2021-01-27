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

namespace Balloon
{
    public class LobbyState : MonoBehaviourPunCallbacks
    {
        public static LobbyState instance;
        [SerializeField] TextMeshProUGUI nameShow;
        string playerName;
        void Start()
        {
            instance = this;
            nameShow.text = "";
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

