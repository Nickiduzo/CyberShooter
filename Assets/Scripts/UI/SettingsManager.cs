using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;
    
    [Header("Audio Settings")]
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;

    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        AudioManager.Instance.SetMusicVolume(settingsData.MusicAudio);
        AudioManager.Instance.SetEffectsVolume(settingsData.EffectsAudio);

        musicVolumeSlider.value = settingsData.MusicAudio;
        effectsVolumeSlider.value = settingsData.EffectsAudio;

        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        effectsVolumeSlider.onValueChanged.AddListener(ChangeEffectsVolume);
    }

    private void ChangeMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        settingsData.MusicAudio = value;
        print(value);

        #if UNITY_EDITOR
        EditorUtility.SetDirty(settingsData);
        #endif
    }

    private void ChangeEffectsVolume(float value)
    {
        AudioManager.Instance.SetEffectsVolume(value);
        settingsData.EffectsAudio = value;
        print(value);

        #if UNITY_EDITOR
        EditorUtility.SetDirty(settingsData);
        #endif
    }

    private void OnDisable()
    {
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        effectsVolumeSlider.onValueChanged.RemoveAllListeners();
    }
}
