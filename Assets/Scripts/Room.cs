using System.Collections.Generic;
using UnityEngine;
// namespace
using EnumHolder;

public class Room : MonoBehaviour
{
    List<GridCell>  _gridCells;
    List<GridCell>  _coreItemCells;
    List<Transform> _corners;
    List<Transform> _edges;
    List<Transform> _spaces;

    GridCell        _entrance;
    GridCell        _exit;

    BoxCollider     _boxCollder;

    RoomType        _roomType;



    void Awake()
    {
        _gridCells = new List<GridCell>();
        _coreItemCells = new List<GridCell>();
        _corners = new List<Transform>(4);
        _edges = new List<Transform>();
        _spaces = new List<Transform>();

        _boxCollder = GetComponent<BoxCollider>();
    }



    public void CalculatePositionAndSize(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 positionDifference = endPosition - startPosition;

        _boxCollder.center = new Vector3(0.0f, -5.0f, 0.0f);
        _boxCollder.size = new Vector3(positionDifference.x + GridCell.GRID_SIZE, 
                                       10.0f, 
                                       positionDifference.z + GridCell.GRID_SIZE);


        transform.position = startPosition + positionDifference * 0.5f;
    }

    
    public void Setup(GridCell start, GridCell end, Dictionary<RoomPart, List<GridCell>> roomCells)
    {
        CalculatePositionAndSize(start.transform.position, end.transform.position);

        Vector2Int startPosition = start.GridLocalPosition;
        Vector2Int endPosition = end.GridLocalPosition;
        Vector2Int positionDifference = endPosition - startPosition;



        foreach (var item in roomCells[RoomPart.Corner])
        {
            GameObject roomCorner = Factory.Instance().GenerateRoomPart(RoomPart.Corner, transform);
            roomCorner.transform.position = item.transform.position;

            _corners.Add(roomCorner.transform);
            _gridCells.Add(item);

            item.PartOfRoom();
        }

        foreach (var item in roomCells[RoomPart.Space])
        {
            GameObject roomSpace = Factory.Instance().GenerateRoomPart(RoomPart.Space, transform);
            roomSpace.transform.position = item.transform.position;

            _spaces.Add(roomSpace.transform);
            _gridCells.Add(item);

            item.PartOfRoom();
        }


        // start going top right
        if (positionDifference.x > 0 &&
            positionDifference.y > 0)
        {
            RoomStartingBottomLeft(roomCells, startPosition, endPosition);
        }
        // start going bottom right
        else if (positionDifference.x > 0 &&
            positionDifference.y < 0)
        {
            RoomStartingTopLeft(roomCells, startPosition, endPosition);
        }
        // start going top left
        else if (positionDifference.x < 0 &&
            positionDifference.y > 0)
        {
            RoomStartingBottomRight(roomCells, startPosition, endPosition);
        }
        // start going bottom left
        else
        {
            RoomStartingTopRight(roomCells, startPosition, endPosition);
        }
    }

    #region Set up Room Based on Direction
    private void RoomStartingBottomLeft(Dictionary<RoomPart, List<GridCell>> roomCells, Vector2Int startPosition, Vector2Int endPosition)
    {
        _corners[0].transform.eulerAngles = new Vector3(0, 90, 0);
        _corners[1].transform.eulerAngles = new Vector3(0, 180, 0);
        _corners[2].transform.eulerAngles = new Vector3(0, 0, 0);
        _corners[3].transform.eulerAngles = new Vector3(0, -90, 0);

        foreach (var item in roomCells[RoomPart.Edge])
        {
            GameObject roomEdge = Factory.Instance().GenerateRoomPart(RoomPart.Edge, transform);
            roomEdge.transform.position = item.transform.position;

            if (item.GridLocalPosition.x == startPosition.x)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (item.GridLocalPosition.y == startPosition.y)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (item.GridLocalPosition.x == endPosition.x)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (item.GridLocalPosition.y == endPosition.y)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, -90, 0);
            }

            _edges.Add(roomEdge.transform);
            _gridCells.Add(item);

