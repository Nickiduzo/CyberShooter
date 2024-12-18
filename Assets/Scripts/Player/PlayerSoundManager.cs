using Unity.Netcode;
using UnityEngine;

public class PlayerSoundManager : NetworkBehaviour
{
    public void MakeSlowStep() => AudioManager.instanse.PlayStep();

    public void MakeSword() => AudioManager.instanse.Play("Sword");

    public void MakeJump() => AudioManager.instanse.Play("Jump");

    public void StopMusic() => AudioManager.instanse.Stop("Polskaya");
}
