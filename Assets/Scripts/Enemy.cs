using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   


    [SerializeField]
    private float _speed = 4.0f;


    // variables
    void Start()
    {
         
    }
    
  
        
    


    // Update is called once per frame
    void Update()
    {  
        transform.Translate (Vector3.down * _speed * Time.deltaTime);
       
        if(transform.position.y < -5.4f)
        {   
            transform.position = new Vector3(Random.Range(-11.3f, 11.3f), 7.4f, 0);
        }
    }

                

       

  
}
