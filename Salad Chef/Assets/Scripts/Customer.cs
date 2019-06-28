using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public string dishWanted;
    [HideInInspector]public float timeToWait;
    [HideInInspector] public int ind;

    public GameObject healthBar;


    public float speed = 2f;
    public Vector3 posTospawn = new Vector3();
    public Vector3 posToReach = new Vector3();

    private float timePerOneVege = 800f;
    private int maxDishSize = 3;
    private char[] allAlphabets = new char[6] { 'A', 'B','C', 'D', 'E', 'F' };
    private bool isMoving = true;
    private bool isGenerated;
    public float _rateOfDecresing;

    //UI elements.
    public Text dishWantedUI;

    private void Start()
    {
        _rateOfDecresing = 1f;
        dishWanted = "";
        isGenerated = false;
    }
    private void Update()
    { 
        if(!isGenerated && !isMoving)
        {
            GenerateRandomString();
        }

        //updating Health and time to wait.
        if (!isMoving && timeToWait > 0)
        {
            UpdateHealthBar(healthBar);
            timeToWait -= _rateOfDecresing;
        }
        else if(!isMoving && timeToWait <=0f)
        {
            //reduce score for both players.
            //and destroy the customer.
            CustomerSpawner.DeleteCustomer(ind);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MoveToCounter();
        }
    }

    //updating Healthbar
    private void UpdateHealthBar(GameObject HealthBar)
    {
        float j= HealthBar.transform.localScale.x;
        float k = HealthBar.transform.localScale.y;
        HealthBar.transform.localScale = new Vector2(j-j*(1/timeToWait),k);
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
