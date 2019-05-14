using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {

    private float speed = 2f;
    private bool canMove = true;
    private Rigidbody2D rb;

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

        float h = Input.GetAxisRaw("Horizontal2");
        float v = Input.GetAxisRaw("Vertical2");

        Vector2 movementVector = new Vector2(h, v)  ;
        rb.velocity = movementVector.normalized * speed;
        Debug.Log("Player 2 ::" + rb.velocity);
    }
}
