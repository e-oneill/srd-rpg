using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int z;
    public int gCost;
    public int hCost;
    public int fCost;
    public PathNode cameFromNode;
    public bool isWalkable;

    public PathNode(Grid<PathNode> grid, int x, int z){
        this.grid = grid;
        this.x = x;
        this.z = z;
        isWalkable = true;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
            return x + "," + z;
    }
}
