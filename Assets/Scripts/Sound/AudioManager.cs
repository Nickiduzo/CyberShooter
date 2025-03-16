using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSound[] sounds;
    public AudioSound[] musicTracks;
    public SettingsData settings;

    private int currentTrackIndex = -1;
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

        foreach (AudioSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = settings.EffectsAudio;
            s.source.loop = s.loop;
        }

        if (musicTracks.Length > 0)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = false;
            musicSource.volume = settings.MusicAudio;
        }
    }

    public void Play(string soundName)
    {
        AudioSound s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning($"Звук '{soundName}' не найден!");
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
        s.source.volume = volume;
    }

    public void SetMusicVolume(float value)
    {
        foreach (AudioSound sound in musicTracks)
        {
            sound.source.volume = value;
        }
    }

    public void SetEffectsVolume(float value)
    {
        foreach (AudioSound sound in sounds)
        {
            sound.source.volume = value;
        }
    }

    public void PlayRandomTrack()
    {
        if (musicTracks.Length == 0) return;

        int randomIndex = Random.Range(0, musicTracks.Length);
        PlayMusicTrack(randomIndex);
    }

    // Воспроизведение следующего трека
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
        musicSource.volume = track.volume;
        musicSource.Play();

        // Ждем окончания трека и включаем следующий
        StartCoroutine(WaitForTrackEnd());
    }

    private IEnumerator WaitForTrackEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);
        PlayNextTrack();
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
}
