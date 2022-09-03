using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    //Variables
    [SerializeField]
    private float _speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move down at a speed of 3
        //when we leave the screen, destroy this object
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.4f)
        {
            Destroy(this.gameObject);
        }

        //check for collisions
        // only collectable by player. hint use tags.
        //on collected, destroy
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
          Player player = other.transform.GetComponent<Player>();
          if (player != null)
          {
            player.Powerup();
          }
          Destroy(this.gameObject);
        }
    }
}
