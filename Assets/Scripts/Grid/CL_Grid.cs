using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CL_Grid : MonoBehaviour
{
    public CL_GridDescriptor gridDescriptor;

    [Header("TEMPLATES")]
    public GameObject gridCellTemplate;
    
    [Header("CELLS ARRAYS")]
    [HideInInspector] public GameObject[,] cells;

    [Header("DRAW PROPERTIES")]
    [HideInInspector] public GameObject gridCellsParent;

    private void Start() {
        cells = new GameObject[(int)gridDescriptor.gridSize.x,(int)gridDescriptor.gridSize.y];

        gridCellsParent = new GameObject("Cells");
        gridCellsParent.transform.SetParent(this.transform);

        ResetGrid();

        gridCellsParent.transform.position = new Vector3(-gridDescriptor.gridSize.x/2, -gridDescriptor.gridSize.y/2, gridDescriptor.gridDepth);
    }

    private void InstantiateGridCells(int x, int y) {
        GameObject instance = Instantiate(gridCellTemplate, new Vector3(x, y, 0), transform.rotation, gridCellsParent.transform);
        CL_GridCell c = instance.GetComponent<CL_GridCell>();
        instance.name = string.Format("Cell {0} [{1},{2}]", c.id, x, y);
        c.x = x;
        c.y = y;

        cells[x,y] = instance;
    }

    public void ResetGrid() {
        ClearGrid();
        
        for (int x = 0; x < gridDescriptor.gridSize.x; x++) {
            for (int y = 0; y < gridDescriptor.gridSize.y; y++) {
                InstantiateGridCells(x,y);
            }
        }
    }

    private void ClearGrid() {
        // We first clear the 2-dimensional array 'cells'
        cells = new GameObject[(int)gridDescriptor.gridSize.x,(int)gridDescriptor.gridSize.y];

        // Then, we clear the instantiated objects in the scene
        if (gridCellsParent.transform.childCount > 0) {
            foreach (Transform cell in gridCellsParent.transform) {
                Destroy(cell.gameObject);
            }
        }
    }
}