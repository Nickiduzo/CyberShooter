using Unity.Netcode;
using UnityEngine;

public class AudioManager : NetworkBehaviour
{
    public static AudioManager instanse;

    public Sound[] sounds;

    public Sound[] walkSteps;

    private void Awake()
    {
        if (instanse == null)
        {
            instanse = this;
        }
        DontDestroyOnLoad(gameObject);
        Initialization();
    }

    public void Play(string songName)
    {
        foreach (var sound in sounds)
        {
            if(sound.clipName == songName)
            {
                sound.source.Play();
                break;
            }
        }
    }

    public void Stop(string songName)
    {
        foreach(var sound in sounds)
        {
            if(sound.clipName == songName)
            {
                sound.source.Stop();
                break;
            }
        }
    }

    public void PlayStep()
    {
        walkSteps[Random.Range(0, walkSteps.Length)].source.Play();
    }

    #region Initialization
    private void Initialization()
    {
        InitializeSFXSounds();
        InitializeWalkSteps();
    }

    private void InitializeSFXSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.isLoop;
            s.source.name = s.clipName;
        }
    }

    private void InitializeWalkSteps()
    {
        foreach (Sound s in walkSteps)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.isLoop;
            s.source.name = s.clipName;
        }
    }

    #endregion
}
