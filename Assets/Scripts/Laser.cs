using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
   //speed variable 
        [SerializeField]
        private float _speed = 10.0f;
        [SerializeField]
        
      




    // Update is called once per frame
    void Update()
    {
        
        // translate (move) laserObject up when space bar is pressed
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // if laser position is greater than 8 on the y axis 
        // destroy object

        if (transform.position.y > 8.0f)
        {
            Destroy(this.gameObject);
        }

    }
} 
