using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillainController : MonoBehaviour
{
    private Perceptron perceptron;
    private Rigidbody2D rb;
    public float dodgeSpeed = 5f;
    private bool canDodge = false;
    private int redBallHits = 0; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        perceptron = new Perceptron(4); 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            Debug.Log("Hit by: " + collision.gameObject.name);

            // Increase hit count
            redBallHits++;

            // Enable dodging after 2 hits
            if (redBallHits >= 5)
            {
                canDodge = true;
                Debug.Log("Villain learned to dodge after 2 hits!");
            }

            // Learn and update perceptron
            TrainPerceptron(collision.gameObject.transform.position, "Damage", 5f, true); 

            StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f)); 
        }
        else if (collision.gameObject.CompareTag("NoDamage"))
        {
            Debug.Log("Hit by: " + collision.gameObject.name + " (No damage, no dodge)");
            StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));
        }
    }

    // Pass ball data to Perceptron
    public void DodgeBall(Vector2 ballDirection, string ballType, float ballSpeed, bool lastDodgeSuccess)
    {
        if (canDodge)
        {
            float ballTypeInput = (ballType == "Damage") ? 1f : 0f;
            float ballDistance = Vector2.Distance(transform.position, ballDirection);
            float normalizedDistance = Mathf.Clamp01(ballDistance / 10f);
            float normalizedSpeed = Mathf.Clamp01(ballSpeed / 15f);
            float dodgeSuccess = lastDodgeSuccess ? 1f : 0f;

            float[] inputs = { ballTypeInput, normalizedDistance, normalizedSpeed, dodgeSuccess };
            int decision = perceptron.Predict(inputs);

            Debug.Log($"Inputs: Type={ballTypeInput}, Distance={normalizedDistance}, Speed={normalizedSpeed}, LastDodge={dodgeSuccess}");
            Debug.Log($"Perceptron Decision: {decision}");

            if (ballType == "Damage" && decision == 1)
            {
                Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;
                rb.velocity = dodgeDirection * dodgeSpeed;
                Debug.Log("Dodge Successful: " + dodgeDirection);
            }
            else
            {
                Debug.Log("No dodge executed. Ball type: " + ballType);
            }

            // Train perceptron based on success/failure
            TrainPerceptron(ballDirection, ballType, ballSpeed, decision == 1);
        }
    }

    // Train perceptron based on success/failure
    public void TrainPerceptron(Vector2 ballDirection, string ballType, float ballSpeed, bool success)
    {
        float ballTypeInput = (ballType == "Damage") ? 1f : 0f;
        float ballDistance = Vector2.Distance(transform.position, ballDirection);
        float normalizedDistance = Mathf.Clamp01(ballDistance / 10f);
        float normalizedSpeed = Mathf.Clamp01(ballSpeed / 15f);
        float dodgeSuccess = success ? 1f : 0f;

        float[] inputs = { ballTypeInput, normalizedDistance, normalizedSpeed, dodgeSuccess };
        int targetOutput = success ? 1 : 0; // 1 = Successful dodge, 0 = Fail
        perceptron.Train(inputs, targetOutput);
    }

    // Destroy ball after delay
    IEnumerator DestroyBallWithDelay(GameObject ball, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(ball);
    }
}



// public class VillainController : MonoBehaviour
// {
//     public int health = 100;  // High health to avoid death
//     private int redBallHits = 0;  // Track hits for perceptron learning
//     public float dodgeSpeed = 5f;
//     private Rigidbody2D rb;

//     private Vector2 lastBallDirection;
//     private Perceptron perceptron;
//     private bool canDodge = false;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         perceptron = new Perceptron();  // Initialize perceptron
//     }

//     // Detect collision with ball
//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Damage"))
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name);

//             // Learn and react to dodge logic
//             LearnToDodge(collision.gameObject);

//             // Destroy the ball after delay
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));
//         }
//         else if (collision.gameObject.CompareTag("NoDamage"))
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name + " (No damage, no dodge)");
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));  // Destroy without dodging
//         }
//     }

//     // Learn to dodge logic
//     void LearnToDodge(GameObject ball)
//     {
//         lastBallDirection = (ball.transform.position - transform.position).normalized;
//         redBallHits++;

