using FN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LastBoss : MonoBehaviour
{
    public GameObject cutscenePanel;
    public Button skipButton;
    bool cutsceneSkipped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            // Enable the cutscene panel
            cutscenePanel.SetActive(true);

            // Start the cutscene
            StartCoroutine(PlayCutscene());
        }
    }

    private IEnumerator PlayCutscene()
    {
        // Play your cutscene here
        // You can use WaitForSeconds or other methods to control the duration of the cutscene

        // Add a "Skip" button listener
        skipButton.onClick.AddListener(SkipCutscene);

        yield return new WaitForSeconds(20f); // Example cutscene duration of 3 seconds

        // Check if the cutscene is not skipped
        if (!cutsceneSkipped)
        {
            // Load the tutorial scene
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void SkipCutscene()
    {
        // Set the cutscene as skipped
        cutsceneSkipped = true;

        // Stop the cutscene playback or perform any necessary cleanup

        // Load the tutorial scene immediately
        SceneManager.LoadScene("MainMenu");
    }
}
