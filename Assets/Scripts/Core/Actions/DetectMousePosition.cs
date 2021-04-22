using UnityEngine;
using RPG.StateMachine;
using RPG.TurnManager;
using RPG.Interfaces;
using RPG.Explore.Movement;
using RPG.Characters;


// Tester Action that gets the node that the mouse pointer is currently in.

namespace RPG.Actions 
{

    public class HandleMouseInteractions : StateActions 
    {
        GridCharacter prevCharacter;

        public override void Execute(StateManager states, SessionManager sm, Turn t)
        {
            bool mouseClick = Input.GetMouseButtonDown(0);
            bool rightmouseClick = Input.GetMouseButtonDown(1);
            if (prevCharacter != null)
            {
                prevCharacter.OnDeHighlight(states.playerHolder);
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                RPG.Grid.Node n = sm.gridManager.GetNode(hit.point);
                // Debug.Log(n.character);
                if (n != null)
                {
                    //Highlighted your own unit
                    // Debug.Log("Node: " + n.x + ", " + n.z);
                    // if (n.x == sm.storedCharacterLoc.x && n.z == sm.storedCharacterLoc.z)
                    //     {
                    //     DumpToConsole(n);
                    //     }
                    IDetectable detectable = hit.transform.GetComponent<IDetectable>();
                    if (detectable != null) 
                    {
                        n = detectable.OnDetect();
                    }

                    if (sm.turnCombat==true)
                    {
                        if (n.character != null && n.character.owner == states.playerHolder)
                        {
                            n.character.OnHighlight(states.playerHolder);

                            prevCharacter = n.character;
                        }
                        else //Highlighted neutral or enemy unit.
                        {
                            
                        }
                        if(states.currentCharacter != null && n.character == null && !mouseClick && !rightmouseClick && sm.turnCombat == true)
                        {
                            // Debug.Log("Calling Path Detection in DetectMousePosition.cs");
                            PathDetection(states, n, sm, states.currentCharacter);
                            
                        }
                        else //No character selected.
                        {
                            if (mouseClick && sm.turnCombat == true)
                            {
                                // Debug.Log("Node Clicked:" + n.x + "," + n.z + "," + n.y);

                                if (n.character != null && n.character.owner == states.playerHolder)
                                {
                                    n.character.OnSelect(states.playerHolder);
                                    states.prevNode = null;
                                    sm.ClearPath();
                                }
                                else 
                                {
                                    // Debug.Log(states.currentCharacter.currentPath);
                                        if (states.currentCharacter.currentPath != null || states.currentCharacter.currentPath.Count > 0)
                                        {
                                            states.SetState("move");
                                        }
                                    
                                }
                            }
                            if (rightmouseClick)
                            {
                                Debug.Log("Node Clicked:" + n.x + "," + n.z + "," + n.y);
                            }
                        }
                    }
                    else
                    {
                        if (mouseClick)
                        {
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        
                        bool hasHit = Physics.Raycast(ray, out hit);
                        if (hasHit)
                        {
                            // GetComponent<Mover>().MoveTo(hit.point);
                            // GridCharacter character = RPG.Explore.PlayerController.MoveToCursor();
                        }
                        }
                        if (rightmouseClick)
                        {
                            
                        }
                    }
                }
            }
        }

        void PathDetection(StateManager states, RPG.Grid.Node n, SessionManager sm, GridCharacter c)
        {
            if (c.currentNode != null)
            {
                
                if (c.currentNode != states.prevNode || states.prevNode == null)
                {
                // Debug.Log(c.currentNode.x + ", " + c.currentNode.z);
                states.prevNode = states.currentNode;
                sm.PathfinderCall(c, n);
                }
            }
            else
            {
                Debug.Log("Current Node is not set");
            }
        }
        public static void DumpToConsole(RPG.Grid.Node n)
        {
            var output = JsonUtility.ToJson(n, true);
            Debug.Log(output);
        }
    }

    
}