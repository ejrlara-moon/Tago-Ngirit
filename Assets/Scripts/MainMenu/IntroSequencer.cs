using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequencer : MonoBehaviour
{
    [Header("Sequence Configuration")]
    [Tooltip("Drag your logo objects here (ProductionLogo, GameIcon, WarningPanel). Order matters!")]
    public CanvasGroup[] logoSequence;

    [Tooltip("The Main Menu Panel to show after the logos finish.")]
    public GameObject mainMenuPanel;

    [Header("Timing")]
    public float fadeDuration = 0.5f;
    public float displayDuration = 2.0f;

    private bool isSkipping = false;

    private void Start()
    {
        // 1. Hide the Main Menu initially
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);

        // 2. Hide all logos initially (set alpha to 0 and block raycasts)
        foreach (CanvasGroup logo in logoSequence)
        {
            if (logo != null)
            {
                logo.gameObject.SetActive(false);
                logo.alpha = 0f;
            }
        }

        // 3. Start the sequence
        StartCoroutine(PlaySequence());
    }

    private void Update()
    {
        // 4. Check for click to SKIP
        if (Input.GetMouseButtonDown(0) && !isSkipping)
        {
            isSkipping = true; // Flag to stop the coroutine loop
        }
    }

    private IEnumerator PlaySequence()
    {
        // Loop through every logo in the list
        foreach (CanvasGroup logo in logoSequence)
        {
            if (logo == null) continue;

            // Enable the object
            logo.gameObject.SetActive(true);

            // FADE IN
            float timer = 0f;
            while (timer < fadeDuration && !isSkipping)
            {
                timer += Time.deltaTime;
                logo.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                yield return null;
            }
            logo.alpha = 1f;

            // HOLD (Display)
            timer = 0f;
            while (timer < displayDuration && !isSkipping)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // FADE OUT
            timer = 0f;
            while (timer < fadeDuration && !isSkipping)
            {
                timer += Time.deltaTime;
                logo.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                yield return null;
            }
            logo.alpha = 0f;
            logo.gameObject.SetActive(false);

            // If player clicked skip, break the loop immediately
            if (isSkipping) break;
        }

        // SEQUENCE FINISHED: Show Main Menu
        FinishSequence();
    }

    private void FinishSequence()
    {
        // Ensure all logos are hidden
        foreach (CanvasGroup logo in logoSequence)
        {
            if (logo != null) logo.gameObject.SetActive(false);
        }

        // Show the Main Menu
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);

            // Optional: Animate Main Menu appearing smoothly
            CanvasGroup menuCG = mainMenuPanel.GetComponent<CanvasGroup>();
            if (menuCG != null)
            {
                menuCG.alpha = 1f;
                menuCG.interactable = true;
                menuCG.blocksRaycasts = true;
            }
        }
    }
}
