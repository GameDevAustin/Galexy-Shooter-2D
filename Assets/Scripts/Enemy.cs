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
         //Instantiate(this.gameObject, position, Quaternion.identity);  
    }
    
  
        
    


    // Update is called once per frame
    void Update()
    {  SpawnEnemy();
        //move Enemy down 4m per sec. 
         transform.Translate (Vector3.down * _speed * Time.deltaTime);
        //if bottom of screen 
        // respawn at top

        if(transform.position.y < -5.4f)
        {   
            transform.position = new Vector3(Random.Range(-11.3f, 11.3f), 7.4f, 0);


        }
        //respawn at top with new random x position.
        // vertical screen limits -5.4 to 7.4 y-axis
         
       

    }

    void SpawnEnemy()
    {   
        //variables
                 
                

       

    }
}
