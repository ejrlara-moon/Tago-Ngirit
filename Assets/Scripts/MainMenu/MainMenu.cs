using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required to change scenes
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("The exact name of the scene to load. Based on your GDD, this is likely 'Rooftop'.")]
    public string startingSceneName = "TRY Dark Scene";

    [Header("UI Panels")]
    [Tooltip("Drag your Settings Panel GameObject here.")]
    public GameObject settingsPanel;

    [Tooltip("Drag the Main Menu buttons container here (optional, to hide buttons when settings are open).")]
    public GameObject mainButtonsContainer;

    [Header("Audio (Optional)")]
    public AudioSource uiAudioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private void Start()
    {
        // Ensure settings are closed and cursor is visible when menu starts
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // --- BUTTON FUNCTIONS ---

    public void PlayGame()
    {
        PlayClickSound();
        // Load the game scene
        SceneManager.LoadScene(startingSceneName);
    }

    public void OpenSettings()
    {
        PlayClickSound();
        // Show Settings, Hide Main Buttons
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(false);
    }

    public void CloseSettings()
    {
        PlayClickSound();
        // Hide Settings, Show Main Buttons
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(true);
    }

    public void QuitGame()
    {
        PlayClickSound();
        Debug.Log("User requested Quit. (Application.Quit only works in Build)");
        Application.Quit();
    }

    // --- AUDIO FUNCTIONS ---
    // Link this to the EventTrigger "PointerEnter" on your buttons
    public void PlayHoverSound()
    {
        if (uiAudioSource != null && hoverSound != null)
        {
            uiAudioSource.PlayOneShot(hoverSound);
        }
    }

    private void PlayClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }
}
