using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.TurnManager;

//this script executes a state and passes the other objects we need.

namespace RPG.StateMachine
{
    public abstract class StateActions
    {
        public abstract void Execute(StateManager states, SessionManager sm, Turn t);
    }
}
