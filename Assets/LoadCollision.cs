using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Balloon
{
    public class LoadCollision : MonoBehaviour
    {
        public static LoadCollision instance;
        public Collider[] Collisions;
        private void Start()
        {
            instance = this;
        }
    }
}

