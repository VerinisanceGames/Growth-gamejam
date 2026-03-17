using UnityEngine;

public class Menusounds : MonoBehaviour
{
    [SerializeField] private AudioListener _menuListener;
    
    public AudioSource mainMenuMusicSource;
    public AudioSource buttonSoundSource;
    public AudioClip clickSound;
    public AudioClip menuMusic;

    private bool musicFadeOutEnabled = false;
    
    void Start()
    {
        if (mainMenuMusicSource != null && mainMenuMusicSource != null)
        {
            //buttonSoundSource.PlayOneShot(menuMusic);
            mainMenuMusicSource.Play();
        }
    }

    public void PlayClickSound()
    {
        musicFadeOutEnabled = true;
        
        if (buttonSoundSource != null && buttonSoundSource != null)
        {
            buttonSoundSource.Play();
        }

        // if (mainMenuMusicSource != null && mainMenuMusicSource.isPlaying)
        // {
        //     mainMenuMusicSource.Stop();
        // }
        
    }

    // public void StopPlayback()
    // {
    //     if (mainMenuMusicSource != null && mainMenuMusicSource.isPlaying)
    //     {
    //         mainMenuMusicSource.Stop();
    //     }
    // }

    public void Update()
    {
        if (musicFadeOutEnabled)
        {
            if (mainMenuMusicSource.volume <= 0.01f)
            {
                mainMenuMusicSource.Stop();
                musicFadeOutEnabled = false;
                _menuListener.enabled = false;
            }
            else
            {
                float newVolume = mainMenuMusicSource.volume - (0.08f * Time.deltaTime);
                if (newVolume < 0f)
                {
                    newVolume = 0f;
                }
                mainMenuMusicSource.volume = newVolume;
            }
        }
    }
}