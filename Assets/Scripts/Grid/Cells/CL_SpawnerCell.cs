using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CL_SpawnerCell : CL_Cell 
{
    /*[Header("ID")]
    public int id;
    public static int count;

    private void Awake() {
        id = count++;
    }

    [Header("LIVE STATE")]
    public CellState state;

    [Header("REFERENCES")]
    [HideInInspector] public CL_Grid grid;

    [Header("NEIGHBOURS")]
    public List<CL_SpawnerCell> liveNeighbours;

    private void Start() {
        grid = GameObject.Find("Grid").GetComponent<CL_Grid>();
        state = CellState.DEAD;

        liveNeighbours = new List<CL_SpawnerCell>();
    }

    /// <summary>
    /// Checks for the cells neighbouring the current cell.
    /// </summary>
    /// <returns>Returns the amount of living cells found.</returns>
    public void CheckNextGenerationNeighbours() {
        int x = (int)this.x;
        int y = (int)this.y;
        List<CL_SpawnerCell> neighbourCells = new List<CL_SpawnerCell>();

        #region CORNERS/BORDERS (Exceptions)
        #region CORNERS
        // BOTTOM-LEFT CORNER
        if (x == 0 && y == 0) {
            if (grid.cells[x,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y+1].GetComponent<CL_SpawnerCell>());
            }
        }
        // TOP-LEFT CORNER
        else if (x == 0 && y == grid.cells.GetLength(1)-1) {
            if (grid.cells[x+1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y-1].GetComponent<CL_SpawnerCell>());
            }
        }
        // TOP-RIGHT CORNER
        else if (x == grid.cells.GetLength(0)-1 && y == grid.cells.GetLength(1)-1) {
            if (grid.cells[x-1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x-1,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y-1].GetComponent<CL_SpawnerCell>());
            }
        }
        // BOTTOM-RIGHT CORNER
        else if (x == grid.cells.GetLength(0)-1 && y == 0) {
            if (grid.cells[x-1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x-1,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y+1].GetComponent<CL_SpawnerCell>());
            }
        }
        #endregion
        #region BORDERS
        // LEFT BORDER
        else if (x == 0 && (y > 0 && y < grid.cells.GetLength(1)-1)) {
            if (grid.cells[x,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y+1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_SpawnerCell>());
            }
        }
        // RIGHT BORDER
        else if (x == grid.cells.GetLength(0)-1 && (y > 0 && y < grid.cells.GetLength(1)-1)) {
            if (grid.cells[x,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x-1,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x-1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x-1,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y+1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_SpawnerCell>());
            }
        }
        // TOP BORDER
        else if ((x > 0 && x < grid.cells.GetLength(0)-1) && y == grid.cells.GetLength(1)-1) {
            if (grid.cells[x-1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x-1,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y-1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_SpawnerCell>());
            }
        }
        // BOTTOM BORDER
        else if ((x > 0 && x < grid.cells.GetLength(0)-1) && y == 0) {
            if (grid.cells[x-1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x-1,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y+1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y+1].GetComponent<CL_SpawnerCell>());
            }
            if (grid.cells[x+1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_SpawnerCell>());
            }
        }
        #endregion
        #endregion
        #region OTHER NEIGHBOURS (Inner Grid)
        // OTHER NEIGHBOURS (inside grid, corners and borders excluded) 
        else if ((x > 0 && x < grid.cells.GetLength(0)-1) && (y > 0 && y < grid.cells.GetLength(1)-1)) {
            // Bottom-left neighbour
            if (grid.cells[x-1,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y-1].GetComponent<CL_SpawnerCell>());
            }
            // Bottom neighbour
            if (grid.cells[x,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_SpawnerCell>());
            }
            // Bottom_right neighbour
            if (grid.cells[x+1,y-1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y-1].GetComponent<CL_SpawnerCell>());
            }
            // Left neighbour
            if (grid.cells[x-1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_SpawnerCell>());
            }
            // Right neighbour
            if (grid.cells[x+1,y].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_SpawnerCell>());
            }
            // Top-left neighbour
            if (grid.cells[x-1,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y+1].GetComponent<CL_SpawnerCell>());
            }
            // Top neighbour
            if (grid.cells[x,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_SpawnerCell>());
            }
            // Top-right neighbour
            if (grid.cells[x+1,y+1].GetComponent<CL_SpawnerCell>().state == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y+1].GetComponent<CL_SpawnerCell>());
            }
        }
        #endregion

        liveNeighbours = neighbourCells;
    }

    private void FixedUpdate() {
        if (state == CellState.ALIVE) {
            // Instruction #1
            if (liveNeighbours.Count < 2) {
                state = CellState.DEAD;
            }
            // Instruction #2
            else if (liveNeighbours.Count == 2 || liveNeighbours.Count == 3) {
                state = CellState.ALIVE;
            }
            else if (liveNeighbours.Count > 3) {
                state = CellState.DEAD;
            }
        }
        else if (state == CellState.DEAD) {
            if (liveNeighbours.Count == 3) {
                state = CellState.ALIVE;
            } 
            else {
                state = CellState.DEAD;
            }
        }

        CheckNextGenerationNeighbours();
    }

    private void Update() {
        if (CL_GameManager.gameState == GameState.PAUSE) { CheckNextGenerationNeighbours(); }

        UpdateSprite();
    }

    private void UpdateSprite() {
        if (state == CellState.ALIVE) {
            GetComponent<SpriteRenderer>().enabled = true;
        } 
        else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }*/
}