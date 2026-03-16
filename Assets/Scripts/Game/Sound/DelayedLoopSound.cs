using System.Collections;
using UnityEngine;

public class DelayedAudioLoop : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip soundToLoop;
    public float delayBetweenPlays = 5.0f;
    public float delayFromStart = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found!");
            return;
        }


        // Assign the clip if not already assigned in the Inspector
        if (soundToLoop != null && audioSource.clip == null)
        {
            audioSource.clip = soundToLoop;
        }

       
        StartCoroutine(SoundLoopRoutine(delayFromStart));
    }

    IEnumerator SoundLoopRoutine(float delay)
    {
        yield return new WaitForSeconds(delayFromStart);

        while (true) // Loop forever
        {
            // Play the sound
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }

            // Wait for the sound to finish playing AND the specified delay time
            yield return new WaitForSeconds(audioSource.clip != null ? audioSource.clip.length + delayBetweenPlays : delayBetweenPlays);
        }
    }
}
