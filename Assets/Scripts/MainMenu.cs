using FM;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FN
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Button")]

        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;

        public GameObject cutscenePanel;
        public Button skipButton;
        public GameObject creditPanel;
        bool cutsceneSkipped = false;

        private void Start ()
        {
            if (!DataPersistenceManager.Instance.HasGameData())
            {
                loadGameButton.interactable = false;
            }
        }

        public void ClickNewGame()
        {
            // Disable menu buttons and enable cutscene panel
            DisableMenuButtons();

            DataPersistenceManager.Instance.NewGame();
            // Enable the cutscene panel
            cutscenePanel.SetActive(true);

            // Start the cutscene
            StartCoroutine(PlayCutscene());
        }

        private IEnumerator PlayCutscene()
        {
            // Play your cutscene here
            // You can use WaitForSeconds or other methods to control the duration of the cutscene

            // Add a "Skip" button listener
            skipButton.onClick.AddListener(SkipCutscene);

            yield return new WaitForSeconds(22f); // Example cutscene duration of 3 seconds

            // Check if the cutscene is not skipped
            if (!cutsceneSkipped)
            {
                // Load the tutorial scene
                SceneManager.LoadSceneAsync("Tutorial");
            }
        }

        public void SkipCutscene()
        {
            // Set the cutscene as skipped
            cutsceneSkipped = true;

            // Stop the cutscene playback or perform any necessary cleanup

            // Load the tutorial scene immediately
            SceneManager.LoadSceneAsync("Tutorial");
        }

        public void ClickLoadGame()
        {
            DisableMenuButtons();
            DataPersistenceManager.Instance.LoadGame();
            SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.gameData.currentstage);
        }

        public void ClickQuitGame()
        {
            Application.Quit();
        }

        public void Credits()
        {
            creditPanel.SetActive(true);
        }

        public void CloseCredits()
        {
            creditPanel.SetActive(false);
        }

        private void DisableMenuButtons()
        {
            newGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
}
