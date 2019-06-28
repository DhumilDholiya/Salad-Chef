using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //unique for both player;

    public int PlayerID;

    [SerializeField] private float speed = 2f;
    [SerializeField] private KeyCode useKey;
    [SerializeField] private float choppingTime = 2f;

    private bool canMove = true;
    private Rigidbody2D rb;
    private int maxDishSize = 3;

    //gameobject to interact
    private GameObject toInteract = null;

    private bool isChopping = false;
    private bool startChoping = false;

    //choppign Board

    //UI variables
    public Text allVege = null;
    public Text readyDish = null;
    public Text[] dishOnChoppingBoard = new Text[2];
    public Text[] DishVege = new Text[2];


    //To store current vegetables holder
    private Queue<string> vegetablesInHand = new Queue<string>(); 
    //To store chopped vegetables
    private ChoppingBoard choppedVege;
    //To store Dish
    private Dish dishItem;
    //To store Readysalad
    private string readySalad;
    //To store Customer
    private Customer cust;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //store script component of interactables.
        if (toInteract)
        {
            Interactable objToInteract = toInteract.GetComponent<Interactable>();
            //check for vegetables
            if (objToInteract.Type == "Vegetable" && Input.GetKeyDown(useKey)
                && vegetablesInHand.Count < 2)
            {
                //add vege to hand
                string vegeName = objToInteract.Name;
                AddToPlayerHand(vegeName);
            }

            //check for chopping Board
            if (objToInteract.Type == "Board")
            {
                //storing argument to pass on function..
                int boardID = objToInteract.ID;
                choppedVege = toInteract.GetComponent<ChoppingBoard>();
                //chop vegetables.
                if (vegetablesInHand.Count > 0 && Input.GetKeyDown(useKey)
                    && choppedVege.Container.Count < 3 && !startChoping)
                {
                    isChopping = true;
                    StartCoroutine(ChoppingVege(boardID,choppedVege));
                }
                //pick up dish from Board.
                else if (vegetablesInHand.Count == 0 && Input.GetKeyDown(useKey)
                    && readyDish.text == "")
                {
                    //add dish on player hand
                    AddSaladOnPlayer(boardID);
                }
            }

            //check for Dustbin
            if (objToInteract.Type == "Dustbin")
            {
                //delete choppedvege list
                if (Input.GetKeyDown(useKey))
                {
                    if (choppedVege)
                    {
                        ClearChoppedVegeBoard(choppedVege);
                    }
                }
            }

            //check for Dish
            if (objToInteract.Type == "Dish" && Input.GetKeyDown(useKey))
            {
                int dishID = objToInteract.ID;
                dishItem = toInteract.GetComponent<Dish>();
                AddToDish(dishID, dishItem);
            }

            //check for Customer
            if (objToInteract.Type == "Customer" && Input.GetKeyDown(useKey))
            {
             //   Debug.Log("Customer Interaction.");
                cust = objToInteract.GetComponent<Customer>();
                isCustomerDone();
            }
        }
    }

    void FixedUpdate()
    {
        //move player
        if (!isChopping)
        {
            MovePlayer();
        }
    }

    public void MovePlayer()
    {
        if (!canMove)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);

        float h = Input.GetAxisRaw("Horizontal1");
        float v = Input.GetAxisRaw("Vertical1");

        Vector2 movementVector = new Vector2(h, v);
        rb.velocity = movementVector.normalized * speed ;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Interactable"))
        {
            toInteract = other.gameObject;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            if (other.gameObject == toInteract)
            {
                toInteract = null;
            }
        }
    }

    //For Customer
    void isCustomerDone()
    {
        //check for 70% timer or less.
        if (cust.dishWanted.Equals(readySalad))
        {
            //udate score.
            CustomerSpawner.isEmpty[cust.ind] = true;
            CustomerSpawner.DeleteCustomer(cust.ind);
            ClearChoppedVegeBoard(choppedVege);
        }
        else
        {
            // make customer Angry.
        }
    }

    //dustbin
    void ClearChoppedVegeBoard(ChoppingBoard choBoard)
    {
        choBoard.Container.Clear();
        choBoard.currChopped = 0;
        readyDish.text = "";
        readySalad = null;
    }

    void AddToPlayerHand(string curVege)
    {
        string vegeName = curVege;
        vegetablesInHand.Enqueue(vegeName);
        //updating UI.
        allVege.text = "";
        foreach (var i in vegetablesInHand)
        {
            allVege.text = allVege.text + i;
        }
    }

    IEnumerator ChoppingVege(int BoardID, ChoppingBoard choBoard)
    {
        string curChoppedVege = vegetablesInHand.Dequeue();
        startChoping = true;
        yield return new WaitForSeconds(choppingTime);
        choBoard.Container.Add(curChoppedVege);
        //updating UI.
        allVege.text = "";
        foreach (var i in vegetablesInHand)
        {
            allVege.text = allVege.text + i;
        }
        readySalad = readySalad + choBoard.Container[choBoard.currChopped];
        dishOnChoppingBoard[BoardID].text = readySalad;
        choBoard.currChopped++;
        isChopping = false;
        startChoping = false;
    }

    void AddSaladOnPlayer(int BoardID)
    {
        readyDish.text = dishOnChoppingBoard[BoardID].text;
        dishOnChoppingBoard[BoardID].text = " ";
    }

    void AddToDish(int DishID, Dish dish)
    {
        //check if dish is empty or not
        if (dish.DishContainer.Length == 0)
        {
            //add vege from hand to dish.
            if (vegetablesInHand.Count > 0)
            {
                dish.DishContainer = vegetablesInHand.Dequeue();
                allVege.text = "";
                foreach (var i in vegetablesInHand)
                {
                    allVege.text = allVege.text + i;
                }
                DishVege[DishID].text = dish.DishContainer;
            }
        }
        else if (dish.DishContainer.Length == 1)
        {
            //take vege from dish to hand.
            if (vegetablesInHand.Count<2)
            {
                AddToPlayerHand(dish.DishContainer);
                DishVege[DishID].text = "";
                dish.DishContainer = "";
            }
        }
    }

}
