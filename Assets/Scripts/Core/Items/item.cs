using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "item", menuName = "RPG Game/item", order = 0)]
public class item : ScriptableObject {
    public enum itemtype 
    {
        weapon,
        armor,
        consumable
    }
    [System.Serializable]
    public class ItemGeneralParams
    {
        public Sprite itemIcon;
    }

    public ItemGeneralParams generalParams;

    public itemtype itemType;

    [System.Serializable]
    public class weaponParams 
    {
        public enum WeaponType
        {
            SimpleMeleeWeapon,
            MartialMeleeWeapon,
            SimpleRangedWeapon,
            MartialRangedWeapon,
            ImprovisedWeapon
        }

        public WeaponType weaponType;

        public bool isMagical;
        public enum WeaponDamageDie
        {
            d4 = 4,
            d6 = 6,
            d8 = 8,
            d10 = 10,
            d12 = 12
        }
        public WeaponDamageDie weaponDamageDie;

        public enum MeleeReach
        {
            five_feet = 5,
            ten_feet = 10,
            fifteen_feet = 15
        }

        public MeleeReach meleeReach;

        public int weaponDamageDieCount = 1;

        public bool isVersatile;
        [System.Serializable]
        public class VersatileParams
        {
            public enum VersatileDamageDie
            {
                d4 = 4,
                d6 = 6,
                d8 = 8,
                d10 = 10,
                d12 = 12               
            }

            public VersatileDamageDie versatileDamageDie;
            
            public int versatileDamageDieCount;
        }

        public VersatileParams versatileParams;
        
        public class MagicWeaponParams
        {
            public int attackbonus;
            public int damagebonus;
        }
    }

    public weaponParams WeaponParams;
    
}