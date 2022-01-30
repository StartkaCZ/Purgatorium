using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DisasterType
{
    MILD,
    MEDIOCRE,
    DISASTER
}

enum MediocreEvents
{
    ClownCarAccident,
    SlippedOnBanannaPeel,
    LaughedToDeath,
    AteTooMuchPizza,
    MAX_CHOICE
}

enum DisasterEvents
{
    Sharknado,
    YourMum,
    MAX_CHOICE
}

public class WorldEvents : MonoBehaviour
{
    SoulSpawner _spawner;

    bool _spawn = false;

    /// <summary>
    /// Distater rating:
    /// Mild(1,3) (Don't show)
    /// Mediocre (5-8)
    /// Disaster(10-15)

    void Start()
    {
        _spawner = GetComponent<SoulSpawner>();
        StartCoroutine(testSpawner());
    }


    void Spawn(DisasterType type)
    {
        int min=0,max=0;
        switch(type)
        {
        case DisasterType.MILD:
            min = 1;
            max = 3;
            break;
        case DisasterType.MEDIOCRE:
            min = 5;
            max = 8;
            break;
        case DisasterType.DISASTER:
            min = 10;
            max = 15;
            break;
        }
        Debug.Log(type + " level Disaster happened");
        int soulsSpawned = Random.Range(min, max);
        _spawner.spawn(1);
    }


    IEnumerator testSpawner()
    {
        while(true)
        {
            int randNum = Random.Range(0, 100);
            DisasterType type;

            if (randNum < 5)
            { // 5% chance
                int randDisaster = Random.Range(0, (int)DisasterEvents.MAX_CHOICE);
                Spawn(DisasterType.DISASTER);
            }
            else if (randNum < 20)
            { // 20% chance
                int randomDisaster = Random.Range(0, (int)MediocreEvents.MAX_CHOICE);
                Spawn(DisasterType.MEDIOCRE);
            }
            else if (randNum < 70)// 50% chance
                Spawn(DisasterType.MILD);


            yield return new WaitForSeconds(Random.Range(3, 5));
        }
    }
}
