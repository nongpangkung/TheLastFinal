using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FN
{
    public class Teleport : MonoBehaviour
    {
        public string LevelName;
        private string nextSceneName;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Character"))
            {
                // Get the next scene name
                nextSceneName = LevelName; // Replace LevelName with the appropriate variable or value that represents the next scene name

                // Clear the cache synchronously
                Caching.ClearCache();

                // Save the next scene name to currentstage in the game data
                DataPersistenceManager.Instance.gameData.currentstage = nextSceneName;

                // Save the updated game data
                DataPersistenceManager.Instance.SaveGame();

                StartCoroutine(WaitAndLoadScene(2f));
            }
        }

        private IEnumerator WaitAndLoadScene(float delay)
        {
            yield return new WaitForSeconds(delay);

            // Load the next scene
            Debug.Log("Complete");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
