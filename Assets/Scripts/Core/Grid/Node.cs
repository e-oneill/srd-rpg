using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Grid
    {
    public class Node
    {
        public int x;
        public int y;
        public int z;

        public bool isWalkable;
        public bool isAir;
        public Vector3 worldPosition;
        // public Vector3 floorPosition;
        public GridObject obstacle;
        public List<int> walls = new List<int>();
        public GameObject tileViz;
        public GridCharacter character;
        public float hCost;
        public float gCost;
        public float fCost
        {
            get {
                return gCost + hCost;
            }
        }

        public Node parentNode;

    }

}
