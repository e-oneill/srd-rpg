using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Grid;
using RPG.TurnManager;
using RPG.Characters;

//this is the state manager script. It manages the finite state machine and mackes sure that states are gotten, set and the tick is updated on time.deltatime. 


namespace RPG.StateMachine
{
    public abstract class StateManager : MonoBehaviour
    {
        public State currentState;
        public State startingState;
        public bool forceExit;
        public Node currentNode;
        public Node prevNode;
        public float delta;
        public PlayerHolder playerHolder;
        
        
        public GridCharacter currentCharacter{
            get {
                return _currentCharacter;
            }
            set {
                if (_currentCharacter != null)
                {
                    _currentCharacter.OnDeselect(playerHolder);
                }

                _currentCharacter = value;
            }
        }
        GridCharacter _currentCharacter;



        protected Dictionary<string, State> allStates = new Dictionary<string, State>();

        private void Start() 
        {
            Init();
        }
        
        public abstract void Init();

        public void Tick(SessionManager sm, Turn turn)
        {
            delta = sm.delta;
            if (currentState != null)
            {
            currentState.Tick(this, sm, turn);
            }
            forceExit = false;
        }

        
        public void SetState(string id)
        {
            State targetState = GetState(id);
            if (targetState == null)
            {
                Debug.LogError("State with id: " + id + " cannot be found. Check state ids.");
            }

            currentState = targetState;
        }

        public void ToStartingState()
        {
            currentState = startingState;
        }

        State GetState(string id)
        {
            State result = null;
            allStates.TryGetValue(id, out result);
            return result;
        }
    }
}
