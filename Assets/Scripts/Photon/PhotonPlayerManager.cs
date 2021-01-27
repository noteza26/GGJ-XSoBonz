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
        public static PhotonPlayerManager instance;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        [SerializeField] string PlayerName;
        [SerializeField] TextMeshProUGUI textPlayerName;

        // Start is called before the first frame update

        void Awake()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }

        }
        void Start()
        {
            textPlayerName.text = PlayerName.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (textPlayerName.text == null || textPlayerName.text == "")
                textPlayerName.text = PlayerName.ToString();
        }
        public void SetPlayerName(string playername)
        {
            if (playername == null)
                Debug.LogError("NULL NAME");
            PlayerName = playername;
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
            }
            else
            {
                // Network player, receive data
                this.PlayerName = (string)stream.ReceiveNext();
            }
        }

        #endregion
    }
}

