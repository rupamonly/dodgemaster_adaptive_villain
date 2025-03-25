using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy ball immediately if it hits a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}


