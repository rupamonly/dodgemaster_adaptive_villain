using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveAmount = 0.1f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
    
        float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
