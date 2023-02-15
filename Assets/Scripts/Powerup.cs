using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    //Variables
    [SerializeField] private float _speed = 3.0f;

    [SerializeField] private int _powerupID; // 0 = Triple Shot, 1 = Shield, 2 = Ammo, 3 = Health, 4 = Nuke

    [SerializeField] private AudioClip _clip;

    void Start()
    {

    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.4f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            Player player = other.transform.GetComponent<Player>();
            Enemy enemy = transform.GetComponent<Enemy>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.ShieldActive();
                        break;
                    case 2:
                        player.AmmoUp();
                        break;
                    case 3:
                        player.Health();
                        break;
                    case 4:
                        player.Nuke();
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
