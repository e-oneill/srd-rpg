using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Grid
{

// Master script that initiates and manages the Grid. The Grid is defined via a GameObject holding this script, and there must also be children of that object carrying the GridPosition class to properly generate a Grid.

public class GridManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    public float xzScale = 2f;
    [SerializeField]
    float yScale = 2;
    Vector3 minPos;
    [SerializeField]
    public int ViewLevel = 0;
    [SerializeField]
    public int globalOffset = 1;
    [SerializeField]
    public int characterYOffset = 1;
    [SerializeField]
    public int gridStartPosYOffset = 0;
    


    int maxX;
    int maxZ;
    int maxY;
    public Node[,,] grid;
    public Node n;
    [SerializeField] public bool visualiseCollisions;
    public List<Vector3> nodeViz = new List<Vector3>();
    public Vector3 extends = new Vector3 (1.5f, 1.5f, 1.5f);
    GameObject tileContainer;

    int pos_x;
    int pos_y;
    int pos_z;

    #endregion

    public GameObject unit;
    public GameObject tileViz;

    public void Init() 
    {
        ReadLevel();
        Node n = GetNode(unit.transform.position);
        if (n != null)
        {
            unit.transform.position = n.worldPosition;
        }
    }

    void ReadLevel()
    {
        GridPosition[] gp = GameObject.FindObjectsOfType<GridPosition>();

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minZ = minX;
        float maxZ = maxX;
        float minY = minX;
        float maxY = maxX;
        

        for (int i=0; i < gp.Length; i++)
        {
            Transform t = gp[i].transform;

            #region Read Positions
            // Debug.Log(t.position.x);
            if(t.position.x < minX) 
            {
                minX = t.position.x;
                // Debug.Log(minX);
            }

            if (t.position.x > maxX)
            {
                maxX = t.position.x;
                // Debug.Log(maxX);
            }

            if (t.position.z < minZ)
            {
                minZ = t.position.z;
            }
            
            if (t.position.z > maxZ)
            {
                maxZ = t.position.z;
            }

            if (t.position.y < minY)
            {
                minY = t.position.y;
            }

            if (t.position.y > maxY)
            {
                maxY = t.position.y;
            }
            #endregion
        
        }
        pos_x = Mathf.FloorToInt((maxX - minX)/xzScale);
        pos_z = Mathf.FloorToInt((maxZ - minZ)/xzScale);
        pos_y = Mathf.FloorToInt((maxY - minY)/yScale);
        if (pos_y == 0)
        {
            pos_y = 1;
        }
        minPos = Vector3.zero;
        minPos.x = minX;
        minPos.z = minZ;
        minPos.y = minY;

        CreateGrid(pos_x,pos_z, pos_y);
        }

    public void CreateGrid(int pos_x, int pos_z, int pos_y)
        {
            GameObject tileContainer = new GameObject("tileContainer");
            grid = new Node[pos_x, pos_y, pos_z];
            for (int y=0; y < pos_y; y++)
            {
                for (int x=0;x < pos_x; x++)
                {
                    for (int z=0;z < pos_z; z++)
                    {
                        Node n = new Node();
                        n.x = x;
                        n.z = z;
                        n.y = y;


                        Vector3 tp = minPos;
                        tp.x += x * xzScale;
                        tp.z += z * xzScale;
                        tp.y += y * yScale;

                        n.worldPosition = tp;
                        Vector3 colliderTest = tp;
                        //For some reason, the grid generation is not interacting with well with Mesh Colliders.
                        //Some experimenting in the Editor found that offsetting the location of the mesh collider by 1, 1, 1 resulted in more correct results. 
                        //For now, using a simple workaround to do the offset in the script, but in long term should find a solution to this issue. 
                        colliderTest.x += 0;
                        colliderTest.z += 0;
                        colliderTest.y += -1;

                        //First we run a collider check for tileblockers. 
                        Collider[] overlapNode = Physics.OverlapBox(colliderTest, extends/2, Quaternion.identity);
                        if (overlapNode.Length > 0) 
                        {
                            bool isWalkable = false;

                            for (int i=0; i < overlapNode.Length; i++)
                            {
                                GridObject obj = overlapNode[i].transform.GetComponentInChildren<GridObject>();
                                
                                if (obj != null)
                                {
                                    if (obj.isWalkable && n.obstacle == null) 
                                    {
                                        isWalkable = true;
                                    }
                                    else
                                    {
                                        isWalkable = false;
                                        n.obstacle = obj;
                                    }
                                    n.isWalkable = obj.isWalkable;

                                }

                                n.isWalkable = isWalkable;
                            }

                            
                            if (n.isWalkable) 
                            {
                                RaycastHit hit;
                                Vector3 origin = n.worldPosition;
                                origin.y += yScale;
                                if (Physics.Raycast(origin, Vector3.down, out hit, yScale))
                                    {
                                        n.worldPosition = hit.point;
                                    }
                                    if (visualiseCollisions) 
                                    {

                                    //It appears that this is originating the tileViz game object from its centre. Need to check if we can originate from same pivot point of the grid. Probably an issue with the tileViz prefab itself.
                                    Vector3 tileVizPos = n.worldPosition;
                                    tileVizPos.x += +globalOffset -1 ;
                                    tileVizPos.z += +globalOffset -1 ;
                                    tileVizPos.y += +globalOffset -1 +.1f ;
                                    Debug.Log("Instantiating tileViz");
                                    GameObject go = Instantiate(tileViz, tileVizPos, Quaternion.identity) as GameObject;
                                    n.tileViz = go;
                                    go.transform.parent = tileContainer.transform;
                                    go.SetActive(true);
                                    }
                            }
                            else
                            {
                                if (n.obstacle = null)
                                {
                                    n.isAir = true;
                                }
                            }
                            if(n.y >= ViewLevel)
                            {
                                if (n.obstacle != null)
                                {
                                    nodeViz.Add(n.worldPosition);
                                }
                            }

                        }

                        //Our next check is for GridWall objects to populate the node's GridWall List.
                        Vector3 fullNode;
                        fullNode.x = xzScale -.1f;
                        fullNode.z = xzScale -.1f;
                        fullNode.y = 1;
                        overlapNode = Physics.OverlapBox(colliderTest, fullNode/2, Quaternion.identity);
                        if (overlapNode.Length > 0) 
                        {
                            

                            for (int i=0; i < overlapNode.Length; i++)
                            {
                                GridWall wall = overlapNode[i].transform.GetComponentInChildren<GridWall>();
                                
                                if (wall != null)
                                {
                                    // Debug.Log("We found a wall. Wall in node: " + n.x + ", " + n.z + ", " + n.y);
                                    Vector3 relativewallPos = n.worldPosition - wall.transform.position;
                                    // Debug.Log(relativewallPos);
                                    if (relativewallPos.z < 0)
                                    {
                                        n.walls.Add(1);
                                        // Debug.Log(wall.wallAxis);
                                    }
                                    if (relativewallPos.z > 0)
                                    {
                                        n.walls.Add(-1);
                                        // Debug.Log(wall.wallAxis);
                                    }
                                    if (relativewallPos.x < 0)
                                    {
                                        n.walls.Add(2);
                                        // Debug.Log(wall.wallAxis);
                                    }
                                    if (relativewallPos.x > 0)
                                    {
                                        n.walls.Add(-2);
                                        // Debug.Log(wall.wallAxis);
                                    }
                                    // Debug.Log("Completed wall search of Node " + n.x + ", " + n.z + ", " + n.y);
                                    // foreach (int w in n.walls)
                                    // {
                                    //     Debug.Log(n.x + ", " + n.z + ", " + n.y + " " + w);
                                    // }
                                    // DumpToConsole(n.walls);


                                }

                            }
                        }

                        if (n.isWalkable)
                        {
                        // nodeViz.Add(n.worldPosition);
                        }
                        grid[x,y,z] = n;
                    }
                }
            }
        }

    public Node GetNode(Vector3 wp)
    {
        Vector3 p = wp - minPos;
        int x = Mathf.RoundToInt(p.x / xzScale);
        int y = Mathf.RoundToInt(p.y / yScale);
        int z = Mathf.RoundToInt(p.z / xzScale);

        return GetNode(x,y,z);
    }

    public Node GetNode(int x, int y, int z)
    {
        if (x < 0 || x > pos_x-1 || y < 0 || y > pos_y - 1 || z < 0 || z > pos_z - 1)
        {
            return null;
        }
        // Debug.Log(x + " " + y + " " + z);
        return grid[x,y,z];
    }

    public void SetNodeCharacter(int x, int y, int z, GridCharacter c)
    {
        if (x < 0 || x > pos_x-1 || y < 0 || y > pos_y - 1 || z < 0 || z > pos_z - 1)
        {
            return;
        }
        if (c != null) {
        grid[x,y,z].character = c;
        c.currentNode = grid[x,y,z];
        }
        else 
        {
            grid[x,y,z].character = null;
        }
    }

    private void OnDrawGizmos() 
    {
        if (visualiseCollisions)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < nodeViz.Count; i++) 
            {
                Gizmos.DrawWireCube(nodeViz[i], extends);
            }
        }
    }

    public static void DumpToConsole(List<int> l)
        {
            var output = JsonUtility.ToJson(l, true);
            Debug.Log(output);
        }
    }
}
