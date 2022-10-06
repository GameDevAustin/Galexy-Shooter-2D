using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField] private float _delay = 2.35f;

    private Animator _anim; //handle to animator component
    
    private Player _player;
    private Collider2D _deadEnemy;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
            
            //assign component to anim
        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("Animator is null");
        }
    }
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
            // trigger animation
        _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
         Destroy(this.gameObject, _delay);
       }  
       //checking for collision from laser 
       if (other.tag == "Laser")
       { 
         Destroy(other.gameObject);
         
         if(_player != null)
         {
              _player.AddScore(10);
         }

            //trigger animation
        _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
         Destroy(this.gameObject, _delay);
            _deadEnemy = GetComponent<BoxCollider2D>();
            _deadEnemy.enabled = false;
       }
    }
}
