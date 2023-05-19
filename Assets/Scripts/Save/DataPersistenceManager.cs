using FM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace FN
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("Debugging")]

        [SerializeField] private bool initializeDataIfNull = false;

        [Header("File Storage Config")]

        [SerializeField] private string fileName;

        [SerializeField] private bool useEncryption;

        public GameData gameData;

        private List<IDataPersistence> dataPersistenceObjects;

        private FileDataHandler dataHandler;

        public static DataPersistenceManager Instance {  get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void OnSceneUnloaded(Scene scene)
        {
            SaveGame();
        }

        public void NewGame()
        {
            this.gameData = new GameData(SceneManager.GetActiveScene().name);
        }

        public void LoadGame()
        {
            this.gameData = dataHandler.Load();

            if (this.gameData == null && initializeDataIfNull)
            {
                NewGame();
            }

            if (this.gameData == null)
            {
                Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
                return;
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }

        public void SaveGame()
        {
            if (this.gameData == null)
            {
                Debug.Log("No data was found. A New Game needs to be started before data can be saved.");
                return;
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }

            dataHandler.Save(gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public bool HasGameData()
        {
            return gameData != null;
        }
    }
}
