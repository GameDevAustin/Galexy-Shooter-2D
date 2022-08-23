using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
        //speed variable of 8
        [SerializeField]
        private float _speed = 8.0f 



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // translate (move) laserObject up when space bar is pressed
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
} 
