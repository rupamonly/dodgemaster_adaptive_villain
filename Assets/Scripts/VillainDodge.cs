using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    public float bounceForce = 5f;  // Adjust bounce intensity
    private Rigidbody2D rb;
    private bool recentlyBounced = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Detect collision with walls
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !recentlyBounced)
        {
            Debug.Log("Wall Hit! Bouncing...");

            // Get direction opposite to collision
            Vector2 bounceDirection = collision.contacts[0].normal;

            // Apply bounce force
            rb.velocity = bounceDirection * bounceForce;

            // Start cooldown to prevent multiple bounces
            StartCoroutine(BounceCooldown());
        }
    }

    IEnumerator BounceCooldown()
    {
        recentlyBounced = true;
        yield return new WaitForSeconds(0.5f);  // Adjust cooldown for smoother effect
        rb.velocity = Vector2.zero;  // Stop movement after bounce
        recentlyBounced = false;
    }
}
