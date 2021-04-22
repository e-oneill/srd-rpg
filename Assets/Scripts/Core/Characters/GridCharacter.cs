using UnityEngine;
using System.Collections.Generic;
using RPG.TurnManager;
using RPG.Interfaces;
using RPG.Grid;
using Cinemachine;
using RPG.Saves;


namespace RPG.Characters
{


    public class GridCharacter : MonoBehaviour , ISelectable, IDeselect, IHighlight, IDeHighlight, IDetectable, ISavable {
        public PlayerHolder owner;
        
        public GameObject highlighter;
        public bool isSelected;



        #region Creating Character Variables
        #region Basic Setup
        [System.Serializable]
        public enum charType {
            PlayerCharacter,
            NonPlayerCharacter
        }

        public enum Faction
        {
            Player,
            Neutral,
            Enemy
        }

        public Faction faction;

        public charType CharacterType;
        public int level;
        public int XP;
        public int currentHP;
        public int maxHP;
        public CharacterClasses charClass;
        // public enum charClass {
        //     Fighter,
        //     Rogue,
        //     Ranger,
        //     Wizard,
        //     Cleric
        // }

        // public charClass characterClass;
        
        [System.Serializable]
        public class AbilitiesBlock
        {
            public int strength = 10;
            public int dexterity = 10;
            public int constitution = 10;
            public int intelligence = 10;
            public int wisdom = 10;
            public int charisma = 10;
        }
        public AbilitiesBlock abilitiesBlock;
        public int [,] abilitiesArray;
        


        public RacialsBlock racialsBlock;

        [System.Serializable]
        public class RacialsBlock
        {
            public float moveSpeed;
            public int charMovement;
            public string charMoveType;
        }

        public int moveRemaining;
    

        #endregion
        #endregion

        [HideInInspector]
        public List<Node> currentPath;
        [HideInInspector]
        public Node currentNode;
        public void LoadPath(List<Node> path)
        {
            currentPath = path;
        }
        
        private void Awake() {
            abilitiesArray = new int[6,6];
            PopulateAbilitiesArray();
        }



        public void OnInit()
        {
            owner.RegisterCharacter(this);
            highlighter.SetActive(false);
            moveRemaining = racialsBlock.charMovement;
            anim = GetComponentInChildren<Animator>();
            #region initialise level and hp
            if (level==1)
            {
                maxHP = (int)charClass.hitdice + abilitiesArray[2,1];
                currentHP = maxHP;
            }
            else
            {
                int conMod = abilitiesArray[2,1];
                int hitDie = (int)charClass.hitdice;
                maxHP = hitDie + conMod;
                int hpPerLevel = Mathf.CeilToInt(hitDie/2) + conMod;
                maxHP += hpPerLevel * level;
                currentHP = maxHP;
            }
            #endregion
        
        }

        public void PlayAnimation(string targetAnim)
        {
            anim.CrossFade(targetAnim, 0.2f);
        }

        #region Interfaces
        public void OnSelect(PlayerHolder player)
        {
            highlighter.SetActive(true);
            isSelected = true;
            player.stateManager.currentCharacter = this;
            CinemachineVirtualCamera follow = FindObjectOfType<CinemachineVirtualCamera>();
            follow.Follow = this.transform;
        }

        public void OnDeselect(PlayerHolder player)
        {
            highlighter.SetActive(false);
            isSelected = false;
        }

        public void OnHighlight(PlayerHolder player)
        {
            highlighter.SetActive(true);
        }

        public void OnDeHighlight(PlayerHolder player)
        {
            if (!isSelected)
            {
            highlighter.SetActive(false);
            }
        }

        public Node OnDetect()
        {
            return currentNode;
        }

        #endregion;

        public void PopulateAbilitiesArray()
        {

            abilitiesArray[0, 0] = abilitiesBlock.strength;
            abilitiesArray[1, 0] = abilitiesBlock.dexterity;
            abilitiesArray[2, 0] = abilitiesBlock.constitution;
            abilitiesArray[3, 0] = abilitiesBlock.intelligence;
            abilitiesArray[4, 0] = abilitiesBlock.wisdom;
            abilitiesArray[5, 0] = abilitiesBlock.charisma;

        for (int i = 0; i < 6; i++)
        {
            if (abilitiesArray[i,0] > 29)
             {
                 abilitiesArray[i,1] = 10;
                 continue;
             }
            if (abilitiesArray[i,0] > 27 )
             {
                 abilitiesArray[i,1] = 9;
                 continue;
             }
            if (abilitiesArray[i,0] > 25)
             {
                 abilitiesArray[i,1] = 8;
                 continue;
             }
            if (abilitiesArray[i,0] > 23)
             {
                 abilitiesArray[i,1] = 7;
                 continue;
             }
            if (abilitiesArray[i,0] > 21)
             {
                 abilitiesArray[i,1] = 6;
                 continue;
             }
            if (abilitiesArray[i,0] > 19)
             {
                 abilitiesArray[i,1] = 5;
                 continue;
             }
            if (abilitiesArray[i,0] > 17)
             {
                 abilitiesArray[i,1] = 4;
                 continue;
             }
            if (abilitiesArray[i,0] > 15)
             {
                 abilitiesArray[i,1] = 3;
                 continue;
             }
            if (abilitiesArray[i,0] > 13)
             {
                 abilitiesArray[i,1] = 2;
                 continue;
             }
            if (abilitiesArray[i,0] > 11)
             {
                 abilitiesArray[i,1] = 1;
                 continue;
             }
            if (abilitiesArray[i,0] > 9)
             {
                 abilitiesArray[i,1] = 0;
                 continue;
             }
            if (abilitiesArray[i,0] > 7)
             {
                 abilitiesArray[i,1] = -1;
                 continue;
             }
            if (abilitiesArray[i,0] > 5)
             {
                 abilitiesArray[i,1] = -2;
                 continue;
             }
            if (abilitiesArray[i,0] > 3)
             {
                 abilitiesArray[i,1] = -3;
                 continue;
             }
            if (abilitiesArray[i,0] > 1)
             {
                 abilitiesArray[i,1] = -4;
                 continue;
             }
            if (abilitiesArray[i,0] < 2)
             {
                 abilitiesArray[i,1] = -5;
                 
             }
             
        }
        // for (int i = 0; i < 6; i++)
        // {
        //     // Debug.Log("Ability Score: " + abilitiesArray[i,0] + "Modifier: " + abilitiesArray [i,1]);
        // }
        return;
        }

        public Animator anim;


        public void RestoreState(object state)
        {
            currentHP = (int)state;

            if (currentHP <= 0)
            {
                //Character is dead.
            }
        }

        public object CaptureState()
        {
            return currentHP;
        }


    }
}