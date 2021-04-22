using UnityEngine;

namespace RPG.Vars
{
    [CreateAssetMenu(fileName = "TransformVariable", menuName = "RPG Game/Variable/Transform", order = 0)]
    public class TransformVariable : ScriptableObject 
    {
        public Transform value;
    }
}