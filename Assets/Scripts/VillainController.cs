using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillainController : MonoBehaviour
{
    private Perceptron perceptron;
    private Rigidbody2D rb;
    public float dodgeSpeed = 10f;
    private bool canDodge = false;
    private int redBallHits = 0;  
    private float lastBallSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        perceptron = new Perceptron(4); // 4 inputs: ball type, distance, speed, last dodge
    }

    // Reset perceptron after tag change
    public void ResetPerceptron()
    {
        perceptron = new Perceptron(4); 
        redBallHits = 0;
        canDodge = false;
        Debug.Log("Perceptron reset after tag change.");
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        string ballType = collision.gameObject.tag;
        float ballSpeed = lastBallSpeed;  

        if (ballType == "Damage")
        {
            Debug.Log("Hit by: " + collision.gameObject.name);
            
            
            redBallHits++;

            
            if (redBallHits >= 5)
            {
                canDodge = true;
                Debug.Log("Villain learned to dodge after 2 hits!");
            }

            
            TrainPerceptron(collision.gameObject.transform.position, ballType, ballSpeed, true);

            Destroy(collision.gameObject, 1.0f); 
        }
        else if (ballType == "NoDamage")
        {
            Debug.Log("Hit by: " + collision.gameObject.name + " (No damage, no dodge)");
            Destroy(collision.gameObject, 1.0f);
        }
    }

    
    public void DodgeBall(Vector2 ballDirection, string ballType, float ballSpeed, bool lastDodgeSuccess)
    {
        lastBallSpeed = ballSpeed; 

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

}











// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class VillainController : MonoBehaviour
// {
//     private Perceptron perceptron;
//     private Rigidbody2D rb;
//     public float dodgeSpeed = 10f;
//     private float lastBallSpeed = 0f;
    
//     
//     private int totalPredictions = 0;
//     private int correctPredictions = 0;
//     private float confidenceThreshold = 0.5f;  

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         perceptron = new Perceptron(4); 
//     }

//     
//     public void ResetPerceptron()
//     {
//         perceptron = new Perceptron(4); 
//         totalPredictions = 0;
//         correctPredictions = 0;
//         confidenceThreshold = 0.6f;
//         Debug.Log("Perceptron reset after tag change.");
//     }

//     
//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         string ballType = collision.gameObject.tag;
//         float ballSpeed = lastBallSpeed;

//         if (ballType == "Damage")
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name);
            
//             bool wasDodgeSuccessful = DodgeBall(collision.gameObject.transform.position, ballType, ballSpeed, false);
            
//             
//             TrainPerceptron(collision.gameObject.transform.position, ballType, ballSpeed, wasDodgeSuccessful);

//             Destroy(collision.gameObject, 1.0f);
//         }
//         else if (ballType == "NoDamage")
//         {
//             Debug.Log("Hit by: " + collision.gameObject.name + " (No damage, no dodge)");
//             Destroy(collision.gameObject, 1.0f);
//         }
//     }

//     
//     public bool DodgeBall(Vector2 ballDirection, string ballType, float ballSpeed, bool lastDodgeSuccess)
//     {
//         lastBallSpeed = ballSpeed;

//         float ballTypeInput = (ballType == "Damage") ? 1f : 0f;
//         float ballDistance = Vector2.Distance(transform.position, ballDirection);
//         float normalizedDistance = Mathf.Clamp01(ballDistance / 10f);
//         float normalizedSpeed = Mathf.Clamp01(ballSpeed / 15f);
//         float dodgeSuccess = lastDodgeSuccess ? 1f : 0f;

//         float[] inputs = { ballTypeInput, normalizedDistance, normalizedSpeed, dodgeSuccess };
//         int decision = perceptron.Predict(inputs);

//         Debug.Log($"Inputs: Type={ballTypeInput}, Distance={normalizedDistance}, Speed={normalizedSpeed}, LastDodge={dodgeSuccess}");
//         Debug.Log($"Perceptron Decision: {decision}");

//         
//         if (ballType == "Damage" && decision == 1 && GetAccuracy() >= confidenceThreshold)
//         {
//             Vector2 dodgeDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;
//             rb.velocity = dodgeDirection * dodgeSpeed;
//             Debug.Log("Dodge Successful: " + dodgeDirection);

//             correctPredictions++;  // Track successful dodge
//             return true;
//         }
//         else
//         {
//             Debug.Log("No dodge executed. Ball type: " + ballType);
//             return false;
//         }
//     }

//     
//     public void TrainPerceptron(Vector2 ballDirection, string ballType, float ballSpeed, bool success)
//     {
//         float ballTypeInput = (ballType == "Damage") ? 1f : 0f;
//         float ballDistance = Vector2.Distance(transform.position, ballDirection);
//         float normalizedDistance = Mathf.Clamp01(ballDistance / 10f);
//         float normalizedSpeed = Mathf.Clamp01(ballSpeed / 15f);
//         float dodgeSuccess = success ? 1f : 0f;

//         float[] inputs = { ballTypeInput, normalizedDistance, normalizedSpeed, dodgeSuccess };
//         int targetOutput = success ? 1 : 0;
//         perceptron.Train(inputs, targetOutput);

//         
//         totalPredictions++;
//         if (success)
//         {
//             correctPredictions++;
//         }

//         UpdateConfidence();
//     }

//    
//     private float GetAccuracy()
//     {
//         if (totalPredictions == 0) return 0f;
//         return (float)correctPredictions / totalPredictions;
//     }

//     
//     private void UpdateConfidence()
//     {
//         float accuracy = GetAccuracy();
//         if (accuracy > confidenceThreshold)
//         {
//             confidenceThreshold = Mathf.Clamp(accuracy + 0.05f, 0.6f, 0.9f); // Slowly increase confidence threshold up to 90%
//             Debug.Log($"Confidence Threshold Increased: {confidenceThreshold * 100}%");
//         }
//     }
// }


