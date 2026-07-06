using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioSource _sfxPlayer;
    private const float MIN_PITCH = 0.9f;
    private const float MAX_PITCH = 1.1f;
    
    public void PlaySFX(AudioData audioData)
    {
        _sfxPlayer.PlayOneShot(audioData.AudioClip,audioData.Volume);
    }

    public void PlayRandomSFX(AudioData audioData)
    {
        _sfxPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }

    public void PlayRandomSFX(AudioData[] audioData)
    {
        PlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
    }
}

[System.Serializable]
public class AudioData
{
    public AudioClip AudioClip;
    public float Volume;
}
