using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Balloon.Photon
{
    public class PhotonLog : MonoBehaviour
    {
        [Space(10)]
        [Header("Log")]
        [SerializeField] private TextMeshProUGUI pingServer = null;
        [SerializeField] private TextMeshProUGUI versionGame = null;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            LogUpdate();
        }
        void LogUpdate()
        {
            if (pingServer)
            {
                if (PhotonNetwork.IsConnected)
                    pingServer.text = "Ping " + PhotonNetwork.GetPing().ToString();
                else
                    pingServer.text = "Ping 0";
            }

            if (versionGame)
                versionGame.text = "Version " + Application.version;
        }
    }
}

