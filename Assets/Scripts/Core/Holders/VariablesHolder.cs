using UnityEngine;
namespace RPG.Vars
{
    [CreateAssetMenu(menuName = "RPG Game/Variables/Game Variables Holder")]
    public class VariablesHolder : ScriptableObject 
    {
        public float cameraMoveSpeed = 15;
        public float cameraZoomSpeed = 80;

        [Header("Scriptable Variables")]
        #region Scriptables
        public TransformVariable cameraTransform;
        public FloatVariable horizontalInput;
        public FloatVariable verticalInput;
        public FloatVariable zoomInput;
        public FloatVariable rotateInput;
        #endregion 
        
    }
}