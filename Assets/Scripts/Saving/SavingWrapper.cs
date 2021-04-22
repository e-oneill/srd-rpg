using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saves
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "autosave";

        // private IEnumerator Start() 
        // {
        //     yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        // }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveGame();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadGame();
            }
        }

        public void SaveGame()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void SaveGame(string SaveFileName)
        {
            GetComponent<SavingSystem>().Save(SaveFileName);
        }

        public void LoadGame()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void LoadGame(string SaveFileName)
        {
            GetComponent<SavingSystem>().Load(SaveFileName);
        }
    }
}
