using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
   //speed variable 
   [SerializeField]
   private float _speed = 10.0f;


    void Update()
   {
     transform.Translate(Vector3.up * _speed * Time.deltaTime); 
        // destroy objects when they leave playable area
     if (transform.position.y > 8.0f)
     {
         if(transform.parent != null)
         {
            Destroy(transform.parent.gameObject);
         }
         Destroy(this.gameObject);
     }
   }
} 
