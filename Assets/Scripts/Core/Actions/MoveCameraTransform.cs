using UnityEngine;
using Cinemachine;
using RPG.StateMachine;
using RPG.Grid;
using RPG.TurnManager;
using RPG.Vars;

namespace RPG.Actions 

// Moves the camera to follow input that is imported from the Variables Holder object. The Variables Holder is defined somewhere else, e.g. PlayerStateManager.cs, and we get it when calling the function.

{
    public class MoveCameraTransform : StateActions 
    {
        TransformVariable cameraTransform;
        FloatVariable horizontal;
        FloatVariable vertical;
        FloatVariable zoom;
        FloatVariable rotateInput = null;
        public CinemachineBrain brain;
        VariablesHolder varHolder;
        public CinemachineVirtualCamera follow;

        public MoveCameraTransform(VariablesHolder holder)
        {
            varHolder = holder;
            // Debug.Log(varHolder);
            cameraTransform = varHolder.cameraTransform;
            horizontal = varHolder.horizontalInput;
            vertical = varHolder.verticalInput;
            zoom = varHolder.zoomInput;
            rotateInput = varHolder.rotateInput;
            
            
        }

        private void Update() {
            Camera.main.TryGetComponent<CinemachineBrain>(out CinemachineBrain brain);
            if (Input.GetKeyDown(KeyCode.W))
            {
                brain.enabled = !brain.enabled;
            }
        }

        public override void Execute(StateManager states, SessionManager sm, Turn t)
        {


            // Vector3 tp = cameraTransform.value.forward * (vertical.value * varHolder.cameraMoveSpeed);
            // tp += cameraTransform.value.right * (horizontal.value * varHolder.cameraMoveSpeed);
            // tp += cameraTransform.value.up * (zoom.value * varHolder.cameraZoomSpeed);
            // if (cameraTransform.value.TryGetComponent<CinemachineVirtualCamera>(out follow))
            // {
                follow = cameraTransform.value.GetComponentInChildren<CinemachineVirtualCamera>();
                Transform followChild = follow.transform.GetChild(0);
                Vector3 tp = followChild.transform.up * (vertical.value * varHolder.cameraMoveSpeed);
                // if (tp != null)
                // {
                //     // Debug.Log("Vector3 init \n Y value: " + tp.y);
                // }
                tp += follow.transform.right * (horizontal.value * varHolder.cameraMoveSpeed);
                // Debug.Log(follow);
                
                float currentsize = follow.m_Lens.OrthographicSize;
                float newSize = currentsize + zoom.value * varHolder.cameraZoomSpeed * states.delta;
                if (Mathf.FloorToInt(newSize) > 0)
                {
                    if (Mathf.FloorToInt(newSize) > 10)
                    {
                        newSize = 10f;
                    }

                } 
                else
                {
                    newSize = 1f;
                }
                
                follow.m_Lens.OrthographicSize = newSize;
                // Debug.Log(currentsize);
            // }

            // cameraTransform.value.position += tp * states.delta;
            follow.transform.position += tp * states.delta;
            
            
            

            if (rotateInput.value != 0f) 
            {
                // cameraTransform.value.Rotate(0f,rotateInput.value += states.delta,0f, Space.Self);
                follow.transform.Rotate(0f,rotateInput.value += states.delta,0f, Space.World);
                // Debug.Log("Rotation block successfully called.");
            }
            else 
            {
                // Debug.Log("Rotate Input is " + rotateInput.value);
            }

        }
    }
}