using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Category
{
    Click,

}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;

    public AudioClip WhenPlayerJoin;
    public AudioClip WhenPlayerLeft;
    public AudioClip ClickTime;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SoundWhenPlayerJoin()
    {
        audioSource.PlayOneShot(WhenPlayerJoin);
    }
    public void SoundWhenPlayerLeft()
    {
        audioSource.PlayOneShot(WhenPlayerLeft);
    }
    public void SoundClickTime()
    {
        audioSource.PlayOneShot(ClickTime);

    }
}
