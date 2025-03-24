using Unity.Netcode;
using UnityEngine;

public class PlayerSoundManager : NetworkBehaviour
{
    [SerializeField] private AudioSource swordAudioSource;
    [SerializeField] private AudioSource swordHitSource;
    [SerializeField] private AudioSource stepAudioSource;
    [SerializeField] private AudioSource jumpAudioSource;
    [SerializeField] private AudioSource landAudioSource;

    public AudioSound swordHit;
    public AudioSound swordSwing;
    public AudioSound[] walkSteps;
    public AudioSound jump;
    public AudioSound land;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            Sword.SwordHit += MakeHit;
            PlayerJump.OnLand += MakeLand;
        }
    }

    public void MakeSlowStep()
    {
        if (AudioManager.Instance == null || AudioManager.Instance.IsMuted()) return;
        int index = Random.Range(0, walkSteps.Length);
        stepAudioSource.clip = walkSteps[index].clip;
        stepAudioSource.volume = walkSteps[index].GetEffectsVolume();
        stepAudioSource.Play();
    }

    public void MakeSword()
    {
        if (AudioManager.Instance == null || AudioManager.Instance.IsMuted()) return;
        swordAudioSource.clip = swordSwing.clip;
        swordAudioSource.volume = swordSwing.GetEffectsVolume();
        swordAudioSource.Play();
    }

    public void MakeHit()
    {
        if (AudioManager.Instance == null || AudioManager.Instance.IsMuted()) return;
        swordHitSource.clip = swordHit.clip;
        swordHitSource.volume = swordHit.GetEffectsVolume();
        swordHitSource.Play();
    }

    public void MakeJump()
    {
        if (AudioManager.Instance == null || AudioManager.Instance.IsMuted()) return;
        jumpAudioSource.clip = jump.clip;
        jumpAudioSource.volume = jump.GetEffectsVolume();
        jumpAudioSource.Play();
    }

    public void MakeLand()
    {
        if (AudioManager.Instance == null || AudioManager.Instance.IsMuted()) return;
        landAudioSource.clip = land.clip;
        landAudioSource.volume = land.GetEffectsVolume();
        landAudioSource.Play();
    }

    public override void OnNetworkDespawn()
    {
        if(IsOwner)
        {
            Sword.SwordHit -= MakeHit;
            PlayerJump.OnLand -= MakeLand;
        }
    }
}
