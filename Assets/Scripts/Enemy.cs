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

    private bool _enemyDeath = false;

    private Player _player;
    private Collider2D _deadEnemy;
    private Animator _anim;
    private AudioSource _audioSource;

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
    }
    void Update()
    {
        CalculateMovement();
        if (_enemyDeath != true)
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
            // Debug.Break();
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
            _enemyDeath = true;
            _deadEnemy.enabled = false;
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
            _enemyDeath = true;
            Destroy(this.gameObject, _delay);
        }
        if (other.tag == "enemyLaser")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.GetComponent<Collider2D>());
        }
    }
}
