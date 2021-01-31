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
    [SerializeField] GameObject prefSound;
    public static AudioManager instance;
    public AudioSource audioSource;

    public AudioClip WhenPlayerJoin;
    public AudioClip WhenPlayerLeft;

    [Header("Shoot")]
    public AudioClip Shooting;
    public AudioClip JumpingSound;

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
    public void SoundShoot(Transform trans)
    {

        var audioNew = Instantiate(prefSound, trans.position, Quaternion.identity);
        var audioNewSoure = audioNew.GetComponent<AudioSource>();
        audioNewSoure.volume = 0.5f;
        audioNewSoure.PlayOneShot(Shooting);

        Destroy(audioNew, Shooting.length);
    }
    public void SoundJump(Transform trans)
    {

        var audioNew = Instantiate(prefSound, trans.position, Quaternion.identity);
        var audioNewSoure = audioNew.GetComponent<AudioSource>();
        audioNewSoure.volume = 0.5f;
        audioNewSoure.PlayOneShot(JumpingSound);

        Destroy(audioNew, JumpingSound.length);
    }
}
