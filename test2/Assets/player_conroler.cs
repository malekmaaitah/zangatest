using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_conroler : MonoBehaviour
{
    private Animator m_Animator;
    private SpriteRenderer gfx;
    private Rigidbody2D rb;
    private Transform my_transform;
    private bool moving_left,moving_right,sprinting=false;
    [SerializeField]
    private Transform feet;
    [SerializeField]
    private LayerMask ground_layer;
    [SerializeField]
    private float h_speed=10,vspeed,slowing_speed,fall_multiplier;
    private float orginal_speed;

    
    
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        gfx = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        my_transform = GetComponent<Transform>();
        orginal_speed = h_speed;
       
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) 
        {
            Debug.Log(rb.velocity.x);
        }
       //using ray to jump and puling down if he reachs max hight
        RaycastHit2D grounded= Physics2D.Linecast(transform.position, feet.position,ground_layer);
        if (grounded)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {

               rb.AddForce(new Vector2(0, vspeed), ForceMode2D.Impulse);
                
            }
           
            

        }
       



       //movig left and right
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (grounded)
            {

            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * h_speed, rb.velocity.y);
            }
          
           
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                moving_right = true;
                moving_left = false;

            }else if(Input.GetAxisRaw("Horizontal") < 0)
            {
               moving_right = false;
                moving_left = true;
            }
            else
            {
                moving_left= false; 
                moving_right= false;
            }
        }
        else
        {
            moving_left= false;
            moving_right= false;
        }
       
       


        //stopping the player if he is not pressing left or right
        if(!moving_left && !moving_right&&grounded)
        {
            rb.velocity=Vector2.Lerp(rb.velocity,new Vector2(0,rb.velocity.y), Time.deltaTime*slowing_speed);
        }


        //fliping the sprite when moving left to right
        if (moving_right)
        {
           
            gfx.flipX = false;
        }else if (moving_left)
        {  
            gfx.flipX = true;
        }
       
        //animations
        if ((moving_right || moving_left)&&grounded) 
        {
            m_Animator.Play("player_walk");
        }
        if (!moving_left && !moving_right && grounded)
        {
            m_Animator.Play("player_idle");
        }
        if (!grounded)
        {
            m_Animator.Play("player_jump");
        }
        
    }

}
