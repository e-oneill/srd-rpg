using UnityEngine;
using RPG.Grid;

namespace RPG.Characters
{
    [CreateAssetMenu(fileName = "Class", menuName = "RPG Game/Characters/Class", order = 0)]
    public class CharacterClasses : ScriptableObject {

        public string className;
        public enum classHitDie
        {
            d6 = 6,
            d8 = 8,
            d10 = 10,
            d12 = 12
        }

        public classHitDie hitdice;
        
    }
}