using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorBuilding : MonoBehaviour
{

    [SerializeField]
    BoxCollider[]   _boxColliders;
    [SerializeField]
    BoxCollider[]   _entrances;
    [SerializeField]
    BoxCollider     _waitingRoom;
    [SerializeField]
    bool            _isUnlocked = false;



    void Awake()
    {
        _boxColliders = GetComponentsInChildren<BoxCollider>();
    }



    public BoxCollider[] BoxColliders { get => _boxColliders; }

    public BoxCollider[] Entraces { get => _entrances; }

    public BoxCollider WaitingRoom { get => _waitingRoom; }

    public bool IsUnlocked { get => _isUnlocked; }
}
