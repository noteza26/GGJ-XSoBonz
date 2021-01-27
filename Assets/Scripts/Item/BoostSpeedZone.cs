using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Balloon.Item
{
    public class BoostSpeedZone : MonoBehaviour
    {
        [SerializeField] float BoostTo;
        [SerializeField] float Timer;
        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerMovement>();
            if (player)
            {
                //Debug.Log("Player");
                if (!player.CanBoost) return;
                player.BoostSpeed(BoostTo, Timer);
            }
            else
            {
                Debug.Log(other.name);
            }

        }
    }
}

