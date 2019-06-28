using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [HideInInspector]public static int maxSpwanPos = 5;

    public Transform[] spawnPos = new Transform[5];
    public Transform[] reachPos = new Transform[5];
    public GameObject toSpawn;
    [HideInInspector]public static bool[] isEmpty = new bool[5];

    [HideInInspector] public static GameObject[] custObj = new GameObject[5];
    [HideInInspector]public static Customer[] customers = new Customer[5];

    private void Start()
    {
        for (int i = 0; i < maxSpwanPos; i++)
            isEmpty[i] = true;
    }

    private void Update()
    {

    }
    //generate random index to spawn.
    private int GenerateRandomNo()
    {
        int no =0;
        no = Random.Range(0, maxSpwanPos);
        return no;
    }


    public void SpawnCustomer()
    {
        int index = GenerateRandomNo();
        if (isEmpty[index])
        {
            custObj[index] = Instantiate(toSpawn, spawnPos[index].position,Quaternion.identity);
            customers[index] = custObj[index].GetComponent<Customer>();
            customers[index].posTospawn = spawnPos[index].position;
            customers[index].posToReach = reachPos[index].position;
            customers[index].ind = index;
            isEmpty[index] = false; 
        }
    }

    public static void DeleteCustomer(int index)
    {
        Destroy(custObj[index]);
    }
}
