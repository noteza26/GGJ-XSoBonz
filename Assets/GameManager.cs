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


public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance;
    public bool isStart;
    public int PlayerInRoom;

    public int ScoreAdd;
    public int ScoreDelete;

    public List<PlayerData> AllPlayerData;

    public float TimerInGame;
    private void Awake()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        SetPlayerObj();
        CountdownTimer();
    }
    void CountdownTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isStart)
            {
                TimerInGame -= Time.deltaTime;
                if (TimerInGame <= 0)
                {
                    TimerInGame = 0;
                    isStart = false;
                    /*

                        Create Scoreboard Scene to show score


                    */
                }
            }
        }
    }
    public void BoardKilled(string hurtby, string hurtto)
    {
        Debug.Log("Player " + hurtto + "has killed By" + hurtby);
        if (hurtby == hurtto)
        {
            for (int i = 0; i < AllPlayerData.Count; i++)
            {
                if (hurtby == AllPlayerData[i].PlayerName)
                {
                    var playerScore = AllPlayerData[i].PlayerScore;
                    if (playerScore > 0)
                        playerScore -= ScoreDelete;

                    Debug.Log("Delete " + ScoreDelete + " Total" + playerScore);
                }
            }
        }
        else
        {
            for (int i = 0; i < AllPlayerData.Count; i++)
            {
                if (hurtby == AllPlayerData[i].PlayerName)
                {
                    var playerScore = AllPlayerData[i].PlayerScore;
                    playerScore += ScoreAdd;

                    Debug.Log("Add " + ScoreAdd + " Total" + playerScore);
                }
            }
        }
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
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(TimerInGame);
            stream.SendNext(isStart);
            stream.SendNext(AllPlayerData);

        }
        else
        {
            // Network player, receive data
            this.TimerInGame = (float)stream.ReceiveNext();
            this.isStart = (bool)stream.ReceiveNext();
            this.AllPlayerData = (List<PlayerData>)stream.ReceiveNext();
        }
    }


}
