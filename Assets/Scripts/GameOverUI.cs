using FM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace FN
{
    public class GameOverUI : MonoBehaviour
    {
        public void Retry()
        {
            // Clear the cache
            Caching.ClearCache();

            // Load the scene
            DataPersistenceManager.Instance.LoadGame();
            string currentSceneName = DataPersistenceManager.Instance.gameData.currentstage;
            SceneManager.LoadScene(currentSceneName);
        }

        public void mainmenu()
        {
            // Clear the cache
            Caching.ClearCache();

            SceneManager.LoadScene("MainMenu");
        }
    }
}
