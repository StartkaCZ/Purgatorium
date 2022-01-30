using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorBuilding : MonoBehaviour
{
    [SerializeField]
    bool            _isUnlocked = false;

    BoxCollider[]   _boxColliders;



    void Awake()
    {
        _boxColliders = GetComponentsInChildren<BoxCollider>();
    }



    public BoxCollider[] BoxColliders { get => _boxColliders; }

    public bool IsUnlocked { get => _isUnlocked; }
}
