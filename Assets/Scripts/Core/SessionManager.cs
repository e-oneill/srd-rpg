using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Grid;
using RPG.Explore;
using RPG.Characters;
using RPG.Core;

namespace RPG.TurnManager
{
public class SessionManager : MonoBehaviour

    {
        int turnIndex;
        public Turn[] turns;
        [SerializeField] public bool initiateTurn;
        public bool turnCombat = false;
        public bool unitsPlaced = false;
        public Transform gridObject;
        public GridCharacter activeChar;

        public GridManager gridManager;

        public float delta;
        public LineRenderer pathViz;
        public Node storedCharacterLoc;
        
        bool isPathfinding;
        bool isInit;

        #region Init
        private void Start()
        {
            gridManager.Init();
            
            InitStateManagers();
            if (initiateTurn )
            {
            placeUnits();
            }
            isInit = true;
        }

        private void LateUpdate()
        {
            if (initiateTurn && !turnCombat)
            {
            placeUnits();
            }
            if (!initiateTurn)
            {
                ExitTurnMode();
            }
        }

        public void InitTurnMode()
        {
            initiateTurn = true;
            placeUnits();
        }

        public void ExitTurnMode()
        {
            initiateTurn = false;
            turnCombat = false;
            unitsPlaced = false;
            GridCharacter[] units = GameObject.FindObjectsOfType<GridCharacter>();
            foreach (GridCharacter u in units)
            {
                PlayerController playerController = u.GetComponent<PlayerController>();
                playerController.isTurnCombat = false;
            }
        }

        void InitStateManagers()
        {
            foreach (Turn t in turns)
            {
                t.player.Init();
            }
        }
        public void placeUnits()
        {   
            unitsPlaced = false;
            GridCharacter[] units = GameObject.FindObjectsOfType<GridCharacter>();
            foreach (GridCharacter u in units)
            {
                u.OnInit();
                Node n;
                Vector3 CharacterPositionHelper = u.transform.position;
                
                // Debug.Log(CharacterPositionHelper);
                turnCombat = true;

                n = gridManager.GetNode(CharacterPositionHelper);
                //Arbitrarily subtracting 1 for the y coordinates of the node the character is believed to be in. Believe this is necessary because pivot point of character model used during testing and dev is in center.
                // Debug.Log(gridManager + " " + n.y);
                n.y = n.y-gridManager.characterYOffset;

                
                if (n != null)
                {
                    if (n.character == null)
                    {

                    }
                    else
                    {
                        List<Node> Neighbours = GetNeighbours(n);
                        foreach (Node neighbour in Neighbours)
                        {
                            if (neighbour.isWalkable && neighbour.character == null)
                            {
                                n = neighbour;
                                break;
                            }
                        }
                    }
                    Vector3 CharacterPlacer = n.worldPosition;
                    //Doing the offset because of the strange 1 unit offset with the grid location find.
                    // CharacterPlacer.x += gridManager.globalOffset;
                    // CharacterPlacer.z += gridManager.globalOffset;
                    CharacterPlacer.y += gridManager.gridStartPosYOffset;
                    u.transform.position = CharacterPlacer;
                    n.character = u;
                    PlayerController playerController = u.GetComponent<PlayerController>();
                    ActionScheduler actionScheduler = u.GetComponent<ActionScheduler>();
                    actionScheduler.StopAction();
                    u.GetComponent<Animator>().SetFloat("ForwardSpeed", 0f);
                    playerController.isTurnCombat = true;
                    gridManager.SetNodeCharacter(n.x, n.y, n.z, u);
                    // Debug.Log ("Character should be placed on the Grid");
                    u.currentNode = n;
                    Debug.Log("Character: " + n.character + "Character Location in grid: " + n.x + ", " + n.z + ", " + n.y);
                    storedCharacterLoc = n;
                    
                }
            }
            unitsPlaced = true;
        }

        #endregion

