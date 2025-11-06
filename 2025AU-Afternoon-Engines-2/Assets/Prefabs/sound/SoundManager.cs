using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;     // ONE source for all SFX

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public bool muteSFX = false;

    [Header("Sound Library")]
    public List<SoundItem> sfxClips = new List<SoundItem>();

    [System.Serializable]
    public class SoundItem
    {
        public string name;
        public AudioClip clip;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        UpdateVolume();
    }

    public void PlaySFX(string name)
    {
        if (muteSFX) return;

        AudioClip clip = GetClip(name);
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    private AudioClip GetClip(string name)
    {
        SoundItem item = sfxClips.Find(s => s.name == name);

        if (item == null)
        {
            Debug.LogWarning("[SoundManager] Missing SFX: " + name);
            return null;
        }

        return item.clip;
    }

    public void SetVolume(float value)
    {
        sfxVolume = value;
        UpdateVolume();
    }

    public void Mute(bool mute)
    {
        muteSFX = mute;
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        sfxSource.volume = muteSFX ? 0 : sfxVolume;
    }
}
