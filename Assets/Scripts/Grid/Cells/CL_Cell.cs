using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState {
    ALIVE,
    DEAD
}

[System.Serializable]
public class CL_Cell : MonoBehaviour
{
    public enum CellHoverState {
        IDLE,
        HOVER
    }

    [Header("POSITION COORDINATES")]
    public float x;
    public float y;

    [Header("STATES")]
    public CellState cellState;
    public CellHoverState cellHoverState;

    [Header("SPRITE COLORS")]
    public Color spriteColorDead;
    public Color spriteColorDeadHover;
    public Color spriteColorAlive;
    public Color spriteColorAliveHover;

    [Header("NEIGHBOURS")]
    public List<CL_Cell> liveNeighbours;

    [Header("REFERENCES")]
    [HideInInspector] public CL_Grid grid;


    private void Start() {
        // Find the 'Grid' reference in the hierarchy
        grid = GameObject.Find("Grid").GetComponent<CL_Grid>();

        // States initialization
        cellState = CellState.DEAD;
        cellHoverState = CellHoverState.IDLE;

        // Grid sprite color initialization
        this.gameObject.GetComponent<SpriteRenderer>().color = spriteColorDead;

        // Neighbours list initialization
        liveNeighbours = new List<CL_Cell>();
    }

    private void FixedUpdate() {
        if (liveNeighbours.Count > 0) {
            if (cellState == CellState.ALIVE) {
                // Instruction #1
                if (liveNeighbours.Count < 2) {
                    cellState = CellState.DEAD;
                }
                // Instruction #2
                else if (liveNeighbours.Count == 2 || liveNeighbours.Count == 3) {
                    cellState = CellState.ALIVE;
                }
                else if (liveNeighbours.Count > 3) {
                    cellState = CellState.DEAD;
                }
            }

            if (cellState == CellState.DEAD) {
                if (liveNeighbours.Count == 3) {
                    cellState = CellState.ALIVE;
                }
            }
        }
        else {
            cellState = CellState.DEAD;
        }
    }

    private void Update() {
        UpdateSprite();
        CheckNextGenerationNeighbours(); 
    }

    private void UpdateSprite() {
        if (cellState == CellState.ALIVE) {
            if (cellHoverState == CellHoverState.HOVER) { GetComponent<SpriteRenderer>().color = spriteColorAliveHover; }
            else { GetComponent<SpriteRenderer>().color = spriteColorAlive; }
        } 
        else {
            if (cellHoverState == CellHoverState.HOVER) { GetComponent<SpriteRenderer>().color = spriteColorDeadHover; }
            else { GetComponent<SpriteRenderer>().color = spriteColorDead; }
        }
    }

    #region MOUSE EVENTS
    private void OnMouseEnter() {
        cellHoverState = CellHoverState.HOVER;
        if (cellState == CellState.ALIVE) { this.gameObject.GetComponent<SpriteRenderer>().color = spriteColorAliveHover; }
        else { this.gameObject.GetComponent<SpriteRenderer>().color = spriteColorDeadHover; }
        
        // If mouse slide over the cell while being held down, we still update the state of the cell
        if (Input.GetMouseButton(0)) {
            if (cellState == CellState.ALIVE) {
                cellState = CellState.DEAD;
            } else {
                cellState = CellState.ALIVE;
            }
        }
    }

    private void OnMouseExit() {
        cellHoverState = CellHoverState.IDLE;
        if (cellState == CellState.ALIVE) { this.gameObject.GetComponent<SpriteRenderer>().color = spriteColorAlive; }
        else { this.gameObject.GetComponent<SpriteRenderer>().color = spriteColorDead; }
    }

    private void OnMouseDown() {
        if (cellState == CellState.ALIVE) {
            cellState = CellState.DEAD;
        } else {
            cellState = CellState.ALIVE;
        }
    }
    #endregion

