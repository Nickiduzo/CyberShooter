using UnityEngine;

[CreateAssetMenu(fileName ="SettingsData", menuName ="Settings")]
public class SettingsData : ScriptableObject
{
    public float GeneralAudio;
    public float MusicAudio;
    public float EffectsAudio;
    public bool IsMuted;

    public bool Window;
    public bool FullScreen;

    public bool IsLow;
    public bool IsMedium;
    public bool IsUltra;

    public bool IsEffects;
    public bool IsFog;
    public bool IsLight;
}
