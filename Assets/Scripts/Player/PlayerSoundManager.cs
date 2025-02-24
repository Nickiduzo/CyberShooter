using Unity.Netcode;
using UnityEngine;

public class PlayerSoundManager : NetworkBehaviour
{
    [SerializeField] private AudioSource stepAudioSource;
    [SerializeField] private AudioSource swordAudioSource;

    public Sound[] walkSteps;

    public Sound swordSwing;
    
    public void MakeSlowStep()
    {
        stepAudioSource.clip = walkSteps[Random.Range(0, walkSteps.Length)].clip;
        stepAudioSource.Play();
    }

    public void MakeSword()
    {
        swordAudioSource.clip = swordSwing.clip;
        swordAudioSource.Play();
    }

    public void MakeJump() => AudioManager.instanse.Play("Jump");

    public void StopMusic() => AudioManager.instanse.Stop("Polskaya");
}
