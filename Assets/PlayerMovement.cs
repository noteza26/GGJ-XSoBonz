using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.FTail;
using Photon.Pun;
using Balloon.Photon;
namespace Balloon
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] PhotonPlayerManager MyPhotonManager;
        [SerializeField] PhotonView photonView;
        [SerializeField] Transform GameCamera;
        [SerializeField] TailAnimator2 tailAnimator2;

        [Header("Player Setting")]
        [Space(10)]
        public bool CanBoost;
        public bool OnBoost;
        public float StartSpeed = 6.0F;
        public float Speed;
        public float SprintBoost = 10.0f;
        public float JumpSpeed = 8.0F;
        public float RotateSpeed = 3.0F;
        public float boostTo;

        [SerializeField] float Timer;
        [Header("World")]
        [Space(10)]
        public float Gravity = 20.0F;
        private Vector3 moveDirection = Vector3.zero;
        private void Start()
        {
            if (!photonView.IsMine) return;

            CanBoost = true;
            Speed = StartSpeed;

            if (tailAnimator2)
                //tailAnimator2.IncludedColliders.Add(this.GetComponent<Collider>());

                LoadCollisionGet();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            if (MyPhotonManager.StopMove) return;

            CharacterRotate();
            CharacterMove();
        }
        void LateUpdate()
        {
            if (!photonView.IsMine) return;

            var CharacterRotation = GameCamera.transform.rotation;
            CharacterRotation.x = 0;
            CharacterRotation.z = 0;

            transform.rotation = CharacterRotation;
        }
        void Update()
        {
            if (!photonView.IsMine) return;

            Boost();
        }
        void Boost()
        {
            if (Timer > 0)
            {

                Timer -= Time.deltaTime;

            }
            else if (Timer <= 0)
            {
                Speed = StartSpeed;
                boostTo = 0;
                Timer = 0;
                OnBoost = false;
            }
        }
        /*IEnumerator changeValueOverTime()
        {
            Debug.Log("Boost!");

            onBoost = true;

            float counter = 0f;

            while (counter < 5)
            {
                if (newBoost)
                {
                    newBoost = false;
                    onBoost = false;
                    StartCoroutine(changeValueOverTime());
                    Debug.Log("Break");
                    yield break;
                }

                counter += Time.deltaTime;

                if (counter > 0.5f)
                    canBoost = true;

                float val = Mathf.Lerp(boostTo, StartSpeed, counter / 5);
                Speed = val;
                // Debug.Log("Val: " + val);
                yield return null;


            }
            onBoost = false;
            boostTo = StartSpeed;

        }*/
        public void BoostSpeed(float speed, float timer)
        {
            OnBoost = true;
            boostTo += speed;
            Speed = boostTo;
            Timer = timer;
        }

        void LoadCollisionGet()
        {
            if (LoadCollision.instance)
                for (var i = 0; i < LoadCollision.instance.Collisions.Length; i++)
                {
                    tailAnimator2.IncludedColliders.Add(LoadCollision.instance.Collisions[i]);

                }
        }
        void CharacterRotate()
        {
            /* var moveLeftRight = Input.GetAxis("Horizontal");
             if (moveLeftRight != 0)
                 transform.Rotate(Vector3.up * moveLeftRight * RotateSpeed * Time.deltaTime, Space.Self);*/
        }
        void CharacterMove()
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                var moveForward = Input.GetAxis("Vertical");
                var moveLeftRight = Input.GetAxis("Horizontal");


                moveDirection = new Vector3(moveLeftRight, 0, moveForward);
                moveDirection = transform.TransformDirection(moveDirection);
                if (Input.GetButton("Fire3") && !OnBoost)
                    moveDirection *= SprintBoost;
                else
                    moveDirection *= Speed;


                if (Input.GetButton("Jump"))
                    moveDirection.y = JumpSpeed;

            }
            moveDirection.y -= Gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
