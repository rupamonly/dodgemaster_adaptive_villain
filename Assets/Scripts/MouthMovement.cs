using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthMovement : MonoBehaviour
{
    public float moveSpeed = 10f;   // Slightly different speed for variation
    public float moveAmount = 0.08f; // Slightly more movement for the mouth
    public float delayTime = 1f;     // Delay for the mouth
    private Vector3 startPos;
    private bool isDelayed = true;

    void Start()
    {
        startPos = transform.localPosition;
        StartCoroutine(StartWithDelay());
    }

    void Update()
    {
        if (!isDelayed)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveAmount;
            transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
    }

    IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(delayTime); // Delay before starting animation
        isDelayed = false;
    }
}
