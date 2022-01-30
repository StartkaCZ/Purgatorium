using UnityEngine;

public class Door : MonoBehaviour
{
    Vector3     _localPosition;
    Vector3     _localEuler;



    void Awake()
    {
        _localPosition = transform.localPosition;
        _localEuler = transform.localEulerAngles;
    }



    public void FixToParent(Transform parent)
    {
        transform.parent = parent;

        transform.localPosition = _localPosition;
        transform.localEulerAngles = _localEuler;
    }



    public Vector3 EntryPosition { get => transform.position + transform.forward * GridCell.GRID_SIZE; }

    public Vector3 ExitPosition { get => transform.position; }

    public Vector3 LocalPosition { get => _localPosition; }

    public Vector3 LocalEuler { get => _localEuler; }
}
