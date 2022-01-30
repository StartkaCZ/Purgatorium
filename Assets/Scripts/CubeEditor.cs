using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(GridCell))]
public class CubeEditor : MonoBehaviour
{
    //[SerializeField][Range(0.0f, 20.0f)] 
    //const float     GRID_SIZE = 10.0f;

    GridCell        _gridCell;
    TextMesh        _text;



    void Start()
    {
        _text = GetComponentInChildren<TextMesh>();
        _gridCell = GetComponent<GridCell>();
    }



    void Update()
    {
        Vector2 gridPosition = _gridCell.GetGridLocalPosition();
        SnapToGrid(gridPosition);
        UpdateLabel(gridPosition);
    }

    private void SnapToGrid(Vector2 gridPosition)
    {
        float gridSize = GridCell.GRID_SIZE;

        float snappedX = gridPosition.x * gridSize;
        float snappedZ = gridPosition.y * gridSize;

        transform.localPosition = new Vector3(snappedX, 0.0f, snappedZ);
    }

    private void UpdateLabel(Vector2 gridPosition)
    {
        string text = gridPosition.x.ToString() + "," + 
                      gridPosition.y.ToString();
        
        _text.text = text;
        gameObject.name = text;
    }
}
