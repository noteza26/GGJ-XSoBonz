using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Balloon.Photon;

[System.Serializable]
public class PlayerData
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


    public List<PlayerData> AllPlayerData = new List<PlayerData>();

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
                    photonView.RPC("Gameover", RpcTarget.AllBufferedViaServer);

                    /*

                        Create Scoreboard Scene to show score


                    */
                }
            }
        }
    }
    [PunRPC]
    void Gameover()
    {
        TimerInGame = 0;
        PhotonInGameManager.instance.GameOver();
        isStart = false;
    }
    public void BoardKilled(string hurtby, string hurtto)
    {
        photonView.RPC("BoardKilledRPC", RpcTarget.AllBufferedViaServer, hurtby, hurtto);
    }
    [PunRPC]
    void BoardKilledRPC(string hurtby, string hurtto)
    {
        Debug.Log("Player " + hurtto + " has killed By " + hurtby);
        if (hurtby == hurtto)
        {

            for (int i = 0; i < AllPlayerData.Count; i++)
            {
                if (hurtby == AllPlayerData[i].PlayerName)
                {
                    var playerScore = AllPlayerData[i].PlayerScore;
                    if (playerScore > 0)
                    {
                        //  photonView.RPC("DeleteScore", RpcTarget.AllBufferedViaServer, i);

                        playerScore -= ScoreDelete;
                        Debug.Log("Delete " + ScoreDelete + " Total" + playerScore + " Name " + AllPlayerData[i].PlayerName);
                    }

                }
            }
        }
        else
        {

            for (int i = 0; i < AllPlayerData.Count; i++)
            {
                if (hurtby == AllPlayerData[i].PlayerName)
                {
                    //photonView.RPC("AddScore", RpcTarget.AllBufferedViaServer, i);
                    AllPlayerData[i].PlayerScore += ScoreAdd;
                    Debug.Log("Add " + ScoreAdd + " Total" + AllPlayerData[i].PlayerScore + " Name " + AllPlayerData[i].PlayerName);

                }
            }
        }
    }
    public void UpdatePlayerRespawn(string name, GameObject playerObj)
    {
        foreach (var item in AllPlayerData)
        {
            if (item.PlayerName == name)
                item.PlayerObject = playerObj;
        }
    }
    public void SetPlayerObj()
    {
        if (PhotonInGameManager.instance.isGameStart) return;

        var findPlayerInRoom = GameObject.FindGameObjectsWithTag("PlayerController");
        Debug.Log(findPlayerInRoom.Length);

        if (findPlayerInRoom.Length == 0) return;

        if (PlayerInRoom == findPlayerInRoom.Length) return;

        AllPlayerData.Clear();

        for (int i = 0; i < findPlayerInRoom.Length; i++)
        {
            var getPlayerData = findPlayerInRoom[i].GetComponent<PhotonPlayerManager>();
            if (getPlayerData)
            {

                AllPlayerData.Add(new PlayerData
                {
                    PlayerID = getPlayerData.PlayerID,
                    PlayerName = getPlayerData.PlayerName,
                    PlayerScore = 0,
                    PlayerObject = findPlayerInRoom[i]
                });

                Debug.Log("Add playerData " + getPlayerData.PlayerName);
            }
        }
        PlayerInRoom = findPlayerInRoom.Length;
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
            //    stream.SendNext(AllPlayerData);

        }
        else
        {
            // Network player, receive data
            this.TimerInGame = (float)stream.ReceiveNext();
            this.isStart = (bool)stream.ReceiveNext();
            //   this.AllPlayerData = (List<PlayerData>)stream.ReceiveNext();
        }
    }


}
