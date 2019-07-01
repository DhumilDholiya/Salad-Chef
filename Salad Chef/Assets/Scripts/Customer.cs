using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public string dishWanted;
    [HideInInspector]public float timeToWait;
    [HideInInspector] public int ind;

    public GameObject[] powerUps = new GameObject[3];
    public GameObject healthBar;


    float custDisappointed = -1f;

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    public float speed = 2f;
    public Vector3 posTospawn = new Vector3();
    public Vector3 posToReach = new Vector3();

    private float timePerOneVege = 500f;
    private int maxDishSize = 3;
    private char[] allAlphabets = new char[6] { 'A', 'B','C', 'D', 'E', 'F' };
    private bool isMoving = true;
    private bool isGenerated;

    public bool isCustAngry = false;
    public int angryAtPlayer;
    public bool isCusthappy;
    public int happyAtPlayer;
    public float _rateOfDecresing;

    float currTime =0f;

    PowerUp powerUp;

    //UI elements.
    public Text dishWantedUI;

    private void Start()
    {
        _rateOfDecresing = 50f;
        isCusthappy = false;
        dishWanted = "";
        isGenerated = false;
    }
    private void Update()
    {
        if (!isGenerated && !isMoving)
        {
            GenerateRandomString();
            currTime = timeToWait;
        }

        //updating Health and time to wait.
        if (!isMoving)
        {
            if(currTime > 0f)
            {
                UpdateHealthBar(healthBar);
                currTime -= _rateOfDecresing * Time.deltaTime;
            }
        }
        if(!isMoving && currTime <= 0f)
        {
            //reduce score for both players.
            Debug.Log("done");
            if (!isCustAngry)
            {
                GameManager._scorePlayer1 += custDisappointed;
                GameManager._scorePlayer2 += custDisappointed;
            }
            else if(isCustAngry && angryAtPlayer ==1)
            {
                GameManager._scorePlayer1 += custDisappointed * 2;
            }
            else if(isCustAngry && angryAtPlayer == 2)
            {
                GameManager._scorePlayer2 += custDisappointed * 2;
            }
            //and destroy the customer.
            CustomerSpawner.DeleteCustomer(ind);
        }
    }

    private void OnDestroy()
    {
        if (isCusthappy)
        {
            isCustomerHappy();
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MoveToCounter();
        }
    }
    private Vector2 GenerateRandomPosition()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        Vector2 randPos = new Vector2(x, y);
        return randPos;
    }
    public void isCustomerHappy()
    {
        //isntantiate powerUps.
        int x = Random.Range(0, 3);
        Vector3 randPos = GenerateRandomPosition();
        GameObject powUp =  Instantiate(powerUps[x], randPos, Quaternion.identity) as GameObject;
        powerUp = powUp.GetComponent<PowerUp>();
        powerUp.playerId = happyAtPlayer;
    }


    //updating Healthbar
    private void UpdateHealthBar(GameObject HealthBar)
    {
        float j= HealthBar.transform.localScale.x;
        float k = HealthBar.transform.localScale.y;
        HealthBar.transform.localScale = new Vector2(currTime/timeToWait,k);
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