            item.PartOfRoom();
        }
    }

    private void RoomStartingTopLeft(Dictionary<RoomPart, List<GridCell>> roomCells, Vector2Int startPosition, Vector2Int endPosition)
    {
        _corners[0].transform.eulerAngles = new Vector3(0, 180, 0);
        _corners[1].transform.eulerAngles = new Vector3(0, 90, 0);
        _corners[2].transform.eulerAngles = new Vector3(0, -90, 0);
        _corners[3].transform.eulerAngles = new Vector3(0, 0, 0);

        foreach (var item in roomCells[RoomPart.Edge])
        {
            GameObject roomEdge = Factory.Instance().GenerateRoomPart(RoomPart.Edge, transform);
            roomEdge.transform.position = item.transform.position;

            if (item.GridLocalPosition.x == startPosition.x)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (item.GridLocalPosition.y == startPosition.y)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, -90, 0);
            }
            else if (item.GridLocalPosition.x == endPosition.x)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (item.GridLocalPosition.y == endPosition.y)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 90, 0);
            }

            _edges.Add(roomEdge.transform);
            _gridCells.Add(item);

            item.PartOfRoom();
        }
    }

    private void RoomStartingBottomRight(Dictionary<RoomPart, List<GridCell>> roomCells, Vector2Int startPosition, Vector2Int endPosition)
    {
        _corners[0].transform.eulerAngles = new Vector3(0, 0, 0);
        _corners[1].transform.eulerAngles = new Vector3(0, -90, 0);
        _corners[2].transform.eulerAngles = new Vector3(0, 90, 0);
        _corners[3].transform.eulerAngles = new Vector3(0, 180, 0);

        foreach (var item in roomCells[RoomPart.Edge])
        {
            GameObject roomEdge = Factory.Instance().GenerateRoomPart(RoomPart.Edge, transform);
            roomEdge.transform.position = item.transform.position;

            if (item.GridLocalPosition.x == startPosition.x)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (item.GridLocalPosition.y == startPosition.y)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (item.GridLocalPosition.x == endPosition.x)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (item.GridLocalPosition.y == endPosition.y)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, -90, 0);
            }

            _edges.Add(roomEdge.transform);
            _gridCells.Add(item);

            item.PartOfRoom();
        }
    }

    private void RoomStartingTopRight(Dictionary<RoomPart, List<GridCell>> roomCells, Vector2Int startPosition, Vector2Int endPosition)
    {
        _corners[0].transform.eulerAngles = new Vector3(0, -90, 0);
        _corners[1].transform.eulerAngles = new Vector3(0, 0, 0);
        _corners[2].transform.eulerAngles = new Vector3(0, 180, 0);
        _corners[3].transform.eulerAngles = new Vector3(0, 90, 0);

        foreach (var item in roomCells[RoomPart.Edge])
        {
            GameObject roomEdge = Factory.Instance().GenerateRoomPart(RoomPart.Edge, transform);
            roomEdge.transform.position = item.transform.position;

            if (item.GridLocalPosition.x == startPosition.x)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (item.GridLocalPosition.y == startPosition.y)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, -90, 0);
            }
            else if (item.GridLocalPosition.x == endPosition.x)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (item.GridLocalPosition.y == endPosition.y)
            {
                roomEdge.transform.eulerAngles = new Vector3(0, 90, 0);
            }

            _edges.Add(roomEdge.transform);
            _gridCells.Add(item);

            item.PartOfRoom();
        }
    }
    #endregion



    public bool TryToFixDoorToWall(Door door)
    {
        foreach (var item in _edges)
        {
            if (Vector3.Distance(item.position, door.transform.position) < GridCell.GRID_SIZE)
            {
                door.FixToParent(item);

                return true;
            }
        }

        return false;
    }


    public bool TryToFixItemToSpace(CoreItem coreItem)
    {
        foreach (var item in _spaces)
        {
            if (Vector3.Distance(item.position, coreItem.transform.position) < GridCell.GRID_SIZE)
            {
                coreItem.FixToParent(item);

                return true;
            }
        }

        return false;
    }


    public void AddDoorPassage(GridCell entrance, GridCell exit)
    {
        _entrance = entrance;
        _exit = exit;

        _entrance.ContainsItem();
        _exit.ContainsItem();
    }


    public void AddCoreItem(List<GridCell> gridCells)
    {
        _coreItemCells = gridCells;

        foreach (var item in _coreItemCells)
        {
            item.ContainsItem();
        }
    }



    void OnDestroy()
    {
        foreach (var item in _gridCells)
        {
            item.ReturnToNormal(true);
        }

        if (_entrance != null)
            _entrance.ReturnToNormal(true);
    }
}
