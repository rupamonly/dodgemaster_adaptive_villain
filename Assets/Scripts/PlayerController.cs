using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float bounceForce = 5f; 
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool recentlyBounced = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        if (!recentlyBounced) 
        {
            rb.velocity = moveInput * moveSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !recentlyBounced)
        {
            Debug.Log("Wall Hit! Bouncing...");

            
            Vector2 bounceDirection = collision.contacts[0].normal;

            
            rb.velocity = bounceDirection * bounceForce;

            
            StartCoroutine(BounceCooldown());
        }
    }

    IEnumerator BounceCooldown()
    {
        recentlyBounced = true;
        yield return new WaitForSeconds(0.5f); 
        rb.velocity=Vector2.zero;       
        recentlyBounced = false;
    }
}
