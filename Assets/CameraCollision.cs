using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Balloon
{
    public class CameraCollision : MonoBehaviour
    {
        [SerializeField] float minDis;
        [SerializeField] float maxDis;
        [SerializeField] float smooth;
        [SerializeField] Transform ModelPlayer;
        Vector3 dollyDir;
        [SerializeField] Vector3 dollyDirAdjusted;
        [SerializeField] float distance;

        private void Awake()
        {
            dollyDir = transform.localPosition.normalized;
            distance = transform.localPosition.magnitude;
        }
        void Update()
        {
            Vector3 desiredCameraPos = ModelPlayer.TransformPoint(dollyDir * maxDis);

            RaycastHit hit;

            if (Physics.Linecast(transform.position, ModelPlayer.localPosition, out hit))
            {
                //distance = Mathf.Clamp((hit.distance * 0.9f), minDis, maxDis);
                this.transform.localPosition = new Vector3(0, 0 - Vector3.Distance(transform.position, hit.point));
            }
            else
            {
                this.transform.localPosition = ModelPlayer.localPosition;
                // distance = maxDis;
            }
            // transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
        }
    }
}

