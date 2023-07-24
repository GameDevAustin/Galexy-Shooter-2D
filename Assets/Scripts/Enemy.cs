using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;

    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _delay = 2.35f;
    [SerializeField] private float _fireRate = 3f;
    [SerializeField] private float _canFire = -1;
 

    public bool enemyDeath = false;
    [HideInInspector] public bool isTargeted;
    [HideInInspector] public bool isDestroyed; 


    private Player _player;
    private Collider2D _deadEnemy;
    private Animator _anim;
    private AudioSource _audioSource;
    private enum MovementType { SideToSide, Circular, AngledEntry }
    private MovementType _selectedMovement;


    void Start()
    {

        _deadEnemy = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();



        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if (_anim == null)
        {
            Debug.LogError("Animator is null");
        }

        _selectedMovement = (MovementType)Random.Range(0, System.Enum.GetValues(typeof(MovementType)).Length);

        switch (_selectedMovement)
        {
            case MovementType.SideToSide:
                GetComponent<SideToSideMovement>().enabled = true;
                break;

            case MovementType.Circular:
                GetComponent<CircularMovement>().enabled = true;
                break;

            case MovementType.AngledEntry:
                GetComponent<AngledEntryMovement>().enabled = true;
                break;

        }
    }
    void Update()
    {
        
        CalculateMovement();
        if (enemyDeath != true)
        {
            
            FireEnemeyLaser();
        }
        
    }
    void FireEnemeyLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            
        }
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.4f)
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
            _audioSource.Play();
            enemyDeath = true;
            _deadEnemy.enabled = false;
            isDestroyed = true;
            Destroy(this.gameObject, _delay);
        }
        //checking for collision from laser 
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _deadEnemy.enabled = false;
            _audioSource.Play();
            enemyDeath = true;
            isDestroyed = true;
            Destroy(this.gameObject, _delay);
        }
        if (other.tag == "enemyLaser")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.GetComponent<Collider2D>());
        }
        if(other.tag == "Missile")
        {
            Destroy(other.gameObject);
           
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _deadEnemy.enabled = false;
            _audioSource.Play();
            enemyDeath = true;
            isDestroyed = true;
            Destroy(this.gameObject, _delay);
            
        }
    }
    public void Destroy()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _deadEnemy.enabled = false;
        _audioSource.Play();
        enemyDeath = true;
        isDestroyed = true;
        Destroy(this.gameObject, _delay);
    }
}
