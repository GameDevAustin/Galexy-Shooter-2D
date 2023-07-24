using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float radius = 5f;
    private Vector3 center;
    private float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        angle += rotationSpeed * Time.deltaTime;
        float newX = center.x + Mathf.Cos(angle) * radius;
        float newY = center.y + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
