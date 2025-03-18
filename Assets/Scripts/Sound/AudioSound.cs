using UnityEngine;

[System.Serializable]
public class AudioSound 
{
    public string name; 
    public AudioClip clip;
    public SettingsData settings;
    public bool loop;

    [HideInInspector] public AudioSource source;
    
    public float GetEffectsVolume()
    {
        return settings.EffectsAudio;
    }

    public float GetMusicVolume()
    {
        return settings.MusicAudio;
    }
}