        #region Pathfinding Calls
        public void PathfinderCall(GridCharacter c, Node targetNode)
        {
            if (!isPathfinding)
            {
                isPathfinding = true;
                
                
                Node start = c.currentNode;
                Node target = targetNode;
                // Debug.Log("Current Node attempting Pathfinder Call: " + start + "Target Node:" + targetNode);
                if (start != null && target !=null)
                {
                
                PathfinderMaster.singleton.RequestPathfind(c, start, target, PathfinderCallback, gridManager);
                }
                else
                {
                    isPathfinding = false;
                }
            }


        }

        void PathfinderCallback(List<Node> p, GridCharacter character)
        {
            // Debug.Log("Pathfinder Callback Called. List:" + p + " Character:" + character);

            isPathfinding = false;
            if (p == null)
            {
                Debug.LogWarning("Path is not valid");
                return;
            }
            else
            {
                // Debug.Log("Path returned.");
            }
            Vector3 offset = Vector3.right *.2f + Vector3.down * .8f;
            pathViz.positionCount = p.Count + 1;
            if (pathViz.positionCount > character.moveRemaining)
            {
                // Debug.Log("Path was longer than character's movement. Limiting to character speed of " + character.racialsBlock.charMovement*5);
                pathViz.positionCount = character.moveRemaining + 1;
            }



            List<Vector3> allPositions = new List<Vector3>();
            allPositions.Add(character.currentNode.worldPosition + offset);
            for (int i = 0; i < p.Count; i++)
            {
                allPositions.Add(p[i].worldPosition + offset);
            }
            
            character.LoadPath(p);

            pathViz.SetPositions(allPositions.ToArray());

        }

        public void ClearPath()
        {
            pathViz.positionCount = 0;
        }
        #endregion
        
        

        // public Node currentNode;

       #region Frame Update
        private void Update()
        {
            delta = Time.deltaTime;
            if (!isInit) return;
            
            if (turns[turnIndex].Execute(this))
            {
                turnIndex++;
                if(turnIndex > turns.Length-1)
                {
                    turnIndex = 0;
                }
            }

            if (!unitsPlaced && turnCombat)
            {
                placeUnits();
                InitStateManagers();
            }
        }


        #endregion
       public List<Node> GetNeighbours(Node node)
        {
            List<Node> retList = new List<Node>();

            for (int x = -1; x <= 1; x++ )
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0) continue;

                    int _x = x + node.x;
                    int _y = node.y;
                    int _z = z + node.z;
                    if (gridManager.GetNode(_x, _y, _z) != null) 
                    {
                    // Debug.Log("Calling GetNode on" + _x + ", " + _z + ", " + _y);
                    Node n = gridManager.GetNode(_x, _y , _z);
                    
                    Node newNode = GetNeighbour(n);
                    
                    if (newNode != null)
                    {
                        //Bottom Left
                        if (x == -1 && z == -1)
                        {
                            if(newNode.walls.Contains(2) && newNode.walls.Contains(1) || node.walls.Contains(-2) && node.walls.Contains(-1))
                            {
                                continue;
                            }
                        }
                        
                        if (x == 1 && z == -1)
                        {
                            if(newNode.walls.Contains(-2) && newNode.walls.Contains(1) || node.walls.Contains(2) && node.walls.Contains(-1))
                            {
                                continue;
                            } 
                        }
                        if (x == 1 && z == 1)
                        {
                            if(newNode.walls.Contains(-2) && newNode.walls.Contains(-1) || node.walls.Contains(2) && node.walls.Contains(1))
                            {
                                continue;
                            }
                        }
                        if (x == -1 && z == 1)
                        {
                            if(newNode.walls.Contains(2) && newNode.walls.Contains(-1) || node.walls.Contains(-2) && node.walls.Contains(1))
                            {
                                continue;
                            } 
                        }
                        retList.Add(newNode);
                    }
                    }
                }
            }
            // Debug.Log(retList.Count);
            return retList;
        }

        Node GetNeighbour(Node currentNode)
        {   
            Node retVal = null;
            if (currentNode.isWalkable){

                Node aboveNode = gridManager.GetNode(currentNode.x, currentNode.y + 1, currentNode.z);
                if (aboveNode == null || aboveNode.isAir)
                {
                    
                }
                    retVal = currentNode;
                // Debug.Log("Found Node: " + retVal.x + ", " + retVal.z + ", " + retVal.z);
            }
            return retVal;
        }
    
    }
}
