using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f
        private bool isFaningRight = true;

    [SerializeField] private Rigibody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Layermark groundLayer;


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();
    }
    private void FixedUpdate()

    {

        rb.velocity = new Vector2(horizontal * speed, rb velocity.y);


    }

    private void (Flip)

   {
        if (isFacingRight && horizontal< 0f !isFacingRight && horizontal> 0)
        {
            isFacingRight = isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale; 
        }
    }
}
