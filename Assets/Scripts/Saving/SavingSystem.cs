using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using RPG.TurnManager;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

namespace RPG.Saves
{
public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int lastScene = (int)state["lastSceneBuildIndex"];
            if (lastScene != SceneManager.GetActiveScene().buildIndex)
            {
                yield return SceneManager.LoadSceneAsync(lastScene);
            }
            RestoreState(state);
        }

        public SessionManager sessionManager;
        public Transform[] partyChars;

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        { 
            RestoreState(LoadFile(saveFile));
        }

        public void SaveFile(string saveFile, object CaptureState)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, CaptureState);                      
            }
        }
      
        private void CaptureState(Dictionary<string, object> state)
        {
            
            foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>())
            {
                state[savable.GetUniqueID()] = savable.CaptureState();
            }
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            state["lastSceneBuildIndex"] = currentScene;
        }

        public Dictionary<string, object> LoadFile(string saveFile)
        {
            string  path = GetPathFromSaveFile(saveFile);
            if (File.Exists(path))
            {
                print("Loading from " + GetPathFromSaveFile(saveFile));
                using (FileStream stream = File.Open(path, FileMode.Open))
                {   
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (Dictionary<string, object>)formatter.Deserialize(stream);
                }
            }
            else
            {
                Debug.Log("Load File not Found, creating new dictionary.");
                return new Dictionary<string, object>();
            }
        }

        private void RestoreState(Dictionary<string, object> stateDict)
        {
            
            foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>())
            {
                string id = savable.GetUniqueID();
                if (stateDict.ContainsKey(id))
                {
                savable.RestoreState(stateDict[savable.GetUniqueID()]); 
                }
            }
            
        }
    
        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}