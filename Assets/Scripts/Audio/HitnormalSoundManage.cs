using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip hitnormalDon;
    public AudioClip hitnormalKan;
    public AudioClip hitnormalFinisherDon;
    public AudioClip hitnormalFinisherKan;
    public AudioClip missSound;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayDon()
    {
        if (audioSource != null && hitnormalDon != null)
            audioSource.PlayOneShot(hitnormalDon, 0.05f);
    }

    public void PlayKan()
    {
        if (audioSource != null && hitnormalKan != null)
            audioSource.PlayOneShot(hitnormalKan, 0.05f);
    }

    public void PlayFinisherDon()
    {
        if (audioSource != null && hitnormalFinisherDon != null)
            audioSource.PlayOneShot(hitnormalFinisherDon, 0.05f);
    }

    public void PlayFinisherKan()
    {
        if (audioSource != null && hitnormalFinisherKan != null)
            audioSource.PlayOneShot(hitnormalFinisherKan, 0.05f);
    }

    public void PlayMiss()
    {
        if (audioSource != null && missSound != null)
            audioSource.PlayOneShot(missSound, 0.05f);
    }
}
