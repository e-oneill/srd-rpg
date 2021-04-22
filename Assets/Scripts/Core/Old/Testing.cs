using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    [SerializeField] int x = 0;
    [SerializeField] int z = 0;
    [SerializeField] int cellSize = 5;
    [SerializeField] int mapSize = 100;
    // [SerializeField] private HeatMapVisual heatMapVisual;
    // [SerializeField] private HeatMapBoolVisual heatMapBoolVisual;
    // [SerializeField] private HeatMapGenericVisual heatMapGenericVisual;


    Vector3 position;

    private Grid<HeatMapGridObject> grid;
    // Start is called before the first frame update
    private void Start()
    {
       grid = new Grid<HeatMapGridObject>(x, z, cellSize, mapSize, (Grid<HeatMapGridObject> g, int x, int z) => new HeatMapGridObject(g,x,z)); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousehit;
            GetMousePosition(out mousehit);
            // grid.SetValue(mousehit, true, cellSize, mapSize);
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(mousehit, cellSize, mapSize);
            if (heatMapGridObject != null) {
                heatMapGridObject.AddValue(5);
            }
            
        }
        if (Input.GetMouseButtonDown(1)) {
            Vector3 mousehit;
            GetMousePosition(out mousehit);
            Debug.Log(grid.GetGridObject(mousehit,cellSize,mapSize));
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

    private void Grid_OnGridValueChanged(object sender, System.EventArgs e){
        // UpdateHeatMapVisual();
    }


    public class HeatMapGridObject {
        private const int MIN = 0;
        private const int MAX = 100;
        public int value;
        public Grid<HeatMapGridObject> grid;
        private int x;
        private int z;

        public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int z){
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
                public void AddValue(int addValue) {
            value += addValue;
            value = Mathf.Clamp(value, MIN, MAX);
            grid.TriggerGridObjectChanged(x,z);
        }

        public float GetValueNormalized(){
            return (float)value / MAX;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

}
