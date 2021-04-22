using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using CodeMonkey.Utils;

public class Grid<TGridObject> {

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int z;
    }

    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;
    private int width;
    private int height;
    private int cellSize;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    // private Vector3 originposition;


    public Grid(int width, int height, int cellSize, int mapSize, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
        this.width = width;
        this.height = height;
        // this.originPosition = originPosition;        
        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++){
            for (int z=0; z<gridArray.GetLength(1); z++){
                gridArray[x,z] = createGridObject(this, x, z);
            }
        }

        bool showDebug = true;
        if (showDebug) {


            debugTextArray = new TextMesh[width, height];
            // Debug.Log(width + " " + height);
            for (int x=0; x < gridArray.GetLength(0); x++){
                for (int z=0; z < gridArray.GetLength(1); z++) {
                    
                    debugTextArray[x, z] = CreateWorldText(gridArray[x, z]?.ToString(), null ,GetWorldPosition(x, z, mapSize, cellSize) + (new Vector3(cellSize,0,cellSize) /2), 8, Color.white, TextAnchor.MiddleCenter);
                    // Debug.Log(x + " " + z + "World Position: " + GetWorldPosition(x,z, mapSize));
                    Debug.DrawLine(GetWorldPosition(x, z, mapSize, cellSize),GetWorldPosition(x, z+1, mapSize, cellSize),Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z, mapSize, cellSize),GetWorldPosition(x+1, z, mapSize, cellSize),Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height, mapSize, cellSize),GetWorldPosition(width, height, mapSize, cellSize),Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0, mapSize, cellSize),GetWorldPosition(width, height, mapSize, cellSize),Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z].ToString();
            };
        }
        // SetValue(2,1,56);
        
    }

        public int GetWidth(){
        return width;
        }

        public int GetHeight(){
        return height;
        }



    public Vector3 GetWorldPosition(int x, int z, int mapSize, int cellSize){
        int ModifytoOrigin = mapSize / 10;
        // Debug.Log(ModifytoOrigin);
        return new Vector3(x-ModifytoOrigin,0,z-ModifytoOrigin) * cellSize;
    }

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), 
    int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, 
    TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000) 
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.rotation = Quaternion.AngleAxis(-90, Vector3.left);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public void SetGridObject(int x, int z, TGridObject value) 
    {
        if (x >= 0 && z >= 0 && x < width && z < height){
            gridArray[x,z] = value;
            debugTextArray[x,z].text = gridArray[x,z].ToString();

        }
    }

    public void GetXZ(Vector3 worldPosition, int cellSize, int mapSize, out int x, out int z)
    {
        //For some reason, cellSize has to be multiplied up until it is 10, need to deconstruct this maths sometime to work out what could break it. 
        x = Mathf.FloorToInt(worldPosition.x/2) + mapSize/(cellSize*5); 
        z = Mathf.FloorToInt(worldPosition.z/2) + mapSize/(cellSize*5);
        // Debug.Log("Get XZ called. X = " + x + " Z = " + z + "cellSize = " + cellSize);           
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value, int cellSize, int mapSize)
    {
        int x, z;
        GetXZ(worldPosition, cellSize, mapSize , out x, out z);
        SetGridObject(x,z,value);
        // Debug.Log("SetValueCalled. X: " + x + " Z: " + z + "Value passed:" + value);
    }

    public void TriggerGridObjectChanged(int x, int z){
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {x = x, z = z});
    }

    public TGridObject GetGridObject(int x, int z) {
        if (x >=0 && z >= 0 && x < width && z < height) {
            return gridArray[x,z];
        } else {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition, int cellSize, int mapSize) {
        int x, z;
        GetXZ(worldPosition, cellSize, mapSize, out x, out z);
        return GetGridObject(x,z);
    }

}

