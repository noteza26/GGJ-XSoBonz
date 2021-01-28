using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailMoveOnLoading : MonoBehaviour
{
    [SerializeField] GameObject tail1;
    [SerializeField] GameObject tail2;
    [SerializeField] float Timer;


    private void Awake()
    {
        InvokeRepeating("TailMove", 0, Timer);

    }
    private void OnEnable()
    {
        //LeanTween.scale(tweenText, new Vector3(1, 1, 1), DelayText).setEase(typeTweenText);
    }//
    // Update is called once per frame
    void Update()
    {
    }
    void TailMove()
    {
        if (tail1.activeInHierarchy == false)
        {
            tail1.SetActive(true);
            tail2.SetActive(false);
        }
        else
        {
            tail1.SetActive(false);
            tail2.SetActive(true);
        }

    }
}
