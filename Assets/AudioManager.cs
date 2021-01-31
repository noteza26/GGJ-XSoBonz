using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum Category
{
    Click,

}
public class AudioManager : MonoBehaviour
{
    [SerializeField] bool isMute;
    [SerializeField] Button[] MutedButton;
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
        for (int i = 0; i < MutedButton.Length; i++)
        {
            MutedButton[i].onClick.AddListener(Muted);
        }
        MutedButton[0].gameObject.SetActive(true);
        MutedButton[1].gameObject.SetActive(false);
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
    public bool CheckMute()
    {
        return isMute;
    }
    public void Muted()
    {
        if (isMute)
        {
            MutedButton[0].gameObject.SetActive(true);
            MutedButton[1].gameObject.SetActive(false);
        }
        else
        {
            MutedButton[1].gameObject.SetActive(true);
            MutedButton[0].gameObject.SetActive(false);
        }

        isMute = !isMute;
    }
}
