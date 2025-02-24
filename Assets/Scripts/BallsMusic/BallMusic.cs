using Unity.Netcode;
using UnityEngine;

public class BallMusic : NetworkBehaviour
{
    [SerializeField] private Sound[] musics;
    [SerializeField] private AudioSource musicSource;

    private NetworkVariable<int> trackIndex = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Start()
    {
        if (IsServer) // Хост выбирает музыку
        {
            ChooseRandomTrackServerRpc();
        }

        trackIndex.OnValueChanged += (oldValue, newValue) => PlayMusic(newValue);
        PlayMusic(trackIndex.Value);
    }

    private void PlayMusic(int index)
    {
        if (musics.Length == 0) return;

        musicSource.clip = musics[index].clip;
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
