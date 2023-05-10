using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioPlayer;
    public AudioClip backgroundMusic;
    public PlayerInformation pi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pi.isGameOver)
        {
            audioPlayer.Stop();
        }

        if(GameManager.currentLocation == GameManager.CurrentLocation.normalGround)
        {
            if (!audioPlayer.isPlaying)
                audioPlayer.PlayOneShot(backgroundMusic);
        }
        else
        {
            audioPlayer.Stop();
        }
    }
}
