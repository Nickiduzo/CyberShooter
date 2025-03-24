using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;

    [Header("Audio Settings")]
    [SerializeField] private Slider generalAudioSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private TextMeshProUGUI muteToggleTitle;

    [Header("Graphic Settings")]
    [SerializeField] private Button lightGraphic;
    [SerializeField] private Button mediumGraphic;
    [SerializeField] private Button ultraGraphic;

    [Header("Special Settings")]
    [SerializeField] private Button effects;
    [SerializeField] private Button fog;
    [SerializeField] private Button lighting;

    [Header("Screen")]
    [SerializeField] private Button fullScreen;
    [SerializeField] private Button windowScreen;

    private void Start()
    {
        InitializeListeners();
        InitializeInterface();
    }

    private void InitializeInterface()
    {
        musicVolumeSlider.value = settingsData.MusicAudio;
        effectsVolumeSlider.value = settingsData.EffectsAudio;
        generalAudioSlider.value = settingsData.GeneralAudio;
        muteToggle.isOn = settingsData.IsMuted;

        if (settingsData.IsMuted)
        {
            muteToggleTitle.text = "Muted";
            generalAudioSlider.interactable = false;
            musicVolumeSlider.interactable = false;
            effectsVolumeSlider.interactable = false;
        }
        else
        {
            muteToggleTitle.text = "Unmuted";
            generalAudioSlider.interactable = true;
            musicVolumeSlider.interactable = true;
            effectsVolumeSlider.interactable = true;
        }
    }

    private void InitializeListeners()
    {
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        effectsVolumeSlider.onValueChanged.AddListener(ChangeEffectsVolume);
        generalAudioSlider.onValueChanged.AddListener(ChangeGeneralVolume);
        muteToggle.onValueChanged.AddListener(ToggleAudio);

        lightGraphic.onClick.AddListener(LightSettings);
        mediumGraphic.onClick.AddListener(MediumSettings);
        ultraGraphic.onClick.AddListener(UltraSettings);

        effects.onClick.AddListener(SwitchEffects);
        fog.onClick.AddListener(SwitchFog);
        lighting.onClick.AddListener(SwitchLight);

        fullScreen.onClick.AddListener(SwitchFullScreen);
        windowScreen.onClick.AddListener(SwitchWindow);
    }

    private void ChangeGeneralVolume(float value)
    {
        settingsData.GeneralAudio = value;

        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(settingsData.MusicAudio);
            OnEffectsChanges(settingsData.EffectsAudio);
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(settingsData);
#endif
    }

    private void ChangeMusicVolume(float value)
    {
        settingsData.MusicAudio = value;
        
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(settingsData);
#endif
    }

    private void ChangeEffectsVolume(float value)
    {
        settingsData.EffectsAudio = value;
        OnEffectsChanges(value);

#if UNITY_EDITOR
        EditorUtility.SetDirty(settingsData);
#endif
    }

    private void OnEffectsChanges(float value)
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume("HoverButton", value);
            AudioManager.Instance.SetVolume("ClickButton", value);
        }
    }

    private void ToggleAudio(bool isMuted)
    {
        if(isMuted)
        {
            settingsData.PrevGeneralAudio = settingsData.GeneralAudio;
            ChangeGeneralVolume(0);
            muteToggleTitle.text = "Muted";

            generalAudioSlider.interactable = false;
            musicVolumeSlider.interactable = false;
            effectsVolumeSlider.interactable = false;
            settingsData.IsMuted = true;
        }
        else
        {
            ChangeGeneralVolume(settingsData.PrevGeneralAudio);
            muteToggleTitle.text = "Unmuted";

            generalAudioSlider.interactable = true;
            musicVolumeSlider.interactable = true;
            effectsVolumeSlider.interactable = true;
            settingsData.IsMuted = false;
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(settingsData);
#endif
    }

    private void LightSettings()
    {
        settingsData.IsLow = !settingsData.IsLow;
        settingsData.IsMedium = false;
        settingsData.IsUltra = false;
        SetQuality(0);
    }

    private void MediumSettings()
    {
        settingsData.IsLow = false;
        settingsData.IsMedium = !settingsData.IsMedium;
        settingsData.IsUltra = false;
        SetQuality(1);
    }

    private void UltraSettings()
    {
        settingsData.IsLow = false;
        settingsData.IsMedium = false;
        settingsData.IsUltra = !settingsData.IsUltra;
        SetQuality(2);
    }

    private void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void SwitchLight()
    {
        settingsData.IsLight = !settingsData.IsLight;
    }

    private void SwitchFog()
    {
        settingsData.IsFog = !settingsData.IsFog;
    }

    private void SwitchEffects()
    {
        settingsData.IsEffects = !settingsData.IsEffects;
    }

    private void SwitchWindow()
    {
        settingsData.Window = true;
        settingsData.FullScreen = false;

        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(1280, 720, false);
    }

    private void SwitchFullScreen()
    {
        settingsData.Window = false;
        settingsData.FullScreen = true;

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow; 
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }

    private void OnDisable()
    {
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        effectsVolumeSlider.onValueChanged.RemoveAllListeners();
        generalAudioSlider.onValueChanged.RemoveAllListeners();
        muteToggle.onValueChanged.RemoveAllListeners();

        lightGraphic.onClick.RemoveAllListeners();
        mediumGraphic.onClick.RemoveAllListeners();
        ultraGraphic.onClick.RemoveAllListeners();

        effects.onClick.RemoveAllListeners();
        fog.onClick.RemoveAllListeners();
        lighting.onClick.RemoveAllListeners();

        fullScreen.onClick.RemoveAllListeners();
        windowScreen.onClick.RemoveAllListeners();
    }
}
