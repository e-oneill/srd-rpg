using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.AI;
using UnityEditor;


namespace RPG.Saves
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueID = "";
        static Dictionary<string, SavableEntity> globalLookup = new Dictionary<string, SavableEntity>();
        
        // uniqueID = System.Guid.NewGuid().ToString();

        void Start()
        {
            if (Application.IsPlaying(gameObject))
            {

            }
            else
            {
                // Debug.Log("Editing");
            }
        }
#if UNITY_EDITOR
        void Update()
        {
            if (Application.IsPlaying(gameObject))
            {

            }
            else
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty  property = serializedObject.FindProperty("uniqueID");
                
                if (string.IsNullOrEmpty(gameObject.scene.path)) return;
                if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
                {
                    property.stringValue = System.Guid.NewGuid().ToString();
                    serializedObject.ApplyModifiedProperties();
                }

                globalLookup[property.stringValue] = this;

                // Debug.Log("Editing");
            }
        }
#endif

    private bool IsUnique(string candidate)
    {
        // 1. Check that the key exists in the dictionary. 
        if (!globalLookup.ContainsKey(candidate)) return true;
        if (globalLookup.ContainsKey(candidate) == this) return true;
        // 2. If the key exists, check whether it points to itself.

        if (globalLookup[candidate] == null)
        {
            globalLookup.Remove(candidate);
            return true;
        }
        
        if (globalLookup[candidate].GetUniqueID() != candidate)
        {
            globalLookup.Remove(candidate);
            return true;
        }

        return false;
        
    }


        public string GetUniqueID()
        {
            return uniqueID;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISavable saveable in GetComponents<ISavable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            // return new SerializableVector3(transform.position);
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string,object> stateDict = (Dictionary<string,object>)state;
            foreach (ISavable saveable in GetComponents<ISavable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }


        }


    }
}