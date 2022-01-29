using System.Collections;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public enum Type
    {
        // exterior (building)
        Invalid,            // by default
        ValidExterior,      // inside a building

        // interior (rooms)
        InvalidInterior,    // walls of a room
        ValidInterior,      // inside a room

        // decorations and equipment
        ItemOccupied,       // room items

        // construction
        BuildingRoom,
        PlacingItem
    }



    [SerializeField]
    int             _index;

    BoxCollider     _boxCollider;
    MeshRenderer    _meshRenderer;

    Vector2Int      _gridLocalPosition;
    Type            _type;


    public const int GRID_SIZE = 10;



    void Awake()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();

        _gridLocalPosition = GetGridLocalPosition();

        SetColourBasedOnType();
    }


    public Vector2Int GetGridLocalPosition()
    {
        int gridX = (int)Mathf.Round(transform.localPosition.x / GRID_SIZE);
        int gridY = (int)Mathf.Round(transform.localPosition.z / GRID_SIZE);
        return new Vector2Int(gridX, gridY);
    }


    public void SetColourBasedOnType()
    {
        Transform cube = transform.GetChild(0);
        Color colour;

        switch (_type)
        {
            case Type.Invalid:
                colour = Color.red * 0.5f;
                break;
            case Type.ValidExterior:
                colour = Color.green * 0.75f;
                break;
            case Type.InvalidInterior:
                colour = Color.red * 0.75f;
                break;
            case Type.ValidInterior:
                colour = Color.green;
                break;
            case Type.ItemOccupied:
                colour = Color.red;
                break;
            case Type.BuildingRoom:
                colour = Color.yellow * 0.75f;
                break;
            case Type.PlacingItem:
                colour = Color.yellow;
                break;

            default:
                colour = Color.white;
                break;
        }

        _meshRenderer.material.color = colour;
    }


    public void CheckIfInsideExteriorBuilding(BoxCollider[] boxColliders)
    {
        // if was already checked for validity, leave
        if (_type != Type.Invalid) { return; }

        // check each collider wheter is contained.
        foreach (var boxCollider in boxColliders)
        {
            if (boxCollider.bounds.Intersects(_boxCollider.bounds))
            {
                _type = Type.ValidExterior;
                SetColourBasedOnType();
                break;
            }
        }
        
    }


    public void CellUnderConstruction()
    {
        StartCoroutine(UnderConstruction());
    }

    private IEnumerator UnderConstruction()
    {
        Type before = _type;

        _type = Type.BuildingRoom;
        SetColourBasedOnType();

        yield return new WaitUntil(() => Input.GetMouseButton(0) == false);

        _type = Type.ValidInterior;
        SetColourBasedOnType();
    }


    public Vector2Int GridPosition { get => _gridLocalPosition; }

    public int Index { get => _index; set => _index = value; }
}
