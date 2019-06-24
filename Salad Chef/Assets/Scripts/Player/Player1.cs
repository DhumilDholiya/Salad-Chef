using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1 : MonoBehaviour {

    [SerializeField] private float speed = 2f;
    [SerializeField] private KeyCode useKey; 
    private bool canMove = true;
    private Rigidbody2D rb;

    //UI variables
    public Text firstVegetable = null;
    public Text secondVegetable = null;

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
        //check for Timeout
    }

    void FixedUpdate()
    {
        //move player
        MovePlayer();
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
    //    Debug.Log("Player 1 ::" + rb.velocity);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Vegetables"))
        {
            //store that vegetable in list if player doesnt have 2 vegetables in hand.
            if (vegetablesInHand.Count < 2 && Input.GetKeyDown(useKey))
            {
                AddToPlayerHand(other.gameObject);  
            }
        }
    }

    void AddToPlayerHand(GameObject curVege)
    {
        string vegeName = curVege.gameObject.name;
        Debug.Log(vegeName);
        vegetablesInHand.Add(vegeName);
        //check if it is first vegetable or a second
        if (vegetablesInHand.Count == 1)
        {
            firstVegetable.text = vegeName;
        }
        else if (vegetablesInHand.Count == 2)
        {
            secondVegetable.text = vegeName;
        }
    }
}
