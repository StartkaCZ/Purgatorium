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
    Type            _previousType;


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


    public void CheckExteriorBuilding(ExteriorBuilding exteriorBuilding)
    {
        // if was already checked for validity, leave
        if (_type != Type.Invalid) { return; }


        CheckAgainstWaitingRoom(exteriorBuilding);
    }

    private void CheckAgainstWaitingRoom(ExteriorBuilding exteriorBuilding)
    {
        if (exteriorBuilding.WaitingRoom.bounds.Intersects(_boxCollider.bounds))
        {
            _type = Type.ItemOccupied;
            _previousType = _type;
            SetColourBasedOnType();
        }
        else
        {
            CheckAgainstExteriorBuilding(exteriorBuilding);
        }
    }

    private void CheckAgainstExteriorBuilding(ExteriorBuilding exteriorBuilding)
    {
        // check each collider wheter is contained.
        foreach (var boxCollider in exteriorBuilding.BoxColliders)
        {
            if (_type == Type.Invalid && 
                boxCollider.bounds.Intersects(_boxCollider.bounds) && 
                !CheckAgainstEntrances(exteriorBuilding))
            {
                _type = Type.ValidExterior;
                _previousType = _type;
                SetColourBasedOnType();
                break;
            }
        }
    }

    private bool CheckAgainstEntrances(ExteriorBuilding exteriorBuilding)
    {
        bool isEntrace = false;

        foreach (var entrance in exteriorBuilding.Entraces)
        {
            if (entrance.bounds.Intersects(_boxCollider.bounds))
            {
                _type = Type.ItemOccupied;
                _previousType = _type;
                SetColourBasedOnType();

                isEntrace = true;
                break;
            }
        }

        return isEntrace;
    }

    public void UnderConstruction()
    {
        if (_type != Type.BuildingRoom)
            _previousType = _type;

        CheckIfObstructed();
        SetColourBasedOnType();
    }

    private void CheckIfObstructed()
    {
        if (_type == Type.ValidExterior)
            _type = Type.BuildingRoom;
        else if (_type == Type.Invalid ||
                 _type == Type.InvalidInterior ||
                 _type == Type.ItemOccupied)
            _type = Type.InvalidInterior;
    }


    public void ReturnToNormal(bool roomDestroyed=false)
    {
        if (roomDestroyed)
        {
            _type = Type.ValidExterior;
            _previousType = _type;
        }
        else
            _type = _previousType;

        SetColourBasedOnType();
    }

    public void PartOfRoom()
    {
        _type = Type.ValidInterior;
        _previousType = _type;
        SetColourBasedOnType();
    }

    public void ContainsItem()
    {
        _type = Type.ItemOccupied;
        SetColourBasedOnType();
    }



    public Type OccupationType { get => _type; }

    public Vector2Int GridLocalPosition { get => _gridLocalPosition; }

    public bool CanBeBuiltUpon { get => _previousType == Type.ValidExterior; }

    public bool CanContainItem { get => _type == Type.ValidInterior; }

    public int Index { get => _index; set => _index = value; }
}
