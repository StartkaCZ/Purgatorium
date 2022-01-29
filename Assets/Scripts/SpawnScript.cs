using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public int maxSouls;
    int currentSouls;
    Vector3 spawnPosition;
    public GameObject SoulObject;
    public void OnValidate()
    {
        maxSouls = 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawn()
    {
        while (currentSouls < maxSouls)
        {
            yield return new WaitForSeconds(2f);
            spawnPosition = new Vector3(Random.Range(-15, 15), 1.25f, Random.Range(-15, 15));
            Instantiate(SoulObject, spawnPosition, Quaternion.identity);
            currentSouls++;
            Debug.Log("Spawned a soul");
        }
    }

}
