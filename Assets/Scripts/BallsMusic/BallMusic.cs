using Unity.Netcode;
using UnityEngine;

public class BallMusic : NetworkBehaviour
{
    [SerializeField] private AudioSound[] musics;
    [SerializeField] private AudioSource musicSource;

    private NetworkVariable<int> trackIndex = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Start()
    {
        if (IsServer) 
        {
            ChooseRandomTrackServerRpc();
        }

        trackIndex.OnValueChanged += (oldValue, newValue) => PlayMusic(newValue);
        PlayMusic(trackIndex.Value);
    }

    private void PlayMusic(int index)
    {
        if (AudioManager.Instance == null || AudioManager.Instance.IsMuted()) return;
        if (musics.Length == 0) return;

        musicSource.clip = musics[index].clip;
        musicSource.volume = musics[index].GetMusicVolume();
        musicSource.Play();
    }

    private void Update()
    {
        if (!musicSource.isPlaying && IsServer)
        {
            ChooseRandomTrackServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChooseRandomTrackServerRpc()
    {
        int newTrack = Random.Range(0, musics.Length);
        trackIndex.Value = newTrack;
    }
}