//         if (redBallHits >= 2 && !canDodge)
//         {
//             canDodge = true;
//             Debug.Log("Villain learned to dodge!");
//         }

//         // Prepare perceptron inputs
//         float[] inputs = { lastBallDirection.x, (ball.CompareTag("Damage") ? 1f : 0f) };

//         // Target output: Dodge (1) or No Dodge (0)
//         int target = (ball.CompareTag("Damage")) ? 1 : 0;

//         // Train perceptron after each hit
//         perceptron.Train(inputs, target);

//         // Use perceptron to decide dodge
//         if (canDodge && BallIsClose(ball.transform.position))
//         {
//             int dodgeDecision = perceptron.Predict(inputs);
//             if (dodgeDecision == 1)
//             {
//                 DodgeBall(lastBallDirection);
//             }
//             else
//             {
//                 Debug.Log("Perceptron decided not to dodge!");
//             }
//         }
//     }

//     // Check if ball is close enough to dodge
//     bool BallIsClose(Vector2 ballPosition)
//     {
//         float distanceToBall = Vector2.Distance(transform.position, ballPosition);
//         return distanceToBall <= 3f;
//     }

//     // Dodge logic (move left or right)
//     public void DodgeBall(Vector2 ballDirection)
//     {
//         Debug.Log("Dodging Ball!");

//         Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;
//         rb.velocity = dodgeDirection * dodgeSpeed;

//         StartCoroutine(StopDodging());
//     }

//     IEnumerator StopDodging()
//     {
//         yield return new WaitForSeconds(0.5f);
//         rb.velocity = Vector2.zero;
//     }

//     IEnumerator DestroyBallWithDelay(GameObject ball, float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         Destroy(ball);
//     }
// }



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class VillainController : MonoBehaviour
// {
//     public int health = 100;  // High health to avoid death
//     private int redBallHits = 0;  // Track hits for perceptron learning
//     public float dodgeSpeed = 5f;
//     private bool canDodge = false;
//     private Rigidbody2D rb;

//     private Vector2 lastBallDirection;  // Track last ball direction for dodging
//     public float wallDetectionDistance = 1f;  // Distance to detect walls
//     private bool isDodging = false;  // Flag to track ongoing dodge

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     // Detect collision with ball
//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Damage"))  // Only red and blue balls
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name);

//             // Learn to dodge after 2 hits
//             LearnToDodge(collision.gameObject);

//             // Delay ball destruction after 1.0 seconds
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));
//         }
//         else if (collision.gameObject.CompareTag("NoDamage"))  // Ignore green ball
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name + " (No damage, no dodge)");
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));  // Destroy the green ball without dodge
//         }
//     }

//     // Learn to dodge logic
//     void LearnToDodge(GameObject ball)
//     {
//         // Skip learning if the ball is NoDamage
//         if (ball.CompareTag("NoDamage"))
//         {
//             Debug.Log("No learning required for NoDamage ball.");
//             return;
//         }

//         redBallHits++;  // Increase hit count
//         lastBallDirection = (ball.transform.position - transform.position).normalized;

//         Debug.Log("Hit by: " + ball.tag + ". Hits so far: " + redBallHits);

//         // Activate dodge after 2 hits
//         if (redBallHits >= 2 && !canDodge)
//         {
//             canDodge = true;
//             Debug.Log("Villain learned to dodge!");
//         }

//         // Dodge if learned and ball is close enough (but not for NoDamage)
//         if (canDodge && BallIsClose(ball.transform.position) && ball.CompareTag("Damage"))
//         {
//             DodgeBall(lastBallDirection);
//         }
//     }

//     // Check if the ball is close enough to trigger dodge
//     bool BallIsClose(Vector2 ballPosition)
//     {
//         float distanceToBall = Vector2.Distance(transform.position, ballPosition);
//         return distanceToBall <= 3f;  // Dodge only if ball is within 3 units
//     }

//     // Coroutine to destroy ball after delay
//     IEnumerator DestroyBallWithDelay(GameObject ball, float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         Destroy(ball);
//     }

//     // Dodge logic (move left or right)
//     public void DodgeBall(Vector2 ballDirection)
//     {
//         if (canDodge && !isDodging)
//         {
//             Debug.Log("Dodging Ball!");

//             // Randomly decide to dodge left or right
//             Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;

