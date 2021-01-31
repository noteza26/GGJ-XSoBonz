using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    Button button;
    [SerializeField] bool isNewAudio;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip soundClick;
    // Start is called before the first frame update
    private void Awake()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(playClip);
    }
    public void playClip()
    {
        if (audioSource == null)
        {
            var newObj = new GameObject();

            var audioNew = Instantiate(newObj, new Vector3(0, 0, 0), Quaternion.identity);
            var audioNewSource = audioNew.AddComponent<AudioSource>();
            audioSource = audioNewSource;
            isNewAudio = true;
        }
        audioSource.clip = soundClick;
        audioSource.Play();

        if (isNewAudio)
        {
            var timeDestroy = soundClick.length;
            Destroy(audioSource.gameObject, timeDestroy);
        }
    }
}
