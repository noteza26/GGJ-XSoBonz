using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Balloon.Photon;

[System.Serializable]
public struct PlayerData
{
    public string PlayerName;
    public int PlayerID;
    public int PlayerScore;
    public GameObject PlayerObject;
}
[System.Serializable]


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public int PlayerInRoom;

    public List<PlayerData> AllPlayerData;


    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayerObj();
    }

    public void SetPlayerObj()
    {
        var findPlayerInRoom = GameObject.FindGameObjectsWithTag("PlayerController");
        if (findPlayerInRoom.Length == 0) return;
        if (PlayerInRoom == PhotonNetwork.CurrentRoom.PlayerCount) return;

        for (int i = 0; i < findPlayerInRoom.Length; i++)
        {
            var getPlayerData = findPlayerInRoom[i].GetComponent<PhotonPlayerManager>();
            if (getPlayerData)
            {
                AllPlayerData.Add(new PlayerData
                {
                    PlayerID = getPlayerData.PlayerID,
                    PlayerName = getPlayerData.PlayerName,
                    PlayerObject = findPlayerInRoom[i]
                });
                PlayerInRoom++;
            }
        }
    }
    public void PlayerMove(bool canMove)
    {
        for (int i = 0; i < AllPlayerData.Count; i++)
        {
            if (AllPlayerData[i].PlayerObject == null) return;

            var playerMove = AllPlayerData[i].PlayerObject.GetComponent<PhotonPlayerManager>();
            playerMove.StopMove = !canMove;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        base.OnPlayerEnteredRoom(newPlayer);


    }


}
