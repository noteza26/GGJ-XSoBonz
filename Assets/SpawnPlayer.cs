using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
namespace Balloon
{
    public class SpawnPlayer : MonoBehaviour
    {
        public static SpawnPlayer instance;
        private void Awake()
        {
            instance = this;
        }
        public void InstantPlayer()
        {
            PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), new Quaternion());
        }

    }
}

