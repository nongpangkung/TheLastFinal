using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN {
    public class ChangeSceneManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DataPersistenceManager.Instance.SaveGame();
        }
    }
}
