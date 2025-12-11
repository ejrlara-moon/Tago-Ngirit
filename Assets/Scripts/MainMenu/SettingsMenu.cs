using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag your Volume Slider here")]
    public Slider volumeSlider;

    private float defaultVolume = 1.0f;

    private void Start()
    {
        // 1. Load the saved volume (or use 1.0 if it's the first time playing)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);

        // 2. Update the slider UI to match the saved value
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;

            // 3. Listen for changes (so it updates while you drag)
            volumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        // 4. Apply the volume to the game immediately
        AudioListener.volume = savedVolume;
    }

    public void SetMasterVolume(float volume)
    {
        // Changes the global volume of the game (0 is mute, 1 is full)
        AudioListener.volume = volume;

        // Save the setting to disk
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
