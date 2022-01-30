using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreItem : MonoBehaviour
{
    [SerializeField]
    List<Transform> _floorPanels;

    Vector3         _localPosition;
    Vector3         _localEuler;

    bool            _canRotate;



    void Awake()
    {
        _localPosition = transform.localPosition;
        _localEuler = transform.localEulerAngles;

        _canRotate = true;
    }



    public void FixToParent(Transform parent)
    {
        transform.parent = parent;

        transform.localPosition = _localPosition;
        transform.localEulerAngles = _localEuler;
    }



    public void Rotate90()
    {
        if (_canRotate)
        {
            _localEuler.y += 90;
            transform.eulerAngles = _localEuler;

            StartCoroutine(RotationCooldown());
        }
    }

    private IEnumerator RotationCooldown()
    {
        _canRotate = false;

        yield return new WaitUntil(() => !Input.GetKey(KeyCode.R));

        _canRotate = true;
    }



    public List<Transform> FloorPanels { get => _floorPanels; }

    public Vector3 LocalPosition { get => _localPosition; }

    public Vector3 LocalEuler { get => _localEuler; }
}
