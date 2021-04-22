using UnityEngine;
using UnityEngine.AI;
using RPG.StateMachine;
using RPG.Grid;
using RPG.TurnManager;
using RPG.Characters;

namespace RPG.Actions
{
        public class MoveCharacterOnPath : StateActions 
        {

            bool isInit;
            float t;
            float RotT;
            float speed;
            Node start;
            Node target;
            Quaternion targetRotation;
            Quaternion startRotation;
            int index;
            
            


            public override void Execute(StateManager states, SessionManager sessionManager, Turn turn)
            {
                // GridManager gridManager;
                GridCharacter character = states.currentCharacter;
                float movement = states.currentCharacter.racialsBlock.charMovement;

                if (!isInit)
                {
                    
                    if (index > character.currentPath.Count-1 || character == null || character.moveRemaining == 0)
                    {
                        // Debug.Log("returning to start state");
                        states.ToStartingState();
                        return;
                    }
                    isInit = true;
                    // Debug.Log("Move Action initialised.");
                    start = character.currentNode;
                    // Debug.Log("Starting at: " + start.x + ", " + start.z);
                    target = character.currentPath[index];
                    // Debug.Log("Moving to: " + target.x + ", " + target.z);
                    float t_ = t-1;
                    t_ = Mathf.Clamp01(t_);
                    t = t_;
                    float distance = Vector3.Distance(start.worldPosition, target.worldPosition);
                    if (character.currentPath.Count < 3) 
                    {
                        speed = character.racialsBlock.moveSpeed / distance * .5f;
                    }
                    else {
                        speed = character.racialsBlock.moveSpeed / distance;
                    }
                    

                    Vector3 direction = target.worldPosition - start.worldPosition;
                    targetRotation = Quaternion.LookRotation(direction);
                    startRotation = character.transform.rotation;
                    character.PlayAnimation("Locomotion");
                    
                }

                t += states.delta * speed;
                RotT += states.delta * character.racialsBlock.moveSpeed * 2;
                if (RotT > 1)
                {
                    RotT = 1;
                }

                if (t > 1)
                {
                    // Debug.Log("T is greater that one");
                    isInit = false;
                    character.currentNode = target;
                    
                    
                    RPG.Grid.Node checker = sessionManager.gridManager.GetNode(target.x, target.y, target.z);
                    index++;
                    if (index > character.currentPath.Count - 1 || index > movement - 1)
                    {
                        
                        // Debug.Log("I think character has finished path. Checking if character is succefully recorded on the right node: " + checker.character.name);
                        //Character has completed path.
                        t = 1;
                        index = 0;
                        sessionManager.gridManager.SetNodeCharacter(target.x, target.y, target.z, character);
                        sessionManager.gridManager.SetNodeCharacter(start.x, start.y, start.z, null);
                        if (states.currentCharacter.moveRemaining >= character.currentPath.Count)
                        {
                            states.currentCharacter.moveRemaining +=  - character.currentPath.Count;
                        }
                        else 
                        {
                            states.currentCharacter.moveRemaining = 0;
                        }
                        states.ToStartingState();
                        character.PlayAnimation("Locomotion");
                        
                        
                    }
                }

                Vector3 tp = Vector3.Lerp(start.worldPosition + Vector3.down, target.worldPosition + Vector3.down, t);
                
                character.transform.position = tp;
                if (character.currentPath.Count < 3){
                    character.anim.SetFloat("ForwardSpeed", 1.7f);
                }
                else
                {
                    character.anim.SetFloat("ForwardSpeed", 5.66f);
                }
                
                character.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, RotT);
                if (character.transform.position == target.worldPosition + Vector3.down)
                {
                    character.anim.SetFloat("ForwardSpeed", 0);
                    character.GetComponent<Animator>().SetFloat("ForwardSpeed", 0);
                }


                // Debug.Log("Movement Remaining this turn:" + states.currentCharacter.moveRemaining);
                


                
            }




        }
}