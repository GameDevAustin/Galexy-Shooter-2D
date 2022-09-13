using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    //Variables
    [SerializeField]
    private float _speed = 3.0f;
    // ID for Powerups
    // 0 = Triple shot
    // 1 = speed 
    // 2 = shield
    [SerializeField]// 0 = Triple Shot, 1 = Speed, 2 = Shield
    private int _powerupID;


    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.4f)
        {
          Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
          Player player = other.transform.GetComponent<Player>();
          if (player != null)
          {       
            switch(_powerupID)
            {
                case 0:
                    player.TripleShotActive();
                    break;
                case 1:
                    player.SpeedBoostActive();
                    break;
                case 2:
                    Debug.Log("Shield Active");
                    player.ShieldActive();
                    break;
                default:
                    Debug.Log("Default Value");
                    break;
            }
          }
          Destroy(this.gameObject);
        }
    }
}
