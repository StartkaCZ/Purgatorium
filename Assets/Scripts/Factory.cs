using UnityEngine;
// namespace
using EnumHolder;

public class Factory : MonoBehaviour
{
    static Factory      _instance;
    

    GameObject          _roomCornerPrefab;
    GameObject          _roomEdgePrefab;
    GameObject          _roomSpacePrefab;
    GameObject          _roomPrefab;
    GameObject          _roomDoorPrefab;
    GameObject          _roomCoreItemPrefab;



    public static Factory Instance()
    {
        if (_instance == null)
        {
            GameObject formationFactory = new GameObject("Factory");
            _instance = formationFactory.AddComponent<Factory>();

            _instance.Initialise();
        }

        return _instance;
    }


    private void Initialise()
    {
        _roomCornerPrefab = Resources.Load<GameObject>(RoomPart.Corner.ToString());
        _roomEdgePrefab = Resources.Load<GameObject>(RoomPart.Edge.ToString());
        _roomSpacePrefab = Resources.Load<GameObject>(RoomPart.Space.ToString());

        _roomPrefab = Resources.Load<GameObject>("Room");
        _roomDoorPrefab = Resources.Load<GameObject>("Door");
        _roomCoreItemPrefab = Resources.Load<GameObject>("CoreItem");
    }



    public GameObject GenerateRoomPart(RoomPart roomPart, Transform parent=null)
    {
        GameObject roomPartClone = null;

        switch (roomPart)
        {
            case RoomPart.Corner:
                roomPartClone = Instantiate(_roomCornerPrefab, parent);
                break;
            case RoomPart.Edge:
                roomPartClone = Instantiate(_roomEdgePrefab, parent);
                break;
            case RoomPart.Space:
                roomPartClone = Instantiate(_roomSpacePrefab, parent);
                break;

            default:
                break;
        }

        return roomPartClone;
    }


    public Room GenerateRoom()
    {
        return Instantiate(_roomPrefab).GetComponent<Room>();
    }


    public Door GenerateDoor(Transform parent)
    {
        return Instantiate(_roomDoorPrefab, parent, false).GetComponent<Door>();
    }

    public CoreItem GenerateCoreItem(Transform parent)
    {
        return Instantiate(_roomCoreItemPrefab, parent, false).GetComponent<CoreItem>();
    }
}
