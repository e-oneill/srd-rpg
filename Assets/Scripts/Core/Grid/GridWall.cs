using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Grid
{
public class GridWall : MonoBehaviour
{
    public bool isWall = true;
    [System.Serializable]
    public enum WallAxis {
        xAxis,
        zAxis
    } 

    
    public WallAxis wallAxis;

}

}