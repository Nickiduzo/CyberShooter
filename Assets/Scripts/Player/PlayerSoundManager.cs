using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public void MakeSlowStep() => AudioManager.instanse.PlayStep();

    public void MakeSword() => AudioManager.instanse.Play("Sword");
}
