using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVoice : MonoBehaviour
{
    public AudioSource mainMenuMusicSource;

    public AudioClip startGameSound;

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("MainVoice Start");
        this.PlayGameStart();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameStart()
    {
        Debug.LogError("MainVoice PlayGameStart");
        if (mainMenuMusicSource != null && startGameSound != null)
        {
            Debug.LogError("MainVoice PlayGameStart 12");
            mainMenuMusicSource.PlayOneShot(startGameSound);
        }

    }
}
