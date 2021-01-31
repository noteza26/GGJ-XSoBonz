using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Balloon;
using Balloon.Photon;

[System.Serializable]
public enum LobbyStateRoom
{
    Lobby,

    onCreateRoom,
    JoinedRoom,
    SearchingRoom,
}
[System.Serializable]
public class SceneSystem
{
    public LobbyStateRoom state;
    public GameObject SceneUI;
}
public class PhotonLobby : MonoBehaviour
{
    public static PhotonLobby instance;

    [Space(10)]
    [Header("Server State")]
    public LobbyStateRoom LobbyStateRoom;

    [Header("Config")]
    [Space(10)]
    [SerializeField] private List<SceneSystem> SceneList = new List<SceneSystem>();

    [SerializeField] TextMeshProUGUI textOnSearch;

    // Start is called before the first frame update
    void Start()
    {
        ChangeRoomState(LobbyStateRoom.Lobby);

        if (PhotonMainMenu.instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeText(string data)
    {
        textOnSearch.text = data;
    }
    public void ChangeRoomState(LobbyStateRoom newState)
    {
        LobbyStateRoom = newState;
        ChangeUI();
    }

    void ChangeUI()
    {

        for (int i = 0; i < SceneList.Count; i++)
        {
            SceneList[i].SceneUI.SetActive(false);

            if (LobbyStateRoom == SceneList[i].state)
            {
                SceneList[i].SceneUI.SetActive(true);
                
            }
        }

    }
}

