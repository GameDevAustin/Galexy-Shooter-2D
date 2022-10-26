using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //variables
    [SerializeField] private float _speed = 5.0f;             //using SerializeField attribute with private variables allows them to be seen in the Inspector
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private GameObject _laserPrefab;                    // use underscore to denote private variables                                                                        
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject playerShield;
    [SerializeField] private Vector3 _laserOffset = new Vector3(0, 1.0f, 0);
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _nextFire = 0; 
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    [SerializeField] private GameObject _leftEngineFire;
    [SerializeField] private GameObject _rightEngineFire;
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private int _shieldLives = 3;
    [SerializeField] private int _ammoCount = 30;
    [SerializeField] private AudioClip _noAmmo;
    //[SerializeField] private int _powerupTime = 5;

    [SerializeField] private float _thrustSpeed = 3;
    [SerializeField] private float _timeStamp;
    private float _duration;
    private bool _shield;

    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    private SpriteRenderer _shieldColor; 
    private bool _isTripleShotActive = false;   
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    
    void Start()
    {
     
        _shieldColor = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _leftEngineFire.gameObject.SetActive(false);
        _rightEngineFire.gameObject.SetActive(false);
       _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

       if (_uiManager == null)
       {
        Debug.LogError("UI Manager is null");
       }

        transform.position = new Vector3(0, 0, 0);                                                                    // take the current position and assign it a start position =new position (0,0,0)
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();                                // find gameObject then get component.
        if(_spawnManager == null)
        {
           Debug.LogError("The Spawn Manager is NULL");
        }
        

        if(_audioSource == null)
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
      if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)                                                   // checks if time is greater than nextFire. calls FireLaser() if true.
      {
            if(_ammoCount <=0)
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
                                                                                                                       // combines Horizontaland Vertical translation to one line
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);        // use clamping function in place of if statements to set boundries where possible
       
        if(transform.position.x > 11.3f)                                                                               // Horizontal postion wrapping
        {
          transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x < -11.3f)
        {
          transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _speed + _thrustSpeed;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
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
    private void AmmoCount(int bullets)
    {
        _ammoCount += bullets;
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    void FireLaser()
    {
        AmmoCount(-1);
        _nextFire = Time.time + _fireRate;
         
       if (_isTripleShotActive == true)
       {
          Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
       }
       else
       {
         Instantiate(_laserPrefab, transform.position+_laserOffset, Quaternion.identity);
       }
     _audioSource.Play();
    }
    public void Damage()
    {   
        if(_isShieldActive == true)
        {
            

             if(_shieldLives == 3)
            {
                _shieldColor.color = Color.yellow;
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
                playerShield.SetActive(false);
               
                
                return;
            }
           
        }


       _lives -= 1;

        //if lives is 2 enable right engine
        //if lives is one enable left engine
        
        if(_lives == 2)
        {
            _rightEngineFire.gameObject.SetActive(true);
        }
        else if(_lives == 1)
        {
            _leftEngineFire.gameObject.SetActive(true);
        }

       _uiManager.UpdateLives(_lives);
       if (_lives == 0)
       {
         _spawnManager.OnPlayerDeath();
        // _audioSource.PlayOneShot(_playerExplode);
         Destroy(this.gameObject);
           

       }
    }
    
    public void TripleShotActive()
    {
        //_audioSource.PlayOneShot(_powerupSound);
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
        //_audioSource.PlayOneShot(_powerupSound);
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    { 
           
          yield return new WaitForSeconds(5.0f);
          _isSpeedBoostActive = false;  
         // _speed /= _speedMultiplier;
    }
   
    
    public void ShieldActive()
    {
        //_audioSource.PlayOneShot(_powerupSound);
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
            playerShield.SetActive(true);

            while (Time.time < _timeStamp + _duration)
            {
                if (_isShieldActive == false)
                {
                    _isShieldActive = true;
                    _timeStamp = Time.time;
                }
                yield return null;

            }
                _isShieldActive = false;
                playerShield.SetActive(false);
            
        }
        _shield = false;
    }
    
    //method to add 10 to score
    // communicate with the UI to Update the score.
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
