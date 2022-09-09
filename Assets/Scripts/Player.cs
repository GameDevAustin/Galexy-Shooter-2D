using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //variables
    [SerializeField]                                                                                                 //using SerializeField attribute with private variables allows them to be seen in the Inspector
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;                                                                                 // use underscore to denote private variables
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _playerShieldPrefab;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, 1.0f, 0);
    [SerializeField] 
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _nextFire = 0; 
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager; 
    [SerializeField]
    private bool _isTripleShotActive = false;   
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;


    void Start()
    {
       
        transform.position = new Vector3(0, 0, 0);                                                                    // take the current position and assign it a start position =new position (0,0,0)
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();                                // find gameObject then get component.
        if(_spawnManager == null)
        {
           Debug.LogError("The Spawn Manager is NULL");
        }
    }

    void Update() 
    {
      CalculateMovement();
      if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)                                                   // checks if time is greater than nextFire. calls FireLaser() if true.
      {
        FireLaser();
      } 
      //ShieldOn();
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
        
        
        transform.Translate(move *_speed * Time.deltaTime);  
       
       
    }

    void FireLaser()
    {
       _nextFire = Time.time + _fireRate;
         
       if (_isTripleShotActive == true)
       {
          Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
       }
       else
       {
         Instantiate(_laserPrefab, transform.position+_laserOffset, Quaternion.identity);
       }
    }
    
    public void Damage()
    {   
       _lives -= 1;
        
       if (_lives < 1)
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
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    { 
           
          yield return new WaitForSeconds(5.0f);
          _isSpeedBoostActive = false;  
          _speed /= _speedMultiplier;
    }
   
    /*
    public void ShieldActive()
    {
        _isShieldActive = true;     
        StartCoroutine(ShieldPowerOffRoutine());
    }
        
    IEnumerator ShieldPowerOffRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isShieldActive = false;
        
    } 

    void ShieldOn()
    {
        if(_isShieldActive == true)
        {   
          Instantiate(_playerShieldPrefab, transform.position, Quaternion.identity);
        }
        else
        {
          Destroy(_playerShieldPrefab);
        }         
    }
    */
}
