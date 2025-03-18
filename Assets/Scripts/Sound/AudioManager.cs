using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSound[] sounds;
    public AudioSound[] musicTracks;
    public SettingsData settings;

    public int currentTrackIndex = -1;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Initialization();
    }

    private void Initialization()
    {
        foreach (AudioSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.GetEffectsVolume();
            s.source.loop = s.loop;
        }

        if (musicTracks.Length > 0)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = false;
            musicSource.volume = musicTracks[0].GetMusicVolume();
        }
    }

    public void Play(string soundName)
    {
        if (IsMuted()) return;
        AudioSound s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning($"«вук '{soundName}' не найден!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string soundName)
    {
        AudioSound s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) return;
        s.source.Stop();
    }

    public void SetVolume(string soundName, float volume)
    {
        AudioSound s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) return;
        s.source.volume = volume * settings.GeneralAudio;
    }

    public void SetMusicVolume(float value)
    {
        if(musicSource != null)
        {
            musicSource.volume = value * settings.GeneralAudio;
        }
    }

    public void PlayRandomTrack()
    {
        if (musicTracks.Length == 0) return;

        int randomIndex = Random.Range(0, musicTracks.Length);
        PlayMusicTrack(randomIndex);
    }

    public void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return;

        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        PlayMusicTrack(currentTrackIndex);
    }

    private void PlayMusicTrack(int index)
    {
        if (index < 0 || index >= musicTracks.Length) return;

        AudioSound track = musicTracks[index];
        musicSource.clip = track.clip;
        musicSource.volume = track.GetMusicVolume();
        musicSource.Play();

        StartCoroutine(WaitForTrackEnd());
    }

    private IEnumerator WaitForTrackEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayNextTrack();
        }
    }

    public void StopAllSounds()
    {
        foreach (AudioSound s in sounds)
        {
            s.source.Stop();
        }

        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public bool IsMuted()
    {
        return settings.IsMuted;
    }
}
