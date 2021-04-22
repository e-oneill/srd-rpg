using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.StateMachine;

namespace RPG.TurnManager
{
    [CreateAssetMenu(menuName = "RPG Game/Turn")]
    public class Turn : ScriptableObject
        {
            
            public PlayerHolder player;
            [System.NonSerialized]
            int index = 0;
            public Phase[] phases;


                        
            public bool Execute(SessionManager sm)
            {
                bool result = false;

                phases[index].OnStartPhase(sm, this);
                if (phases[index].IsComplete(sm, this))
                {
                    phases[index].OnEndPhase(sm, this);
                    index++;
                    if (index > phases.Length-1)
                    {
                        index = 0;
                        result = true;
                    }
                }

                return result;
            }

            public void EndCurrentPhase()
            {
                phases[index].forceExit = true;
            }
        }
    }