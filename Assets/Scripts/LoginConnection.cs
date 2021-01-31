using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Balloon.Photon;

namespace Balloon
{
    public class LoginConnection : MonoBehaviour
    {
        public string PlayerName;

        [SerializeField] int CharacterLimit;
        [SerializeField] TextMeshProUGUI errorText;
        [SerializeField] TMP_InputField inputName;
        [SerializeField] Button submitName;


        private void Start()
        {
            inputName.characterLimit = CharacterLimit;
            submitName.onClick.AddListener(SubmitName);
        }

        void SubmitName()
        {
            if (inputName.text == "")
            {
                errorText.text = "please input name";
            }
            else
            {
                CheckData();
                PlayerName = inputName.text.ToString();
                SuccessCreateName();
            }
        }
        void SuccessCreateName()
        {
            PhotonConnector.instance.LoginSuccess(PlayerName);

        }
        void CheckData()
        {
            //// TO DO USER PASS LOGIN CHECK
        }
    }
}

