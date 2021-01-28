using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SearchTween : MonoBehaviour
{
    [Header("Text")]
    public GameObject textTween;
    public LeanTweenType typeTweenText;
    public float SpeedRoate;
    // Start is called before the first frame update
    private void Awake()
    {
        this.transform.localScale = new Vector3(0, 0, 0);

    }
    private void OnEnable()
    {
        LeanTween.scale(textTween, new Vector3(1, 1, 1), SpeedRoate).setEase(typeTweenText);
    }
}

// Update is called once per frame
