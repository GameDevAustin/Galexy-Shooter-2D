using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngledEntryMovement : MonoBehaviour
{
    public float speed = 5f;
    public float entryAngle = 45f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        transform.Translate(Vector3.right * speed * Mathf.Tan(entryAngle * Mathf.Deg2Rad) * Time.deltaTime);
    }
}