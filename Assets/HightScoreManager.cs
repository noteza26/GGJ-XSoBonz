using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Balloon.Photon;
public class HightScoreManager : MonoBehaviour
{
    [SerializeField] HighScoreData[] highScoreDatas;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadData()
    {
        var data = GameManager.instance.AllPlayerData;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i] == null)
            {
                highScoreDatas[i].SetData("null", 0);
            }
            else
                highScoreDatas[i].SetData(data[i].PlayerName, data[i].PlayerScore);
        }
    }
}
