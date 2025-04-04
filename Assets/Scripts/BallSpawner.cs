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

    
    private string originalGreenTag;
    private string originalRedTag;
    private string originalBlueTag;

    void Start()
    {
        originalGreenTag = greenBallPrefab.tag;
        originalRedTag = redBallPrefab.tag;
        originalBlueTag = blueBallPrefab.tag;
    }

    void Update()
    {
        CheckForTagChanges();

        // Change ball type with keys G, R, B
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

    
    void CheckForTagChanges()
    {
        if (greenBallPrefab.tag != originalGreenTag || redBallPrefab.tag != originalRedTag || blueBallPrefab.tag != originalBlueTag)
        {
            Debug.Log("Ball tags changed! Resetting perceptron...");
            VillainController villain = FindObjectOfType<VillainController>();
            if (villain != null)
            {
                villain.ResetPerceptron(); 
            }

            originalGreenTag = greenBallPrefab.tag;
            originalRedTag = redBallPrefab.tag;
            originalBlueTag = blueBallPrefab.tag;
        }
    }

    void ShootBall()
    {
        GameObject ball = Instantiate(selectedBall, transform.position, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * ballSpeed;

        string ballType = ball.tag == "Damage" ? "Damage" : "NoDamage";

        VillainController villain = FindObjectOfType<VillainController>();
        if (villain != null)
        {
            Vector2 ballDirection = rb.velocity.normalized;
            float ballSpeedMagnitude = rb.velocity.magnitude;

            villain.DodgeBall(ballDirection, ballType, ballSpeedMagnitude, false); 
        }

        Destroy(ball, 5f);  
    }
}



