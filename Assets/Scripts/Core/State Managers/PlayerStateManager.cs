using UnityEngine;
using RPG.Grid;
using RPG.Interfaces;
using RPG.Actions;
using RPG.Vars;

namespace RPG.StateMachine 
{
    public class PlayerStateManager : StateManager 
    {
        public override void Init()
        {
            VariablesHolder gameVars = Resources.Load("GameVariables") as VariablesHolder;
            State interactions = new State();
            interactions.actions.Add(new InputManager(gameVars));
            interactions.actions.Add(new HandleMouseInteractions());
            interactions.actions.Add(new MoveCameraTransform(gameVars));

            State wait = new State();
            State moveonPath = new State();
            moveonPath.actions.Add(new MoveCharacterOnPath());
            currentState = interactions;

            startingState = interactions;

            allStates.Add("interactions", interactions);
            allStates.Add("wait", wait);
            allStates.Add("move", moveonPath);
        }


    }
}