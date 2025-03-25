using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoseMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float moveAmount = 0.02f;
    public float delayTime = 0.5f;
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
