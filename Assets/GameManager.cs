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
    public GameObject PlayerObject;
}
[System.Serializable]

public struct TeamA
{
    public int TeamScore;
    public List<PlayerData> PlayerTeamData;
}
[System.Serializable]

public struct TeamB
{
    public int TeamScore;
    public List<PlayerData> PlayerTeamData;


}
[System.Serializable]

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance;
    public int PlayerInRoom;

    public List<PlayerData> AllPlayerData;
    public TeamA TeamAData;
    public TeamB TeamBData;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CreateTeam();
    }
    public void ClearListTeam()
    {
        TeamAData.TeamScore = 0;
        TeamBData.TeamScore = 0;
        PlayerInRoom = 0;
        AllPlayerData.Clear();
        TeamAData.PlayerTeamData.Clear();
        TeamBData.PlayerTeamData.Clear();
    }
    public void AddDataToTeam(string team, string playerName, int playerid, GameObject playerObj)
    {
        if (team == "A")

            TeamAData.PlayerTeamData.Add(new PlayerData { PlayerName = playerName, PlayerID = playerid, PlayerObject = playerObj });

        else if (team == "B")
            TeamBData.PlayerTeamData.Add(new PlayerData { PlayerName = playerName, PlayerID = playerid, PlayerObject = playerObj });

        else
            Debug.LogError("Fuck");

        AllPlayerData.Add(new PlayerData { PlayerName = playerName, PlayerID = playerid, PlayerObject = playerObj });

    }
    public PhotonPlayerManager.TeamList LoadTeam(PhotonPlayerManager getPlayerData, string name)
    {
        for (int i = 0; i < TeamAData.PlayerTeamData.Count; i++)
        {
            if (TeamAData.PlayerTeamData[i].PlayerName == name)
            {
                Debug.Log("A");
                return PhotonPlayerManager.TeamList.A;
            }
        }
        for (int i = 0; i < TeamBData.PlayerTeamData.Count; i++)
        {
            if (TeamBData.PlayerTeamData[i].PlayerName == name)
            {
                Debug.Log("B");
                return PhotonPlayerManager.TeamList.B;
            }
        }
        Debug.Log("None");
        return PhotonPlayerManager.TeamList.None;

    }
    public PhotonPlayerManager.TeamList SetTeam(PhotonPlayerManager getPlayerData)
    {
        if (this.TeamAData.PlayerTeamData.Count <= this.TeamBData.PlayerTeamData.Count)
        {
            var team = PhotonPlayerManager.TeamList.A;
            this.AddDataToTeam("A", getPlayerData.PlayerName, getPlayerData.PlayerID, getPlayerData.gameObject);
            getPlayerData.Team = team;
            return team;
        }
        else
        {
            var team = PhotonPlayerManager.TeamList.B;
            this.AddDataToTeam("B", getPlayerData.PlayerName, getPlayerData.PlayerID, getPlayerData.gameObject);
            getPlayerData.Team = team;
            return team;
        }
    }
    public void SetPlayerObj()
    {
        var findPlayerInRoom = GameObject.FindGameObjectsWithTag("PlayerController");

        for (int i = 0; i < findPlayerInRoom.Length; i++)
        {
            var getPlayerData = findPlayerInRoom[i].GetComponent<PhotonPlayerManager>();
            if (getPlayerData)
                for (int ii = 0; ii < AllPlayerData.Count; ii++)
                {
                    if (AllPlayerData[ii].PlayerName == getPlayerData.PlayerName)
                    {
                        var obj = AllPlayerData[ii];
                        //  obj.PlayerObject = findPlayerInRoom[i];
                        obj = new PlayerData { PlayerID = 55555, PlayerName = "TestS", PlayerObject = findPlayerInRoom[i] };
                        Debug.Log(findPlayerInRoom[i].name);
                    }
                }
            else
                Debug.LogError("Cant get Player Manager");
        }
    }
    public void PlayerMove(bool canMove)
    {
        /*   for (int i = 0; i < AllPlayerData.Count; i++)
           {
               if (AllPlayerData[i].PlayerObject == null) return;

               var playerMove = AllPlayerData[i].PlayerObject.GetComponent<PhotonPlayerManager>();
               playerMove.StopMove = !canMove;
           }*/
    }
    public void LoadTeam()
    {
        for (int i = 0; i < TeamAData.PlayerTeamData.Count; i++)
        {
            var playerData = TeamAData.PlayerTeamData[i].PlayerObject.GetComponent<PhotonPlayerManager>();
            playerData.SetTeam(PhotonPlayerManager.TeamList.A);
        }

        for (int i = 0; i < TeamAData.PlayerTeamData.Count; i++)
        {
            var playerData = TeamBData.PlayerTeamData[i].PlayerObject.GetComponent<PhotonPlayerManager>();
            playerData.SetTeam(PhotonPlayerManager.TeamList.B);

        }
    }
    void CreateTeam()
    {

        if (PhotonNetwork.IsMasterClient)
            if (PhotonNetwork.CurrentRoom.PlayerCount != PlayerInRoom)
            {
                /*  Debug.Log("UpdateTeam " + PhotonNetwork.CountOfPlayers);
                  Debug.Log("UpdateTeam " + PlayerInRoom);*/
                ClearListTeam();

                photonView.RPC("TeamPun", RpcTarget.AllBufferedViaServer);
            }
    }
    [PunRPC]
    void TeamPun()
    {
        var findPlayerInRoom = GameObject.FindGameObjectsWithTag("PlayerController");
        if (findPlayerInRoom.Length == 0) return;
        if (PhotonNetwork.CurrentRoom.PlayerCount == PlayerInRoom) return;


        for (int i = 0; i < findPlayerInRoom.Length; i++)
        {
            var getPlayerData = findPlayerInRoom[i].GetComponent<PhotonPlayerManager>();
            if (getPlayerData)
            {
                if (this.TeamAData.PlayerTeamData.Count <= this.TeamBData.PlayerTeamData.Count)
                {
                    Debug.Log("ADD a");
                    this.AddDataToTeam("A", getPlayerData.PlayerName, getPlayerData.PlayerID, getPlayerData.gameObject);
                }
                else
                {
                    Debug.Log("ADD b");

                    this.AddDataToTeam("B", getPlayerData.PlayerName, getPlayerData.PlayerID, getPlayerData.gameObject);

                }
                PlayerInRoom++;

            }
        }


        /*    for (var i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                var getPlayerData = PhotonNetwork.PlayerList[i];
                if (this.TeamAData.PlayerTeamData.Count <= this.TeamBData.PlayerTeamData.Count)
                {
                    this.AddDataToTeam("A", getPlayerData.NickName, 0, null);
                }
                else
                {
                    this.AddDataToTeam("B", getPlayerData.NickName, 0, null);
                }
                PlayerInRoom++;
            }*/
        // PlayerInRoom = PhotonNetwork.PlayerList.Length;

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        base.OnPlayerEnteredRoom(newPlayer);


    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*    if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(AllPlayerData);
                stream.SendNext(TeamAData);
                stream.SendNext(TeamBData);
            }
            else if (stream.IsReading)
            {
                // Network player, receive data
                this.AllPlayerData = (List<PlayerData>)stream.ReceiveNext();
                this.TeamAData = (TeamA)stream.ReceiveNext();
                this.TeamBData = (TeamB)stream.ReceiveNext();
            }*/
    }

}
