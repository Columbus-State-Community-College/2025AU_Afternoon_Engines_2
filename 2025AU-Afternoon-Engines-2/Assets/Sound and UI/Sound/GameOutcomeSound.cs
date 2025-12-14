using UnityEngine;

public class GameOutcomeSound : MonoBehaviour
{
    [Header("SFX Keys (SoundManager Library Names)")]
    public string winSFX;
    public string loseSFX;

    [Header("Local Volume Multipliers")]
    [Range(0f, 3f)] public float winVolume = 1f;
    [Range(0f, 3f)] public float loseVolume = 1f;

    private SoundManager soundManager;
    private bool played = false;

private void Start()
{
    Debug.Log("[GameOutcomeSound] Start — testing lose SFX");
    PlayLose();
}



    private void Awake()
    {
        soundManager = SoundManager.instance;
    }

    public void PlayWin()
    {
        if (played) return;
        played = true;

        PlaySFX(winSFX, winVolume);
    }

    public void PlayLose()
    {
        if (played) return;
        played = true;

        PlaySFX(loseSFX, loseVolume);
    }

    private void PlaySFX(string sfxName, float localVolume)
    {
        if (soundManager == null) return;
        if (string.IsNullOrEmpty(sfxName)) return;

        var item = soundManager.sfxClips.Find(s => s.name == sfxName);
        if (item == null || item.clip == null) return;

        float finalVol = soundManager.sfxVolume * localVolume;
        finalVol = Mathf.Clamp(finalVol, 0f, 3f);

        soundManager.sfxSource.PlayOneShot(item.clip, finalVol);
    }
}
