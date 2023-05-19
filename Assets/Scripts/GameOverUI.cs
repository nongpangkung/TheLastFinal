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
            DataPersistenceManager.Instance.LoadGame();
            string currentSceneName = DataPersistenceManager.Instance.gameData.currentstage;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
