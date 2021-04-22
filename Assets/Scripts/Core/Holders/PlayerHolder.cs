using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Grid;
using RPG.StateMachine;
using RPG.Characters;

namespace RPG.TurnManager 
{
    [CreateAssetMenu(menuName="RPG Game/Player Holder")]
    public class PlayerHolder : ScriptableObject
    {
        [System.NonSerialized]
        public StateManager stateManager;
        [System.NonSerialized]
        GameObject stateManagerObject;
        [SerializeField]
        GameObject stateManagerPrefab;
        [System.NonSerialized]
        public List<GridCharacter> characters = new List<GridCharacter>();
        

        public void Init() 
        {
            {
                stateManagerObject = Instantiate(stateManagerPrefab) as GameObject;
                stateManager = stateManagerObject.GetComponent<StateManager>();
                stateManager.playerHolder = this;
            }
        }

        public void RegisterCharacter(GridCharacter c)
        {
            if (!characters.Contains(c))
                // Debug.Log(c);
                characters.Add(c);
                // Debug.Log(characters[0]);
        }

        public void UnRegisterCharacter(GridCharacter c)
        {
            if (characters.Contains(c))
                characters.Remove(c);
        }
    }
}