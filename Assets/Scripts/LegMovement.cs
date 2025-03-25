using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
{
    public float moveSpeed = 0.4f;       // Speed of movement
    public float moveAmount = 0.05f;     // Slight movement amount
    public float delayTime = 0.7f;       // Delay before starting
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
        yield return new WaitForSeconds(delayTime);
        isDelayed = false;
    }
}
