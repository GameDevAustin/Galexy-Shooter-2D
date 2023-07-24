using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSideMovement : MonoBehaviour
{

    public float speed = 4f;
    public float movementRange = 5f;
    private Vector3 startPosition;
    private float direction = 1f; // 1f for moving right, -1f for moving left


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newPositionX = startPosition.x + direction * Mathf.PingPong(Time.time * speed, movementRange);
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
    }
}
