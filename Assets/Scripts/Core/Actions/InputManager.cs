using UnityEngine;
using RPG.StateMachine;
using RPG.Grid;
using RPG.TurnManager;
using RPG.Vars;

namespace RPG.Actions

// Action that is used to manager user input for actions. Demonstration to allow the camera to be moved via the axes defined in Unity's Input Manager (see MoveCameraTransform.cs).

{
    public class InputManager : StateActions 
    {
        VariablesHolder varHolder;
        public InputManager(VariablesHolder holder)
        {
            varHolder = holder;
        }

        public override void Execute(StateManager states, SessionManager sessionManager, Turn t)
        {
            varHolder.horizontalInput.value = Input.GetAxis("Horizontal");
            varHolder.verticalInput.value = Input.GetAxis("Vertical");
            varHolder.zoomInput.value = Input.GetAxis("Mouse ScrollWheel") * -1;
            varHolder.rotateInput.value = Input.GetAxis("RotateInput");
        }
    }
}