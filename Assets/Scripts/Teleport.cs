using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FN
{
    public class Teleport : MonoBehaviour
    {
        public string LevelName;
        string nextSceneName;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Character")
            {
                // Get the next scene name
                nextSceneName = LevelName; // Replace LevelName with the appropriate variable or value that represents the next scene name

                // Save the next scene name to currentstage in the game data
                DataPersistenceManager.Instance.gameData.currentstage = nextSceneName;

                // Save the updated game data
                DataPersistenceManager.Instance.SaveGame();
                Debug.Log("Load");
                StartCoroutine(waitloadscene(2));
            }
        }

        private IEnumerator waitloadscene(float cd)
        {
            yield return new WaitForSeconds(cd);
            // Load the next scene
            Debug.Log("Complete");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