//             // Check if wall is detected before dodging
//             if (!WallDetectedInDirection(dodgeDirection))
//             {
//                 StartCoroutine(SmoothDodge(dodgeDirection * dodgeSpeed));
//             }
//             else
//             {
//                 Debug.Log("Wall detected! Cannot dodge in that direction.");
//             }
//         }
//     }

//     // Coroutine to perform smooth dodging
//     IEnumerator SmoothDodge(Vector2 dodgeVelocity)
//     {
//         isDodging = true;
//         float dodgeTime = 0.3f;  // Duration for smooth dodge
//         float elapsedTime = 0f;

//         Vector2 startVelocity = rb.velocity;
//         while (elapsedTime < dodgeTime)
//         {
//             rb.velocity = Vector2.Lerp(startVelocity, dodgeVelocity, elapsedTime / dodgeTime);
//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         rb.velocity = dodgeVelocity;

//         // Stop dodging after a short duration
//         yield return new WaitForSeconds(0.3f);  // Dodge duration
//         rb.velocity = Vector2.zero;
//         isDodging = false;
//     }

//     // Check for wall in dodge direction
//     bool WallDetectedInDirection(Vector2 dodgeDirection)
//     {
//         RaycastHit2D hit = Physics2D.Raycast(transform.position, dodgeDirection, wallDetectionDistance);
//         if (hit.collider != null && hit.collider.CompareTag("Wall"))
//         {
//             return true;  // Wall detected
//         }
//         return false;  // No wall detected
//     }
// }


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class VillainController : MonoBehaviour
// {
//     public int health = 100;  // High health to avoid death
//     private int redBallHits = 0;  // Track hits for perceptron learning
//     public float dodgeSpeed = 5f;
//     private bool canDodge = false;
//     private Rigidbody2D rb;

//     private Vector2 lastBallDirection;  // Track last ball direction for dodging
//     public float wallDetectionDistance = 1f;  // Distance to detect walls
//     private bool isDodging = false;  // Flag to track ongoing dodge

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     // Detect collision with ball
//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Damage"))  // Only red and blue balls now
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name);

//             // Learn to dodge after 2 hits
//             LearnToDodge(collision.gameObject);

//             // Delay ball destruction after 1.0 seconds
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));
//         }
//         else if (collision.gameObject.CompareTag("NoDamage"))  // Ignore green ball
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name + " (No damage, no dodge)");
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));  // Destroy the green ball without dodge
//         }
//     }

//     // Learn to dodge logic
//     void LearnToDodge(GameObject ball)
//     {
//         redBallHits++;  // Increase hit count
//         lastBallDirection = (ball.transform.position - transform.position).normalized;

//         Debug.Log("Hit by: " + ball.tag + ". Hits so far: " + redBallHits);

//         // Activate dodge after 2 hits
//         if (redBallHits >= 2 && !canDodge)
//         {
//             canDodge = true;
//             Debug.Log("Villain learned to dodge!");
//         }

//         // Dodge if learned and ball is close enough
//         if (canDodge && BallIsClose(ball.transform.position))
//         {
//             DodgeBall(lastBallDirection);
//         }
//     }

//     // Check if the ball is close enough to trigger dodge
//     bool BallIsClose(Vector2 ballPosition)
//     {
//         float distanceToBall = Vector2.Distance(transform.position, ballPosition);
//         return distanceToBall <= 3f;  // Dodge only if ball is within 3 units
//     }

//     // Coroutine to destroy ball after delay
//     IEnumerator DestroyBallWithDelay(GameObject ball, float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         Destroy(ball);
//     }

//     // Dodge logic (move left or right)
//     public void DodgeBall(Vector2 ballDirection)
//     {
//         if (canDodge && !isDodging)
//         {
//             Debug.Log("Dodging Ball!");

//             // Randomly decide to dodge left or right
//             Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;

//             // Check if wall is detected before dodging
//             if (!WallDetectedInDirection(dodgeDirection))
//             {
//                 StartCoroutine(SmoothDodge(dodgeDirection * dodgeSpeed));
//             }
//             else
//             {
//                 Debug.Log("Wall detected! Cannot dodge in that direction.");
//             }
//         }
//     }

