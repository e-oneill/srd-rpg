using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{

    [SerializeField] int x = 42;
    [SerializeField] int z = 42;
    [SerializeField] int cellSize = 5;
    [SerializeField] int mapSize = 250;
    // Start is called before the first frame update
    private Pathfinding pathfinding;
    public void Start(){
        pathfinding = new Pathfinding(x, z, cellSize, mapSize);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)){
            Vector3 mousehit;
            GetMousePosition(out mousehit);
            // Debug.Log(mousehit + " " + pathfinding);
            pathfinding.GetGrid().GetXZ(mousehit, cellSize, mapSize, out int x, out int z);
            List<PathNode> path = pathfinding.FindPath(0,0,x,z);
            if (path!=null) {
                for (int i=0; i<path.Count-1;i++){
                Debug.DrawLine(new Vector3(path[i].x, path[i].z) * 10f + Vector3.one * 5f, new Vector3(path[i].x, path[i].z) * 10f + Vector3.one * 5f, Color.green, 100f);
                // Debug.Log("Drawing Line");
                }
            }
        }
    }

    public void GetMousePosition(out Vector3 mousehit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        mousehit = hit.point;
        // if (hasHit)
        // {
        //     Vector3 worldPosition = hit.point;
        //     grid.SetValue(worldPosition, 56, cellSize, mapSize);
        //     Debug.Log("Raycast Successful. World Position: " + worldPosition);
        // }
    }
}
