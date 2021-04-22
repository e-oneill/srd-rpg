using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 15;
    public Pathfinding(int width, int height, int cellSize, int mapSize) {
        grid = new Grid<PathNode>(width, height, cellSize, mapSize, (Grid<PathNode> g, int x, int z) => new PathNode(g, x, z));

    }

    public Grid<PathNode> GetGrid(){
        return grid;
    }

    public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ){
        PathNode startNode = grid.GetGridObject(startX, startZ);
        PathNode endNode = grid.GetGridObject(endX, endZ);

        openList = new List<PathNode> {startNode};
        closedList = new List<PathNode>();

        for (int x=0;x<grid.GetWidth(); x++) {
            for (int z=0;z<grid.GetHeight();z++){
                PathNode pathNode = grid.GetGridObject(x,z);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0){
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                //Reached Final Node
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)){
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable) {
                        closedList.Add(neighbourNode);
                        continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode)){
                        openList.Add(neighbourNode);
                    }
                
                }

            }
        }

        // Out of Algo loop
        return null;

    }

    private List<PathNode> GetNeighbourList (PathNode currentNode){
        List<PathNode> neighbourList = new List<PathNode>();


        if (currentNode.x - 1 >= 0) {
            //Left
            neighbourList.Add(GetNode(currentNode.x -1, currentNode.z));
            //Left Down
            if (currentNode.z-1 >= 0) neighbourList.Add(GetNode(currentNode.x-1, currentNode.z -1));
            //left Up
            if (currentNode.z + 1 > grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x -1, currentNode.z +1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            //Right
            neighbourList.Add(GetNode(currentNode.x +1, currentNode.z));
            //Right Down
            if (currentNode.z - 1 >= 0) neighbourList.Add(GetNode(currentNode.x+1, currentNode.z-1));
            //Right Up
            if (currentNode.z+1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x+1, currentNode.z+1));
        }
        //Down
        if (currentNode.z-1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.z - 1));
        //Up
        if (currentNode.z +1 < grid.GetHeight ()) neighbourList.Add(GetNode(currentNode.x, currentNode.z+1));

        return neighbourList;


    }

    private PathNode GetNode(int x, int z){
        return grid.GetGridObject(x,z);
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode; 
        while (currentNode.cameFromNode != null ) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        foreach (var node in path) {
            Debug.Log(node);
        }
        // Debug.Log(path);
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;

    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList){
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count;i++){
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;

    }
    

}
