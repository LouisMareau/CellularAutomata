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

        for (int x = 0; x < gridDescriptor.gridSize.x; x++) {
            for (int y = 0; y < gridDescriptor.gridSize.y; y++) {
                InstantiateGridCells(x,y);
            }
        }

        gridCellsParent.transform.position = new Vector3(-gridDescriptor.gridSize.x/2, -gridDescriptor.gridSize.y/2, 0);
    }

    private void InstantiateGridCells(int x, int y) {
        GameObject instance = Instantiate(gridCellTemplate, new Vector3(x, y, 0), transform.rotation, gridCellsParent.transform);
        CL_Cell c = instance.GetComponent<CL_Cell>();
        instance.name = string.Format("Cell [{0},{1}]", x, y);
        c.x = x;
        c.y = y;

        cells[x,y] = instance;
    }

    public void ResetGrid() {
        for (int x = 0; x < cells.GetLength(0); x++) {
            for (int y = 0; y < cells.GetLength(1); y++) {
                cells[x,y].GetComponent<CL_Cell>().cellState = CellState.DEAD;
            }
        }
    }

    public void ActivateCell(int x, int y) {
        cells[x,y].GetComponent<CL_Cell>().cellState = CellState.ALIVE;
    }

    public void DeactivateCell(int x, int y) {
        cells[x,y].GetComponent<CL_Cell>().cellState = CellState.DEAD;
    }
}