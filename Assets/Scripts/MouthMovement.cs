using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthMovement : MonoBehaviour
{
    public float moveSpeed = 10f;   
    public float moveAmount = 0.08f; 
    public float delayTime = 1f;     
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
