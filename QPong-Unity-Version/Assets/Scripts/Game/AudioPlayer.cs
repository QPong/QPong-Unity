
using UnityEngine;

public enum Sound { bouncePaddle, bounceWall, lostSound }

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{

    public AudioClip bouncePaddle;
    public AudioClip bounceWall;
    public AudioClip lostSound;

    private AudioSource audioSource;
   

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(Sound soundName)
    {
        AudioClip audioClip = bouncePaddle;
        switch (soundName)
        {
            case Sound.bouncePaddle:
                audioClip = bouncePaddle;
                break;
            case Sound.bounceWall:
                audioClip = bounceWall;
                break;
            case Sound.lostSound:
                audioClip = lostSound;
                break;
        }
        PlayAudioClip(audioClip);
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip, 0.8f);
    }
}