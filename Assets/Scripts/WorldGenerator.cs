using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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


    public List<GridCell> SelectedGrid(GridCell start, GridCell end)
    {
        List<GridCell> cells = new List<GridCell>();

        Vector2Int startPosition = start.GetGridLocalPosition();
        Vector2Int endPosition = end.GetGridLocalPosition();

        Vector2Int positionDifference = endPosition - startPosition;

        for (int i = startPosition.x; i < positionDifference.x; i++)
        {
            for (int j = startPosition.y; j < positionDifference.y; j++)
            {
                
            }
        }

        print(startPosition);
        print(endPosition);
        print(positionDifference);

        print("-------------INDEX----------------------");

        return cells;
    }

    public int ConvertPositionToIndex(Vector2Int position)
    {
        // y = rows, x = colums
        int index = position.y * _width + position.x;

        print(index);
        return index;
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