    /// <summary>
    /// Checks for the cells neighbouring the current cell.
    /// </summary>
    /// <returns>Returns the amount of living cells found.</returns>
    public void CheckNextGenerationNeighbours() {
        int x = (int)this.x;
        int y = (int)this.y;
        List<CL_Cell> neighbourCells = new List<CL_Cell>();

        #region CORNERS/BORDERS (Exceptions)
        #region CORNERS
        // BOTTOM-LEFT CORNER
        if (x == 0 && y == 0) {
            if (grid.cells[x,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y+1].GetComponent<CL_Cell>());
            }
        }
        // TOP-LEFT CORNER
        else if (x == 0 && y == grid.cells.GetLength(1)-1) {
            if (grid.cells[x+1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_Cell>());
            }
            if (grid.cells[x,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y-1].GetComponent<CL_Cell>());
            }
        }
        // TOP-RIGHT CORNER
        else if (x == grid.cells.GetLength(0)-1 && y == grid.cells.GetLength(1)-1) {
            if (grid.cells[x-1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_Cell>());
            }
            if (grid.cells[x,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x-1,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y-1].GetComponent<CL_Cell>());
            }
        }
        // BOTTOM-RIGHT CORNER
        else if (x == grid.cells.GetLength(0)-1 && y == 0) {
            if (grid.cells[x-1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_Cell>());
            }
            if (grid.cells[x,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x-1,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y+1].GetComponent<CL_Cell>());
            }
        }
        #endregion
        #region BORDERS
        // LEFT BORDER
        else if (x == 0 && (y > 0 && y < grid.cells.GetLength(1)-1)) {
            if (grid.cells[x,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y+1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_Cell>());
            }
        }
        // RIGHT BORDER
        else if (x == grid.cells.GetLength(0)-1 && (y > 0 && y < grid.cells.GetLength(1)-1)) {
            if (grid.cells[x,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x-1,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x-1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_Cell>());
            }
            if (grid.cells[x-1,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y+1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_Cell>());
            }
        }
        // TOP BORDER
        else if ((x > 0 && x < grid.cells.GetLength(0)-1) && y == grid.cells.GetLength(1)-1) {
            if (grid.cells[x-1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_Cell>());
            }
            if (grid.cells[x-1,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y-1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_Cell>());
            }
        }
        // BOTTOM BORDER
        else if ((x > 0 && x < grid.cells.GetLength(0)-1) && y == 0) {
            if (grid.cells[x-1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_Cell>());
            }
            if (grid.cells[x-1,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y+1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y+1].GetComponent<CL_Cell>());
            }
            if (grid.cells[x+1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_Cell>());
            }
        }
        #endregion
        #endregion
        #region OTHER NEIGHBOURS (Inner Grid)
        // OTHER NEIGHBOURS (inside grid, corners and borders excluded) 
        else if ((x > 0 && x < grid.cells.GetLength(0)-1) && (y > 0 && y < grid.cells.GetLength(1)-1)) {
            // Bottom-left neighbour
            if (grid.cells[x-1,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y-1].GetComponent<CL_Cell>());
            }
            // Bottom neighbour
            if (grid.cells[x,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y-1].GetComponent<CL_Cell>());
            }
            // Bottom_right neighbour
            if (grid.cells[x+1,y-1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y-1].GetComponent<CL_Cell>());
            }
            // Left neighbour
            if (grid.cells[x-1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y].GetComponent<CL_Cell>());
            }
            // Right neighbour
            if (grid.cells[x+1,y].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y].GetComponent<CL_Cell>());
            }
            // Top-left neighbour
            if (grid.cells[x-1,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x-1,y+1].GetComponent<CL_Cell>());
            }
            // Top neighbour
            if (grid.cells[x,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x,y+1].GetComponent<CL_Cell>());
            }
            // Top-right neighbour
            if (grid.cells[x+1,y+1].GetComponent<CL_Cell>().cellState == CellState.ALIVE) {
                neighbourCells.Add(grid.cells[x+1,y+1].GetComponent<CL_Cell>());
            }
        }
        #endregion

        liveNeighbours = neighbourCells;
    }
}