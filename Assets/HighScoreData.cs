using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPlayername;
    [SerializeField] TextMeshProUGUI textScore;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetData(string playername, int score)
    {
        textPlayername.text = playername.ToString();
        textScore.text = score.ToString("");
    }
}