//     // Coroutine to perform smooth dodging
//     IEnumerator SmoothDodge(Vector2 dodgeVelocity)
//     {
//         isDodging = true;
//         float dodgeTime = 0.3f;  // Duration for smooth dodge
//         float elapsedTime = 0f;

//         Vector2 startVelocity = rb.velocity;
//         while (elapsedTime < dodgeTime)
//         {
//             rb.velocity = Vector2.Lerp(startVelocity, dodgeVelocity, elapsedTime / dodgeTime);
//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         rb.velocity = dodgeVelocity;

//         // Stop dodging after a short duration
//         yield return new WaitForSeconds(0.3f);  // Dodge duration
//         rb.velocity = Vector2.zero;
//         isDodging = false;
//     }

//     // Check for wall in dodge direction
//     bool WallDetectedInDirection(Vector2 dodgeDirection)
//     {
//         RaycastHit2D hit = Physics2D.Raycast(transform.position, dodgeDirection, wallDetectionDistance);
//         if (hit.collider != null && hit.collider.CompareTag("Wall"))
//         {
//             return true;  // Wall detected
//         }
//         return false;  // No wall detected
//     }
// }





// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class VillainController : MonoBehaviour
// {
//     public int health = 100;  // High health to avoid death
//     private int redBallHits = 0;  // Track hits for perceptron learning
//     public float dodgeSpeed = 5f;
//     private bool canDodge = false;
//     private Rigidbody2D rb;

//     private Vector2 lastBallDirection;  // Track last ball direction for dodging

//     public float wallDetectionDistance = 1f;  // Distance to detect walls

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     // Detect collision with ball
//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("RedBall") || collision.gameObject.CompareTag("BlueBall"))
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name);

//             // Learn to dodge after 2 hits
//             LearnToDodge(collision.gameObject);

//             // Delay ball destruction after 1.0 seconds
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));
//         }
//     }

//     // Learn to dodge logic
//     void LearnToDodge(GameObject ball)
//     {
//         redBallHits++;  // Increase hit count
//         lastBallDirection = (ball.transform.position - transform.position).normalized;

//         Debug.Log("Hit by: " + ball.tag + ". Hits so far: " + redBallHits);

//         // Activate dodge after 2 hits
//         if (redBallHits >= 2 && !canDodge)
//         {
//             canDodge = true;
//             Debug.Log("Villain learned to dodge!");
//         }

//         // Dodge if learned and ball is close enough
//         if (canDodge && BallIsClose(ball.transform.position))
//         {
//             DodgeBall(lastBallDirection);
//         }
//     }

//     // Check if the ball is close enough to trigger dodge
//     bool BallIsClose(Vector2 ballPosition)
//     {
//         float distanceToBall = Vector2.Distance(transform.position, ballPosition);
//         return distanceToBall <= 3f;  // Dodge only if ball is within 3 units
//     }

//     // Coroutine to destroy ball after delay
//     IEnumerator DestroyBallWithDelay(GameObject ball, float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         Destroy(ball);
//     }

//     // Dodge logic (move left or right)
//     public void DodgeBall(Vector2 ballDirection)
//     {
//         if (canDodge)
//         {
//             Debug.Log("Dodging Ball!");

//             // Randomly decide to dodge left or right
//             Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;

//             // Check if wall is detected before dodging
//             if (!WallDetectedInDirection(dodgeDirection))
//             {
//                 rb.velocity = dodgeDirection * dodgeSpeed;

//                 // Stop dodging after a short duration
//                 StartCoroutine(StopDodging());
//             }
//             else
//             {
//                 Debug.Log("Wall detected! Cannot dodge in that direction.");
//             }
//         }
//     }

//     // Check for wall in dodge direction
//     bool WallDetectedInDirection(Vector2 dodgeDirection)
//     {
//         RaycastHit2D hit = Physics2D.Raycast(transform.position, dodgeDirection, wallDetectionDistance);
//         if (hit.collider != null && hit.collider.CompareTag("Wall"))
//         {
//             return true;  // Wall detected
//         }
//         return false;  // No wall detected
//     }

//     IEnumerator StopDodging()
//     {
//         yield return new WaitForSeconds(0.5f);  // Dodge for 0.5 seconds
//         rb.velocity = Vector2.zero;  // Stop movement after dodge
//     }
// }





// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;



// // using System.Collections;
// // using UnityEngine;

