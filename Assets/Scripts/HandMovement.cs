using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    public float rotationSpeed = 7f;   
    public float rotationAmount = 7f;  
    public float delayTime = 0.3f;      
    private bool isDelayed = true;

    void Start()
    {
        StartCoroutine(StartWithDelay());
    }

    void Update()
    {
        if (!isDelayed)
        {
            float rotationAngle = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
            transform.localRotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }

    IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelayed = false;
    }
}
