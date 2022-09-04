using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private float _speed = 4.0f;

    void Update()
    {  
        transform.Translate (Vector3.down * _speed * Time.deltaTime);
       
        if(transform.position.y < -5.4f)
        {   
           transform.position = new Vector3(Random.Range(-9.45f, 9.45f), 7.4f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)          
    {
       //checking for collision from player
       //null checking
       if (other.tag == "Player")
       {
         Player player = other.transform.GetComponent<Player>();
         if (player != null)
         {
           player.Damage();
         }
         Destroy(this.gameObject);
       }  
       //checking for collision from laser 
       if (other.tag == "Laser")
       { 
         Destroy(other.gameObject);
         Destroy(this.gameObject);
       }
    }
}
