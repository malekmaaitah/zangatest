using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_conroler : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform my_transform;
   
    public float h_speed=10,vspeed,ds;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        my_transform = GetComponent<Transform>();
    }
    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * h_speed, rb.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(0, 0); 
            rb.AddForce(new Vector2(0, vspeed));
        }
    }

}
