using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.TurnManager;

//this defines the State clase that we will use in other scripts.
namespace RPG.StateMachine 
{
    
    public class State
    {
        public List<StateActions> actions = new List<StateActions>();

        public void Tick(StateManager states, SessionManager sm, Turn t)
        {
            if (states.forceExit) return;

            for (int i=0; i < actions.Count; i++)
            {
                actions[i].Execute(states,sm,t);
            }
        }
    }

}
