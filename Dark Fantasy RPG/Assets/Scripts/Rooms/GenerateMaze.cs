using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using enums;
public class GenerateMaze : MonoBehaviour
{
    int max_depth, steps_back, max_rooms;

    public class Cell
    {
        public Cell parent;
        public Vector2Int pos;
        public int depth = 0;
        public bool stepped = false;
    }

    public bool[,] Generate(Vector2Int size)
    {
        bool[,] maze = new bool[size.x, size.y];
        List<Cell> mazeCells = GetMaze(new Cell[size.x, size.y], 5, 1, 15);

        for (int i = 0; i < mazeCells.Count; i++)
        {
            maze[mazeCells[i].pos.x, mazeCells[i].pos.y] = true;
        }

        return maze;
    }

    List<Cell> GetMaze(Cell[,] grid, int depth, int steps, int rooms)
    {
        max_depth = depth;
        steps_back = steps;
        max_rooms = rooms;
        Vector2Int start_pos = new Vector2Int(Mathf.RoundToInt(grid.GetLength(0) / 2), Mathf.RoundToInt(grid.GetLength(1) / 2));
        List<Cell> cells = new List<Cell>();
        Cell first_cell = new Cell{pos = new Vector2Int(start_pos.x, start_pos.y)};
        grid[first_cell.pos.x, first_cell.pos.y] = first_cell;
        cells.Add(first_cell);
        return Step(cells, grid, first_cell);
    }

    List<Cell> Step(List<Cell> cells, Cell[,] grid, Cell current)
    {
        if (cells.Count < max_rooms)
        {
            // If any cells are not filled, continue else return list of cells
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == null)
                    {
                        goto step;
                    }
                }
            }
        }
        cells = cells.OrderBy(w => w.depth).ToList();
        return cells;

    step:

        DIR dir = RandomDir();
        if (current.depth <= max_depth)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2Int pos = GetNewPos(dir, current.pos);
                if (InGrid(pos, grid) && grid[pos.x, pos.y] == null)
                {
                    grid[pos.x, pos.y] = new Cell{pos = new Vector2Int(pos.x, pos.y), parent = current, depth = current.depth + 1 };
                    cells.Add(grid[pos.x, pos.y]);
                    return Step(cells, grid, grid[pos.x, pos.y]);
                }
                if (dir == DIR.W)
                {
                    dir = DIR.N;
                }
                else
                {
                    dir++;
                }
            }
        }
        if (current.parent == null)
        {
            cells = cells.OrderBy(w => w.depth).ToList();
            return cells;
        }
        else
        {
            Cell parent_cell = current;
            for (int i = 0; i < steps_back; i++)
            {
                if (parent_cell.parent != null)
                {
                    parent_cell = parent_cell.parent;
                }
            }
            return Step(cells, grid, parent_cell);
        }
    }

    public Vector2Int GetNewPos(DIR dir, Vector2Int pos)
    {
        if (dir == DIR.N) return new Vector2Int(pos.x, pos.y + 1);
        else if (dir == DIR.E) return new Vector2Int(pos.x + 1, pos.y);
        else if (dir == DIR.S) return new Vector2Int(pos.x, pos.y - 1);
        else if (dir == DIR.W) return new Vector2Int(pos.x - 1, pos.y);
        return pos;
    }

    bool InGrid(Vector2Int pos, Cell[,] grid)
    {
        if (pos.x < 0 || pos.x >= grid.GetLength(0) || pos.y < 0 || pos.y >= grid.GetLength(1)) return false;
        else return true;
    }

    DIR RandomDir()
    {
        //Random.InitState((int)Time.time);
        return (DIR)Random.Range(0, 4);
    }
}
