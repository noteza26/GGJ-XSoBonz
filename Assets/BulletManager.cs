using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    //
    public float SpeedBullet;
    public float TimeToDestroy;
    // Update is called once per frame
    private void Start()
    {
        Destroy(this.gameObject, TimeToDestroy);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * SpeedBullet * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var triggerTag = other.tag;
        if (triggerTag == "PlayerController")
        {

        }

        Destroy(this.gameObject);
        Debug.Log(triggerTag);

    }
}
