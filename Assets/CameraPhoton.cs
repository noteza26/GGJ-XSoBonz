using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Balloon
{
    public class CameraPhoton : MonoBehaviour
    {
        [SerializeField] PhotonView photonView;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!photonView.IsMine)
            {
                this.gameObject.GetComponent<Camera>().enabled = false;
                this.gameObject.GetComponent<AudioListener>().enabled = false;
            }
        }
    }
}

