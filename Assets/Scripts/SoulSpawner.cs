using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    Transform spawnPoint;
    public Queue<GameObject> soulQueue;
    int spawnCount = 0;
    Transform _spawnPoint;

    [SerializeField] float _waitTime = 1.0f;
    [SerializeField] GameObject _soul;
    [SerializeField] List<Transform> _positions;

    void Start()
    {
        soulQueue = new Queue<GameObject>();
        _spawnPoint = this.transform;
        StartCoroutine(spawner());
    }

    IEnumerator spawner()
    {
        while (spawnCount > 0)
        {
            int randPos = Random.Range(0, 2); // 0-1
            GameObject go = Instantiate(_soul, _spawnPoint);
            go.GetComponent<SoulBehaviour>().MoveTo(_positions[randPos]);
            spawnCount--;
            yield return new WaitForSeconds(_waitTime);
        }
        yield return new WaitUntil(() => spawnCount > 0);
        StartCoroutine(spawner());
    }

    public void spawn(int t_count)
    {
        spawnCount += t_count;
    }
}
