using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum SoulState
{
    MOVING,
    WANDER,
    IDLE
}

public class SoulBehaviour : MonoBehaviour
{
    [SerializeField] Transform roomCenter;
    NavMeshAgent _agent;

    SoulState _state;

    float _timer = 2;
    float _currentTime;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }


    public void MoveTo(Transform t_roomCenter)
    {
        roomCenter = t_roomCenter;
        _agent.SetDestination(roomCenter.position);
        _state = SoulState.MOVING;
    }

    private void Update()
    {
        if (Vector3.Distance(roomCenter.position, transform.position) < 2 && _state == SoulState.MOVING)
        {
            int newState = Random.Range(1, 3);
            _state = (SoulState)newState;
        }
        else
        {
            switch (_state)
            {
            case SoulState.MOVING:
                break;
            case SoulState.IDLE:
                break;
            case SoulState.WANDER:
                if (_currentTime < Time.time)
                {
                    Vector3 pos = getNext();
                    _agent.SetDestination(pos);

                    _currentTime = Time.time + _timer;
                }
                break;
            }
        }
    }


    Vector3 getNext()
    {
        Vector3 pos = roomCenter.position;
        Vector3 scale = roomCenter.localScale;
        float hx = 5f * scale.x;
        float hz = 5f * scale.z;

        float randomX = Random.Range(-hx, hx);
        float randomZ = Random.Range(-hz, hz);

        return new Vector3(pos.x + randomX, pos.y, pos.z + randomZ);
    }
}
