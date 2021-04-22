using UnityEngine;

namespace RPG.Vars {
    
        public class OnEnableAssignTransformVariable : MonoBehaviour {
            public TransformVariable targetVariable;
        

        private void Awake() 
        {
            targetVariable.value = this.transform;
            Destroy(this);
        }
    }

}