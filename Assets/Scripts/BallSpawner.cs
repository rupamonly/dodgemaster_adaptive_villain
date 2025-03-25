using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject greenBallPrefab;
    public GameObject redBallPrefab;
    public GameObject blueBallPrefab;
    public float ballSpeed = 10f;
    private GameObject selectedBall;

    void Update()
    {
        // Change ball type with keys 1, 2, 3
        if (Input.GetKeyDown(KeyCode.G))
        {
            selectedBall = greenBallPrefab;
            Debug.Log("Green Ball Selected");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            selectedBall = redBallPrefab;
            Debug.Log("Red Ball Selected");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            selectedBall = blueBallPrefab;
            Debug.Log("Blue Ball Selected");
        }

        // Shoot the selected ball with Space key
        if (Input.GetKeyDown(KeyCode.Space) && selectedBall != null)
        {
            ShootBall();
        }
    }

// void OnCollisionEnter2D(Collision2D collision)
//     {
//         // Destroy ball immediately if it hits a wall
//         if (collision.gameObject.CompareTag("Wall"))
//         {
//             Destroy(gameObject);
//         }
//     }

    void ShootBall()
{
    GameObject ball = Instantiate(selectedBall, transform.position, Quaternion.identity);
    Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
    rb.velocity = transform.up * ballSpeed;

    // Determine ball type
    string ballType = "NoDamage";
    if (selectedBall == redBallPrefab || selectedBall == blueBallPrefab)
    {
        ballType = "Damage";
    }

    // Notify villain to dodge if ready
    VillainController villain = FindObjectOfType<VillainController>();
    if (villain != null)
    {
        Vector2 ballDirection = rb.velocity.normalized;
        float ballSpeedMagnitude = rb.velocity.magnitude;

        villain.DodgeBall(ballDirection, ballType, ballSpeedMagnitude, false);
    }

    Destroy(ball, 5f);  // Destroy ball after 5 seconds
}

}