// public class VillainController : MonoBehaviour
// {
//     public int health = 100;  // High health to avoid death
//     private int redBallHits = 0;  // Track hits for perceptron learning
//     public float dodgeSpeed = 5f;
//     private bool canDodge = false;
//     private Rigidbody2D rb;
//     private bool recentlyDodged = false;  // Prevents repeated dodging
//     private Vector2 lastBallDirection;  // Track last ball direction for dodging

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     // Detect collision with ball
//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("RedBall") || collision.gameObject.CompareTag("BlueBall"))
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name);

//             // Learn to dodge after 2 hits
//             LearnToDodge(collision.gameObject);

//             // Delay ball destruction after 1 second
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));
//         }
//     }

//     // Learn to dodge logic
//     void LearnToDodge(GameObject ball)
//     {
//         redBallHits++;
//         lastBallDirection = (ball.transform.position - transform.position).normalized;

//         Debug.Log("Hit by: " + ball.tag + ". Hits so far: " + redBallHits);

//         // Activate dodge after 2 hits
//         if (redBallHits >= 2 && !canDodge)
//         {
//             canDodge = true;
//             Debug.Log("Villain learned to dodge!");
//         }

//         // Dodge only if the ball is close
//         if (canDodge && Vector2.Distance(ball.transform.position, transform.position) <= 2.5f)
//         {
//             DodgeBall(lastBallDirection);
//         }
//     }

//     // Dodge logic (move left or right)
//     public void DodgeBall(Vector2 ballDirection)
//     {
//         if (canDodge && !recentlyDodged)
//         {
//             Debug.Log("Dodging Ball!");

//             // Randomly decide to dodge left or right
//             Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;

//             // Apply dodge movement
//             rb.velocity = dodgeDirection * dodgeSpeed;

//             // Prevent multiple dodges in quick succession
//             StartCoroutine(DodgeCooldown());
//         }
//     }

//     // Prevent multiple dodges by cooldown
//     IEnumerator DodgeCooldown()
//     {
//         recentlyDodged = true;
//         yield return new WaitForSeconds(0.8f);  // Dodge cooldown
//         rb.velocity = Vector2.zero;  // Stop movement after dodge
//         recentlyDodged = false;
//     }

//     // Destroy ball after delay
//     IEnumerator DestroyBallWithDelay(GameObject ball, float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         Destroy(ball);
//     }
// }


// public class VillainController : MonoBehaviour
// {
//     public int health = 100;  // High health to prevent defeat
//     private int redBallHits = 0;  // Track hits for perceptron learning
//     public float dodgeSpeed = 5f;
//     private bool canDodge = false;
//     private Rigidbody2D rb;

//     private Vector2 lastBallDirection;  // Track last ball direction for dodging
//     private bool recentlyBounced = false;  // Prevent overriding dodge effect

//     // Boundary values (set dynamically from Inspector)
//     public float minX = -8f, maxX = 8f, minY = -4.5f, maxY = 4.5f;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     void Update()
//     {
//         // Restrict position to boundary to prevent leaving play area
//         Vector2 clampedPosition = new Vector2(
//             Mathf.Clamp(transform.position.x, minX, maxX),
//             Mathf.Clamp(transform.position.y, minY, maxY)
//         );

//         transform.position = clampedPosition;
//     }

//     // Detect collision with ball
//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if ((collision.gameObject.CompareTag("RedBall") || collision.gameObject.CompareTag("BlueBall")) && !recentlyBounced)
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name);

//             // Learn to dodge after 2-3 hits
//             LearnToDodge(collision.gameObject);

//             // Delay ball destruction after 1.0 seconds
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f));
//         }
//     }

//     // Learn to dodge logic
//     void LearnToDodge(GameObject ball)
//     {
//         redBallHits++;  // Increase hit count
//         lastBallDirection = (ball.transform.position - transform.position).normalized;

//         Debug.Log("Hit by: " + ball.tag + ". Hits so far: " + redBallHits);

//         // Activate dodge after 2 hits
//         if (redBallHits >= 2 && !canDodge)
//         {
//             canDodge = true;
//             Debug.Log("Villain learned to dodge!");
//         }

//         // Dodge immediately if learned and ball is close
//         if (canDodge && Vector2.Distance(ball.transform.position, transform.position) <= 3f)
//         {
//             DodgeBall(lastBallDirection);
//         }
//     }

