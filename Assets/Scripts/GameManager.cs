using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// namespace
using EnumHolder;

public class GameManager : MonoBehaviour
{
    enum BuildingState
    {
        Idle,
        RoomCreating,
        DoorAdding,
        CoreItemAdding,
        Confirmation
    }



    WorldGenerator      _worldGenerator;
    ExteriorBuilding    _exteriorBuilding;
    List<Room>          _rooms;

    BuildingState       _buildingState;



    void Awake()
    {
        Factory.Instance();
        AudioManager.Instance().PlayMusic(AudioManager.Music.Game);
    }



    void Start()
    {
        _worldGenerator = FindObjectOfType<WorldGenerator>();
        _exteriorBuilding = FindObjectOfType<ExteriorBuilding>();

        _rooms = new List<Room>();

        var gridCells = _worldGenerator.Grid;
        foreach (var cell in gridCells)
        {
            if (_exteriorBuilding.IsUnlocked)
            {
                cell.CheckExteriorBuilding(_exteriorBuilding);
            }
        }
    }




    void Update()
    {
        switch (_buildingState)
        {
            case BuildingState.Idle:
                UpdateIdleBuildingState();
                break;
            case BuildingState.RoomCreating:
                UpdateRoomCreatingState();
                break;
            case BuildingState.DoorAdding:
                UpdateDoorAddingState();
                break;
            case BuildingState.CoreItemAdding:
                UpdateCoreItemAddingState();
                break;
            case BuildingState.Confirmation:
                UpdateConfirmationState();
                break;

            default:
                break;
        }
    }

    private void UpdateIdleBuildingState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(StartBuildingRoom());
        }
    }


    private void UpdateRoomCreatingState()
    {
        // everything happens in a coroutine
    }

    private void UpdateDoorAddingState()
    {
        // everything happens in a coroutine
    }

    private void UpdateCoreItemAddingState()
    {
        // everything happens in a coroutine
    }

    private void UpdateConfirmationState()
    {

    }


    private IEnumerator StartBuildingRoom()
    {
        _buildingState = BuildingState.RoomCreating;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Dictionary<RoomPart, List<GridCell>> roomCells = null;
        GridCell startCell = null;
        GridCell endCell = null;

        // ray cast to cell
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            startCell = objectHit.GetComponentInParent<GridCell>();

            print(objectHit.parent.name);
        }

        // mark start cell under construction and build the room
        if (startCell != null)
        {
            startCell.UnderConstruction();

            while (Input.GetMouseButton(0))
            {
                yield return new WaitForEndOfFrame();

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;
                    endCell = objectHit.GetComponentInParent<GridCell>();
                }

                if (endCell != null)
                {
                    if (roomCells != null)
                    {
                        ReturnCellsToNormal(roomCells);
                    }

                    if (endCell != startCell)
                    {
                        roomCells = _worldGenerator.SelectedGrid(startCell, endCell);
                    }
                }
            }
        }

        // release or null
        if (RoomCellsValid(roomCells))
        {
            BuildRoom(roomCells);

            _buildingState = BuildingState.DoorAdding;

            StartCoroutine(AddDoor());
        }
        else
        {
            startCell.ReturnToNormal();
            endCell.ReturnToNormal();
            ReturnCellsToNormal(roomCells);

            _buildingState = BuildingState.Idle;
        }
    }


    private void ReturnCellsToNormal(Dictionary<RoomPart, List<GridCell>> roomCells)
    {
        foreach (var roomCell in roomCells)
        {
            foreach (var item in roomCell.Value)
            {
                item.ReturnToNormal();
            }
        }
    }


    private bool RoomCellsValid(Dictionary<RoomPart, List<GridCell>> roomCells)
    {
        if (roomCells == null || roomCells[RoomPart.Space].Count < 6)
            return false;

        foreach (var roomCell in roomCells)
        {
            foreach (var item in roomCell.Value)
            {
                if (item.CanBeBuiltUpon == false)
                {
                    return false;
                }
            }
        }

        return true;
    }


    private void BuildRoom(Dictionary<RoomPart, List<GridCell>> roomCells)
    {
        GridCell start = roomCells[RoomPart.Corner][0];
        GridCell end = roomCells[RoomPart.Corner][3];

        Room room = Factory.Instance().GenerateRoom();
        room.transform.parent = _worldGenerator.transform;
        room.Setup(start, end, roomCells);

        _rooms.Add(room);
    }


    private IEnumerator AddDoor()
    {
        Room latestRoom = _rooms[_rooms.Count - 1];
        Door door = Factory.Instance().GenerateDoor(latestRoom.transform);

        while (_buildingState == BuildingState.DoorAdding)
        {
            yield return new WaitForEndOfFrame();

            if (Input.GetMouseButton(1))
            {
                Destroy(door.gameObject);
                Destroy(latestRoom.gameObject);

                _buildingState = BuildingState.Idle;
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // ray cast to cell
                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;
                    door.FixToParent(objectHit.parent);

                    // if we managed to fix the door to a wall
                    // and the press confirms by clicking the mouse
                    // and the door is a valid placement
                    if (latestRoom.TryToFixDoorToWall(door) && 
                        Input.GetMouseButton(0))
                    {
                        if (ValidDoorPlacement(door))
                        {
                            _buildingState = BuildingState.CoreItemAdding;

                            StartCoroutine(AddCoreItem());
                        }
                    }
                }
            }
        } // end of while loop

    }

    private bool ValidDoorPlacement(Door door)
    {
        GridCell entryGridCell = _worldGenerator.GetGridCellFromWorldPosition(door.EntryPosition);
        GridCell exitGridCell = _worldGenerator.GetGridCellFromWorldPosition(door.ExitPosition);

        if (entryGridCell.OccupationType == GridCell.Type.ValidExterior && 
            exitGridCell.CanContainItem)
        {
            Room latestRoom = _rooms[_rooms.Count - 1];
            latestRoom.AddDoorPassage(entryGridCell, exitGridCell);

            return true;
        }

        return false;
    }


    private IEnumerator AddCoreItem()
    {
        Room latestRoom = _rooms[_rooms.Count - 1];
        CoreItem coreItem = Factory.Instance().GenerateCoreItem(latestRoom.transform);

        while (_buildingState == BuildingState.CoreItemAdding)
        {
            yield return new WaitForEndOfFrame();

            if (Input.GetMouseButton(1))
            {
                Destroy(coreItem.gameObject);
                Destroy(latestRoom.gameObject);

                _buildingState = BuildingState.Idle;
            }
            else if (Input.GetKey(KeyCode.R))
            {
                coreItem.Rotate90();
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // ray cast to cell
                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;
                    coreItem.FixToParent(objectHit.parent);

                    if (latestRoom.TryToFixItemToSpace(coreItem) && 
                        Input.GetMouseButton(0) &&
                        ValidCoreItemPlacement(coreItem))
                    {
                        _buildingState = BuildingState.Idle;

                        print(objectHit.parent.name);
                    }
                }
            }
        }
    }

    private bool ValidCoreItemPlacement(CoreItem coreItem)
    {
        List<GridCell> gridCells = new List<GridCell>();

        foreach (var item in coreItem.FloorPanels)
        {
            GridCell gridCell = _worldGenerator.GetGridCellFromWorldPosition(item.position);

            if (gridCell.CanContainItem)
                gridCells.Add(gridCell);
            else
                return false;
        }

        Room latestRoom = _rooms[_rooms.Count - 1];
        latestRoom.AddCoreItem(gridCells);

        return true;
    }
}
