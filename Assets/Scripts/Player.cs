using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //variables
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private GameObject _laserPrefab;                                                                                        
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _playerShield;
    [SerializeField] private GameObject _leftEngineFire;
    [SerializeField] private GameObject _rightEngineFire;

    [SerializeField] private Vector3 _laserOffset = new Vector3(0, 1.0f, 0);

    [SerializeField] private int _shieldLives = 3;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    [SerializeField] private int _ammoUp = 15;
    public static int ammoCount;

    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _noAmmo;


    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _nextFire = 0;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private float _thrustSpeed = 3;
    [SerializeField] private float _timeStamp;
    private float _duration;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _shield;
    

    //handles
    private Color _orange = new Color(1f, 0.23f, 0f, 1f);
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private SpriteRenderer _shieldColor;
   
    private void Awake()
    {
        ammoCount = 15;
        
    }
    void Start()
    {
        _shieldColor = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _leftEngineFire.gameObject.SetActive(false);
        _rightEngineFire.gameObject.SetActive(false);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        ammoCount = 16;

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }

        transform.position = new Vector3(0, 0, 0);                                                                    // take the current position and assign it a start position =new position (0,0,0)
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();                                // find gameObject then get component.
       
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)                                                   // checks if time is greater than nextFire. calls FireLaser() if true.
        {
            if (ammoCount <= 0)
            {
                _audioSource.PlayOneShot(_noAmmo, 1f);
                return;
            }
            FireLaser();
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontalInput, verticalInput, 0);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);        // use clamping function in place of if statements to set boundries where possible // combines Horizontaland Vertical translation to one line

        if (transform.position.x > 11.3f)                                                                               // Horizontal postion wrapping
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _speed + _thrustSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _speed - _thrustSpeed;
        }
        if (_isSpeedBoostActive == false)
        {
            transform.Translate(move * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(move * _speed * _speedMultiplier * Time.deltaTime);
        }



    }
    private void AmmoCount()
    {
       -- ammoCount;
        _uiManager.UpdateAmmoCount(ammoCount);
    }
    void FireLaser()
    {
        AmmoCount();
       
        _nextFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        }
        _audioSource.Play();
    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            if (_shieldLives == 3)
            {
                _shieldColor.color = _orange;
                _shieldLives -= 1;
                return;
            }
            else if (_shieldLives == 2)
            {
                _shieldColor.color = Color.red;
                _shieldLives -= 1;
                return;
            }
            else if (_shieldLives <= 1)
            {
                _isShieldActive = false;
                _playerShield.SetActive(false);
                _shield = false;
                return;
            }
        }

        _lives -= 1;
                
        if (_lives == 2)
        {
            _rightEngineFire.gameObject.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngineFire.gameObject.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);


        }
    }
    public void TripleShotActive()
    {
       
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedBoostActive()
    {
       
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        
    }
    public void ShieldActive()
    {
        _shield = true;
        _shieldLives = 3;
        _shieldColor.color = Color.white;
        StartCoroutine(ShieldPowerOffRoutine());
    }
    IEnumerator ShieldPowerOffRoutine()
    {
        _timeStamp = Time.time;
        _duration = 10f;

        if (_shield)
        {
            _isShieldActive = true;
            _playerShield.SetActive(true);

            while (Time.time < _timeStamp + _duration )
            {          
                yield return null;
            }

            _isShieldActive = false;
            _playerShield.SetActive(false);
        }
        _shield = false;
    }
    public void AmmoUp()
    {
        ammoCount += _ammoUp;
        _uiManager.UpdateAmmoCount(ammoCount);
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