//     // Coroutine to destroy ball after delay
//     IEnumerator DestroyBallWithDelay(GameObject ball, float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         Destroy(ball);
//     }

//     // Dodge logic (move left or right based on ball position)
//     public void DodgeBall(Vector2 ballDirection)
//     {
//         if (canDodge && !recentlyBounced)
//         {
//             Debug.Log("Dodging Ball!");

//             // Randomly decide to dodge left or right
//             Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;

//             // Apply dodge movement perpendicular to ball direction
//             Vector2 targetPosition = (Vector2)transform.position + dodgeDirection * dodgeSpeed * Time.deltaTime;

//             // Clamp position to prevent leaving boundaries
//             targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
//             targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

//             rb.MovePosition(targetPosition);

//             // Start cooldown to prevent multiple triggers
//             StartCoroutine(DodgeCooldown());
//         }
//     }

//     // Stop dodging after a short time
//     IEnumerator DodgeCooldown()
//     {
//         recentlyBounced = true;
//         yield return new WaitForSeconds(0.5f);  // Dodge duration
//         rb.velocity = Vector2.zero;  // Stop movement after dodge
//         recentlyBounced = false;
//     }
// }

















// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class VillainController : MonoBehaviour
// {
//     public int health = 100;  // High health to avoid death
//     private int redBallHits = 0;  // Track hits for perceptron learning
//     public float dodgeSpeed = 5f;
//     private bool canDodge = false;
//     private Rigidbody2D rb;

//     private Vector2 lastBallDirection;  // Track last ball direction for dodging

//     public float bounceForce = 5f;

//     private bool recentlyBounced = false;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     // Detect collision with ball
//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("RedBall") || collision.gameObject.CompareTag("BlueBall"))
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name);

//             // Learn to dodge after 2-3 hits
//             LearnToDodge(collision.gameObject);

//             // Delay ball destruction after 1.0 seconds
//             StartCoroutine(DestroyBallWithDelay(collision.gameObject, 1.0f)); 
//         }
//         if (collision.gameObject.CompareTag("Wall") && !recentlyBounced)
//         {
//             Debug.Log("Villain hit the wall! Bouncing...");

//             // Get direction opposite to collision
//             Vector2 bounceDirection = collision.contacts[0].normal;

//             // Apply bounce force
//             rb.velocity = bounceDirection * bounceForce;

//             // Start cooldown to prevent multiple triggers
//             StartCoroutine(BounceCooldown());
//         }
//     }

//      IEnumerator BounceCooldown()
//     {
//         recentlyBounced = true;
//         yield return new WaitForSeconds(0.5f); // Adjust cooldown for smoother effect
//         rb.velocity=Vector2.zero;       
//         recentlyBounced = false;
//     }

//     // Learn to dodge logic
//     void LearnToDodge(GameObject ball)
//     {
//         redBallHits++;  // Increase hit count
//         lastBallDirection = (ball.transform.position - transform.position).normalized;

//         Debug.Log("Hit by: " + ball.tag + ". Hits so far: " + redBallHits);

//         // Activate dodge after 2 hits
//         if (redBallHits >= 2 && !canDodge)
//         {
//             canDodge = true;
//             Debug.Log("Villain learned to dodge!");
//         }

//         // Dodge immediately if learned
//         if (canDodge)
//         {
//             DodgeBall(lastBallDirection);
//         }
//     }

//     // Coroutine to destroy ball after delay
//     IEnumerator DestroyBallWithDelay(GameObject ball, float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         Destroy(ball);
//     }

//     // Dodge logic (move left or right)
//     public void DodgeBall(Vector2 ballDirection)
//     {
//         if (canDodge)
//         {
//             Debug.Log("Dodging Ball!");

//             // Randomly decide to dodge left or right
//             Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;

//             // Apply dodge movement perpendicular to ball direction
//             rb.velocity = dodgeDirection * dodgeSpeed;

//             // Stop dodging after a short duration
//             StartCoroutine(StopDodging());
//         }
//     }

//     IEnumerator StopDodging()
//     {
//         yield return new WaitForSeconds(0.5f);  // Dodge for 0.5 seconds
//         rb.velocity = Vector2.zero;  // Stop movement after dodge
//     }
// }
