using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// namespace
using EnumHolder;

[ExecuteInEditMode]
public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    List<GridCell>  _grid;
    [SerializeField]
    Transform       _gridTransform;
    [SerializeField]
    GameObject      _gridCellPrefab;
    [SerializeField]
    [Range(1, 100)]
    int             _width;
    [SerializeField]
    [Range(1, 100)]
    int             _depth;
    


    public void GenerateGrid()
    {
        if (_gridTransform == null) { return; }

        _grid = new List<GridCell>(_width * _depth);
        Vector2Int gridPosition = Vector2Int.zero;

        // if a grid exists clear the existing grid
        while (_gridTransform.childCount > 0)
        {
            DestroyImmediate(_gridTransform.GetChild(0).gameObject);
        }

        // generate a new grid
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _depth; j++)
            {
                Transform gridTransform = Instantiate(_gridCellPrefab, _gridTransform).transform;
                gridTransform.localPosition = new Vector3(gridPosition.x * GridCell.GRID_SIZE, 
                                                          0, 
                                                          gridPosition.y * GridCell.GRID_SIZE);

                GridCell gridCell = gridTransform.GetComponent<GridCell>();
                gridCell.Index = i * _width + j;
                _grid.Add(gridCell);

                gridPosition.x++;
            }

            gridPosition.x = 0;
            gridPosition.y++;
        }
    }


    public Dictionary<RoomPart, List<GridCell>> SelectedGrid(GridCell start, GridCell end)
    {
        Dictionary<RoomPart, List<GridCell>> roomCells = new Dictionary<RoomPart, List<GridCell>>(3)
        {
            { RoomPart.Corner, new List<GridCell>(4) },
            { RoomPart.Edge, new List<GridCell>() },
            { RoomPart.Space, new List<GridCell>() }
        };

        Vector2Int startPosition = start.GridLocalPosition;
        Vector2Int endPosition = end.GridLocalPosition;
        Vector2Int endSearchPosition = endPosition;
        Vector2Int positionDifference = endPosition - startPosition;

        // if x difference is 0, we need to offset it by 1
        if (positionDifference.x != 0)
            positionDifference.x /= Mathf.Abs(positionDifference.x);
        else
            positionDifference.x = 1;
        // if y difference is 0, we need to offset it by 1
        if (positionDifference.y != 0)
            positionDifference.y /= Mathf.Abs(positionDifference.y);
        else
            positionDifference.y = 1;

        // extend the search by 1 on x and y so that we get the end as well
        endSearchPosition += positionDifference;

        for (int i = startPosition.x; i != endSearchPosition.x; )
        {
            for (int j = startPosition.y; j != endSearchPosition.y; )
            {
                int index = ConvertPositionToIndex(new Vector2Int(i, j));
                // check for corners
                if ((i == startPosition.x && j == startPosition.y) || 
                    (i == startPosition.x && j == endPosition.y) ||
                    (i == endPosition.x && j == startPosition.y) ||
                    (i == endPosition.x && j == endPosition.y))
                {
                    roomCells[RoomPart.Corner].Add(_grid[index]);
                }
                // check for edges
                else if (i == startPosition.x ||
                         i == endPosition.x ||
                         j == startPosition.y ||
                         j == endPosition.y)
                {
                    roomCells[RoomPart.Edge].Add(_grid[index]);
                }
                // check for space
                else
                    roomCells[RoomPart.Space].Add(_grid[index]);

                // mark the grid cell under construction
                _grid[index].UnderConstruction();


                if (positionDifference.y > 0)
                    j++;
                else
                    j--;
            }

            if (positionDifference.x > 0)
                i++;
            else
                i--;
        }


        return roomCells;
    }


    public int ConvertPositionToIndex(Vector2Int position)
    {
        // y = rows, x = colums
        int index = position.y * _width + position.x;

        return index;
    }

    public GridCell GetGridCellFromWorldPosition(Vector3 position)
    {
        // y = rows, x = colums
        Vector2Int location = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z)) / GridCell.GRID_SIZE;
        int index = location.y * _width + location.x;

        return _grid[index];
    }


    public List<GridCell> Grid { get => _grid; }



    #if UNITY_EDITOR
    [CustomEditor(typeof(WorldGenerator))]
    public class WorldGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WorldGenerator worldGenerator = (WorldGenerator)target;

            if (GUILayout.Button("GenerateGrid"))
            {
                worldGenerator.GenerateGrid();
            }
        }
    }
    #endif
}
