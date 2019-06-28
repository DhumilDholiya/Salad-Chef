using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CustomerSpawner spawner;
    private float timeBtwnSpawn = 2f;
    private float currTime;

    //scoring system
    private float score;
    //Main Timer
    private float maxTime;
    private float time;

    private void Start()
    {
        spawner = GetComponent<CustomerSpawner>();
        currTime = 0f;
    }

    private void Update()
    {
        if (Time.time >= currTime)
        {
            currTime = Time.time + timeBtwnSpawn;
         //   Debug.Log("Spawn.");
            spawner.SpawnCustomer();
        }
    }


}

