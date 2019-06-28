using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public string dishWanted;
    [HideInInspector]public float timeToWait;
    [HideInInspector] public int ind;

    public float speed = 2f;
    public Vector3 posTospawn = new Vector3();
    public Vector3 posToReach = new Vector3();

    private float timePerOneVege = 10f;
    private int maxDishSize = 3;
    private char[] allAlphabets = new char[6] { 'A', 'B','C', 'D', 'E', 'F' };
    private bool isMoving = true;
    private bool isGenerated;

    //UI elements.
    public Text dishWantedUI;

    private void Start()
    {
        dishWanted = "";
        isGenerated = false;
    }
    private void Update()
    { 
        if(!isGenerated && !isMoving)
        {
            GenerateRandomString();
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MoveToCounter();
        }
    }

    //generating wantedDish for Customer Randomly.
    private void GenerateRandomString()
    {
        int dishSize = Random.Range(1,maxDishSize +1);
        //time to wait for salad is dependent on disSize;
        timeToWait = dishSize * timePerOneVege;
        for (int i=0; i < dishSize; i++)
        {
            int index = Random.Range(0,6);
            dishWanted = dishWanted + allAlphabets[index];
        }
        //updating UI.
        dishWantedUI.text = dishWanted;
        isGenerated = true;
    }

    private void MoveToCounter()
    {
        isMoving = true;
        if (posToReach != transform.position)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        else if (posToReach == transform.position)
        {
            isMoving = false;
        }
    }
}
