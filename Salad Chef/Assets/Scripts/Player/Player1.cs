using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1 : MonoBehaviour {

    [SerializeField] private float speed = 2f;
    [SerializeField] private KeyCode useKey;
    [SerializeField] private float choppingTime = 2f;

    private bool canMove = true;
    private Rigidbody2D rb;
    private int maxDishSize = 3;
    private int insCounter = 0;
    private int currChopped = 0;
    private string choppedVege;

    //gameobject to interact
    private GameObject toInteract = null;

    private bool isChopping = false;

    //choppign Board

    //UI variables
    public Text firstVegetable = null;
    public Text secondVegetable = null;
    public Text readyDish = null;
    public Text[] dishOnChoppingBoard = new Text[2];


    //To store current vegetables holder
    private List<string> vegetablesInHand = new List<string>();
    //To store chopped vegetables
    private List<string> choppedVegetables = new List<string>();

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
                && toInteract && vegetablesInHand.Count < 2)
            {
                //add vege to hand
                AddToPlayerHand(objToInteract);
            }

            //check for chopping Board
            if (objToInteract.Type == "Board" && toInteract)
            {
                //storing BoardID.
                int boardID = toInteract.gameObject.GetComponent<ChoppingBoard>().BoardID;
                //chop vegetables.
                if (vegetablesInHand.Count > 0 && Input.GetKeyDown(useKey)
                    && choppedVegetables.Count < 3)
                {
                    StartCoroutine(ChoppingVege(boardID));
                }
                //pick up dish from Board.
                else if (choppedVegetables.Count == 3 && vegetablesInHand.Count == 0
                    && Input.GetKeyDown(useKey))
                {
                    //add dish on player hand
                    AddDishOnPlayer(boardID);
                }
            }

            //check for Dustbin
            if (objToInteract.Type == "Dustbin" && toInteract)
            {
                //delete choppedvege list
                if (Input.GetKeyDown(useKey))
                {
                    ClearChoppedVegeBoard();
                }
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

    void ClearChoppedVegeBoard()
    {
        choppedVegetables.Clear();
        currChopped = 0;
        readyDish.text = " ";
    }

    void AddToPlayerHand(Interactable curVege)
    {
        string vegeName = curVege.Name;
        vegetablesInHand.Insert(insCounter, vegeName);
        //check if it is first vegetable or a second
        if (insCounter == 0)
        {
            firstVegetable.text = vegetablesInHand[0];
        }
        else if (insCounter == 1)
        {
            secondVegetable.text = vegetablesInHand[1];
        }
        insCounter++;
    }

    IEnumerator ChoppingVege(int BoardID)
    {
        isChopping = true;
        string curChoppedVege = vegetablesInHand[0];
        choppedVegetables.Add(curChoppedVege);
        yield return new WaitForSeconds(choppingTime);
        if (currChopped%2f == 0)
        {
            firstVegetable.text = "_";
            dishOnChoppingBoard[BoardID].text = dishOnChoppingBoard[BoardID].text + choppedVegetables[currChopped];
        }
        else if (currChopped%2f != 0)
        {
            secondVegetable.text = "_";
            dishOnChoppingBoard[BoardID].text = dishOnChoppingBoard[BoardID].text + choppedVegetables[currChopped];
        }
        currChopped++;
        vegetablesInHand.Remove(curChoppedVege);
        insCounter = 0;
        isChopping = false;
    }

    void AddDishOnPlayer(int BoardID)
    {
        readyDish.text = dishOnChoppingBoard[BoardID].text;
        dishOnChoppingBoard[BoardID].text = " ";
    }

}
