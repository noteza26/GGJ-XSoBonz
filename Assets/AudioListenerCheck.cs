using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerCheck : MonoBehaviour
{
    public bool isMute;
    [SerializeField] AudioListener AudioListener;

    private void Awake()
    {
        AudioListener = this.GetComponent<AudioListener>();
    }

    private void Update()
    {
        var check = AudioManager.instance.CheckMute();
        isMute = AudioManager.instance.CheckMute();

        if (isMute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }



    }
}
