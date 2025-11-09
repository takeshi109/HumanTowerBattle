using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip countdownBeep;
    public AudioClip startSound;
    public AudioClip imageReceivedSound;

    public AudioClip ChangeSceneClearSound;

    public AudioClip ChangeSceneGameOverSound;

    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも残す
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCountdownBeep()
    {
        if (audioSource != null && countdownBeep != null)
            audioSource.PlayOneShot(countdownBeep);
    }

    public void PlayStartSound()
    {
        if (audioSource != null && startSound != null)
            audioSource.PlayOneShot(startSound);
    }

    public void PlayImageReceivedSound()
    {
        if (audioSource != null && imageReceivedSound)
            audioSource.PlayOneShot(imageReceivedSound);
    }

    public void PlaySEAndChangeSceneClear()
    {
        if (audioSource != null && ChangeSceneClearSound)
            audioSource.PlayOneShot(ChangeSceneClearSound);
    }

    public void PlaySEAndChangeSceneGameOver()
    {
        if (audioSource != null && ChangeSceneGameOverSound)
            audioSource.PlayOneShot(ChangeSceneGameOverSound);
    }
}