using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunSound : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("SFX Names (must match SoundManager list)")]
    public string shootSFX = "GunShoot1";
    public string reloadSFX = "GunReload1";
    public string emptySFX = "GunEmpty";

    [Header("Local Volume Settings (can exceed normal volume)")]
    [Range(0f, 5f)]
    public float shootVolume = 1f;

    [Range(0f, 5f)]
    public float reloadVolume = 1f;

    [Range(0f, 5f)]
    public float emptyVolume = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private AudioClip GetClip(string clipName)
    {
        var item = SoundManager.instance.sfxClips.Find(s => s.name == clipName);
        if (item == null)
        {
            Debug.LogWarning("[GunSound] Missing clip: " + clipName);
            return null;
        }
        return item.clip;
    }

    private void PlayLocalClip(string clipName, float localVolume)
    {
        AudioClip clip = GetClip(clipName);
        if (clip == null) return;

        float finalVolume = SoundManager.instance.sfxVolume * localVolume;

        // allow up to 300% volume without clipping too hard
        finalVolume = Mathf.Clamp(finalVolume, 0f, 3f);

        audioSource.PlayOneShot(clip, finalVolume);
    }

    public void PlayShoot()
    {
        PlayLocalClip(shootSFX, shootVolume);
    }

    public void PlayReload()
    {
        PlayLocalClip(reloadSFX, reloadVolume);
    }

    public void PlayEmpty()
    {
        PlayLocalClip(emptySFX, emptyVolume);
    }
}
