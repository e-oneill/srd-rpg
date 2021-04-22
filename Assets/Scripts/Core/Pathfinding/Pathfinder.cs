using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Grid 
{
    public class Pathfinder 
    {
        GridManager gridManager;
        GridCharacter character;
        Node startNode;
        Node endNode;
        
        
        //variable declared as volatile for multithreading support
        public volatile bool jobDone = false;
        public volatile float timer;
 
        public int MOVE_DIAGONAL_COST = 15;
        public int MOVE_STRAIGHT_COST = 10;

        public delegate void PathfindingComplete(List<Node> n, GridCharacter character);
        public PathfindingComplete completeCallback;
        List<Node> targetPath;

        public Pathfinder(GridCharacter c, Node start, Node target, PathfindingComplete callback, GridManager gridManager)
        {
            this.gridManager = gridManager;
            character = c;
            startNode = start;
            // Debug.Log("Start Node of Path:" + start.x + ", " + start.z + ", " + start.y);
            endNode = target;
            // Debug.Log("End Node of Path: " + target.x + ", " + target.z + ", " + target.y);
            completeCallback = callback;
        }

        public void FindPath()
        {
            targetPath = FindPathActual();
            jobDone = true;
        }

        public void NotifyComplete()
        {
            if (completeCallback != null)
            {
                completeCallback(targetPath, character);
            }
        }

        List<Node> FindPathActual()
        {
             List<Node> foundPath = new List<Node>();
             List<Node> openSet = new List<Node>();
             HashSet<Node> closedSet = new HashSet<Node>();

             openSet.Add(startNode);
            //  Debug.Log(openSet[0]);

             while (openSet.Count > 0)
             {
                 Node currentNode = openSet[0];
                    for (int i = 0; i < openSet.Count; i++)
                    {

                        if(openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                        {
                            if(!currentNode.Equals(openSet[i]))
                            {
                                currentNode = openSet[i];
                            }
                        }
                    }
                    openSet.Remove(currentNode);
                    closedSet.Add(currentNode);

                    if(currentNode.Equals(endNode))
                    {
                        foundPath = RetracePath(startNode, currentNode);
                        // Debug.Log("EndNode Reached.");
                        return foundPath;
                        
                    }

                    foreach (Node neighbour in GetNeighbours(currentNode))
                    {
                        if (!closedSet.Contains(neighbour))
                        {
                            float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                            if (currentNode.walls.Contains(1) && neighbour.walls.Contains(-1) && currentNode.z != neighbour.z || currentNode.walls.Contains(-1) && neighbour.walls.Contains(1) && currentNode.z != neighbour.z)
                            {
                                newMovementCostToNeighbour = float.MaxValue;
                                Debug.Log("Path blocked by wall");
                                // return null;
                            }
                            if (currentNode.walls.Contains(2) && neighbour.walls.Contains(-2) && currentNode.x != neighbour.x || currentNode.walls.Contains(-2) && neighbour.walls.Contains(2) && currentNode.x != neighbour.x)
                            {
                                newMovementCostToNeighbour = float.MaxValue;
                                Debug.Log("Path blocked by wall");
                                // return null;
                            }


                            if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour) && newMovementCostToNeighbour != float.MaxValue)
                            {
                                neighbour.gCost = newMovementCostToNeighbour;
                                neighbour.hCost = GetDistance(neighbour,endNode);
                                neighbour.parentNode = currentNode;
                                if (!openSet.Contains(neighbour))
                                {
                                    openSet.Add(neighbour);
                                }
                            }
                        }
                    }

            }
            return null;
        }
        
        int GetDistance(Node posA, Node posB)
        {
            int distX = Mathf.Abs(posA.x - posB.x);
            int distZ = Mathf.Abs(posA.z - posB.z);
            int distY = Mathf.Abs(posA.y - posB.y);

            // if (distX > distZ)
            // {
            //     return 14 * distZ + 10 * distX - distZ;
            // }


            int xDistance = Mathf.Abs(posA.x - posB.x);
            int zDistance = Mathf.Abs(posA.z - posB.z);
            int remaining = Mathf.Abs(distX - distZ);
            return MOVE_DIAGONAL_COST * Mathf.Min(distX, distZ) + MOVE_STRAIGHT_COST * remaining;

            // return 14 * distZ + 10 * distZ - distX;
        }
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> retList = new List<Node>();

            for (int x = -1; x <= 1; x++ )
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0) continue;

                    int _x = x + node.x;
                    int _y = startNode.y;
                    int _z = z + node.z;
                    if (GetNode(_x, _y, _z) != null) 
                    {
                    // Debug.Log("Calling GetNode on" + _x + ", " + _z + ", " + _y);
                    Node n = GetNode(_x, _y , _z);
                    
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



        Node GetNode(int x, int y, int z)
        {
            return gridManager.GetNode(x, y, z);
        }

        Node GetNeighbour(Node currentNode)
        {   
            Node retVal = null;
            if (currentNode.isWalkable){

                Node aboveNode = GetNode(currentNode.x, currentNode.y + 1, currentNode.z);
                if (aboveNode == null || aboveNode.isAir)
                {
                    
                }
                    retVal = currentNode;
                // Debug.Log("Found Node: " + retVal.x + ", " + retVal.z + ", " + retVal.z);
            }
            return retVal;
        }

        List<Node> RetracePath(Node start, Node end)
        {
            List<Node> path = new List<Node>();

            Node currentNode = endNode;
            while (currentNode != start) 
            {
                path.Add(currentNode);
                currentNode =  currentNode.parentNode;
                // Debug.Log("Step in Path: " + currentNode.x + ", " + currentNode.y + ", " + currentNode.z);
            }

            path.Reverse();
            return path;
        }
    }
}