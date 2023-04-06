using Adefagia.GridSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager gridManager = (GridManager)target;
        
        if (GUILayout.Button("Generate")) //8
        {
            ClearGrid(gridManager);
            
            OnButtonClick(gridManager);
        }
    }

    private void OnButtonClick(GridManager gridManager)
    {
        // Init gridElements
        gridManager.CreateGridElements();
            
        // Generate Grids
        gridManager.GenerateGrids();
    }

    private void ClearGrid(GridManager gridManager)
    {
        for (int i = gridManager.transform.childCount-1; i >= 0; i--)
        {
            // destroy
            DestroyImmediate(gridManager.transform.GetChild(i).gameObject);
        }
    }
}
