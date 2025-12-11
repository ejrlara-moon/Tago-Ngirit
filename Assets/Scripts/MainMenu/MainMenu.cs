using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; 

public class MainMenu : MonoBehaviour
{
    [Header("Scene Configuration")]
    public string startingSceneName = "TRY Dark Scene";

    [Header("UI Panels")]
    public GameObject settingsPanel;
    public GameObject mainButtonsContainer;

    [Header("Loading Screen")]
    [Tooltip("Drag your 'loadingscreencanvas' object here")]
    public GameObject loadingScreenPanel;

    [Tooltip("Optional: Drag a Slider here if you have a progress bar")]
    public Slider loadingSlider;

    [Header("Audio")]
    public AudioSource uiAudioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private void Start()
    {
        // Ensure screens are in correct state on start
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (loadingScreenPanel != null) loadingScreenPanel.SetActive(false);
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        PlayClickSound();
        // Start the loading coroutine instead of loading directly
        StartCoroutine(LoadLevelAsync(startingSceneName));
    }

    // --- THE LOADING LOGIC ---
    IEnumerator LoadLevelAsync(string sceneName)
    {
        // 1. Show the Loading Screen
        if (loadingScreenPanel != null)
            loadingScreenPanel.SetActive(true);

        // 2. Hide the Main Menu buttons so player can't click again
        if (mainButtonsContainer != null)
            mainButtonsContainer.SetActive(false);

        // 3. Start loading the scene in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 4. Update the progress bar while loading
        while (!operation.isDone)
        {
            // Unity loads from 0 to 0.9, then finishes. 
            // We divide by 0.9 to get a clean 0 to 1 value.
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update the slider (if you assigned one)
            if (loadingSlider != null)
            {
                loadingSlider.value = progress;
            }

            yield return null; // Wait for the next frame
        }
    }

    // --- STANDARD BUTTONS ---
    public void OpenSettings()
    {
        PlayClickSound();
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(false);
    }

    public void CloseSettings()
    {
        PlayClickSound();
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(true);
    }

    public void QuitGame()
    {
        PlayClickSound();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // --- AUDIO HELPERS ---
    public void PlayHoverSound()
    {
        if (uiAudioSource != null && hoverSound != null)
            uiAudioSource.PlayOneShot(hoverSound);
    }

    private void PlayClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
            uiAudioSource.PlayOneShot(clickSound);
    }
}