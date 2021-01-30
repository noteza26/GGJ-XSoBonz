
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using TMPro;

namespace Balloon.Photon
{
    public class PhotonPlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public enum TeamList
        {
            None,
            A,
            B
        }
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public TeamList Team;
        public string PlayerName;
        public int PlayerID;

        public bool StopMove;
        [SerializeField] TextMeshProUGUI textPlayerName;

        // Start is called before the first frame update

        void Awake()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }

        }

        // Update is called once per frame
        void Update()
        {

            if (textPlayerName.text == null || textPlayerName.text == "")
            {
                textPlayerName.text = PlayerName.ToString() + " : " + Team.ToString();
                SetColorText();
            }
        }
        public void SetPlayer(string playername)
        {
            if (playername == null)
                Debug.LogError("NULL NAME");

            PlayerID = photonView.ViewID;
            PlayerName = playername;
        }
        /*   public void LoadTeam()
           {
               SetTeam();
           }*/

        public void SetTeam(TeamList team)
        {
            //Debug.Log(GameManager.instance.SetTeam(this));
            Team = team;

            textPlayerName.text = PlayerName.ToString() + " : " + Team.ToString();

            SetColorText();
        }
        void SetColorText()
        {
            if (Team == TeamList.A)
                textPlayerName.color = Color.green;

            else if (Team == TeamList.B)
                textPlayerName.color = Color.red;
        }
        public string GetPlayername()
        {
            return PlayerName;
        }
        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(PlayerName);
                stream.SendNext(PlayerID);
                stream.SendNext(Team);
            }
            else
            {
                // Network player, receive data
                this.PlayerName = (string)stream.ReceiveNext();
                this.PlayerID = (int)stream.ReceiveNext();
                this.Team = (TeamList)stream.ReceiveNext();
            }
        }

        #endregion
    }
}

