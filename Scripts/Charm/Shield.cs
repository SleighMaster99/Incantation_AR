using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    AudioSource audioPlayer;
    public AudioClip shieldSound;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        this.transform.position = Camera.main.transform.position;
        if(other.CompareTag("BasicGhost") || other.CompareTag("MiddleGhost"))
        {
            if (!audioPlayer.isPlaying)
                audioPlayer.PlayOneShot(shieldSound);
        }
    }

    public void PlayShieldSound()
    {
        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(shieldSound);
    }
}
