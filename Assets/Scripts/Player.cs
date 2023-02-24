using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //variables

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _playerShield;
    [SerializeField] private GameObject _leftEngineFire;
    [SerializeField] private GameObject _rightEngineFire;
    [SerializeField] private GameObject _thruster;
    [SerializeField] private GameObject _missilePrefab;



    [SerializeField] private Vector3 _laserOffset = new Vector3(0, 1.0f, 0);

    [SerializeField] private int _shieldLives = 3;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    [SerializeField] private int _ammoUp = 15;
    [SerializeField] public static int ammoCount;
    [SerializeField] private int _coreTempIncrease;
    [SerializeField] private int _coreTempDecrease;
    public int maxTemp = 1000;
    public int currentTemp = 0;

    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _noAmmo;
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private AudioClip _missileAudio;
    [SerializeField] private AudioClip _criticalWarning;
    [SerializeField] private AudioClip _tempExceeded;
    [SerializeField] private AudioClip _tempNormal;


    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _nextFire = 0;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private float _thrustSpeed = 10;
    [SerializeField] private float _timeStamp;
    private float _delay = 2.35f;
    private float _duration;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _shield;
    public bool canUseThrusters = false;
    public bool tempWanring = false;
    public bool resetTempWarning = false;
    [SerializeField] private bool _coreCoolDown = true;
    [SerializeField] private bool _hasCooledDown = true;
    private bool isAlive;

    //handles
    private Color _orange = new Color(1f, 0.23f, 0f, 1f);
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private SpriteRenderer _shieldColor;
    private Animator _anim;
    public ThrusterBar thrusterBar;
    public ShakeCamera shakeCamera;


    private void Awake()
    {
        isAlive = true;
        ammoCount = 30;

    }
    void Start()
    {
        thrusterBar = GameObject.Find("Canvas").GetComponentInChildren<ThrusterBar>();
        _shieldColor = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _leftEngineFire.gameObject.SetActive(false);
        _rightEngineFire.gameObject.SetActive(false);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _anim = GetComponent<Animator>();
        shakeCamera = GameObject.Find("Main_Camera").GetComponent<ShakeCamera>();
        ammoCount = 30;
        currentTemp = 0;
        thrusterBar.SetDefaultValue(currentTemp);
        canUseThrusters = true;
        isAlive = true;


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
        CalculateThrusterBar();
        ThrusterLogic();

        if (Input.GetKey(KeyCode.Space) && Time.time > _nextFire)                                                   // checks if time is greater than nextFire. calls FireLaser() if true.
        {
            if (ammoCount <= 0)
             {
                 _audioSource.PlayOneShot(_noAmmo, 1f);
                 return;
             }
            FireLaser();
        }
        else if (Input.GetKeyDown(KeyCode.M) && Time.time > _nextFire)
        {
            if (ammoCount <= 0)
            {
                _audioSource.PlayOneShot(_noAmmo, 1f);
                return;
            }
            FireMissile();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontalInput, verticalInput, 0);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);        // use clamping function in place of if statements to set boundries where possible // combines Horizontaland Vertical translation to one line

        if (isAlive)
        {
            if (transform.position.x > 11.3f)                                                                               // Horizontal postion wrapping
            {
                transform.position = new Vector3(-11.3f, transform.position.y, 0);
            }
            else if (transform.position.x < -11.3f)
            {
                transform.position = new Vector3(11.3f, transform.position.y, 0);
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
    }

    void CalculateThrusterBar()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _hasCooledDown && canUseThrusters)
        {
            ThrustersOn(5);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ThrustersOff();
        }
    }
    void ThrusterLogic()
    {
        if (currentTemp > 0 && _coreCoolDown == true && canUseThrusters == true)
        {
            currentTemp -= _coreTempDecrease;
            thrusterBar.SetDefaultValue(currentTemp);
        }
        if (currentTemp > 6000 && _coreCoolDown == true && canUseThrusters == true)
        {
            _uiManager.CriticalWarning(true);
            if (resetTempWarning == false)
            {
                resetTempWarning = true;
                StartCoroutine(PlayCriticalWarning());
            }
        }
        else
        {
            _uiManager.CriticalWarning(false);
        }
        if (currentTemp >= 9999 && _coreCoolDown == true && canUseThrusters == true)
        {
            StartCoroutine(PlayEngineShutdown());
            _coreCoolDown = false;
            canUseThrusters = false;

            //EngineShutdownDrifting();
            _uiManager.EngineShutdown(true);

        }
        if (currentTemp > 2500 && _coreCoolDown == false)
        {
            currentTemp -= _coreTempDecrease;
            thrusterBar.SetDefaultValue(currentTemp);

            //EngineShutdownDrifting();
        }
        if (currentTemp == 2500 && _coreCoolDown == false && canUseThrusters == false)
        {
            _coreCoolDown = true;
            canUseThrusters = true;
            _uiManager.EngineShutdown(false);
            currentTemp -= _coreTempDecrease;
            thrusterBar.SetDefaultValue(currentTemp);

            // _playerThrusterLeft.SetAcitve(true); Future Implementation. adding multiple thruster animations for speed boost
            // _playerThrusterRight.SetActive(true);

            _speed = 5.0f;
            //_uiManager.EngineStable(true);

            /*if (transform.rotation.z != 0)
            {
                StartCoroutine(RotateForward(this.transform, Quaternion.identity, 1f));
            }*/
        }

    }
    void EngineShutdownDrifting()                         //Future Implementation. method for making player Shutdown when engines overheat. 
    {
        //_playerThrusterLeft.GameObject.SetActive(false); Future Implementation. adding multiple thruster animations for speed boost
        //_playerThrusterRight.GameObject.SetActive(false);
        _nextFire = Time.time + 10f;
        _speed = 0.25f;
        transform.Rotate(Vector3.forward * -50f * Time.deltaTime);
    }
    void ThrustersOn(int tempIncrease)
    {
        currentTemp += tempIncrease;
        thrusterBar.SetDefaultValue(currentTemp);
        if (currentTemp > maxTemp)
        {
            currentTemp = maxTemp;
        }
        _speed = _thrustSpeed;
    }
    IEnumerator PlayCriticalWarning()
    {
        // _audioSource.PlayOneShot(_criticalWarning);
        yield return new WaitForSeconds(3.0f);
        resetTempWarning = false;
    }
    IEnumerator PlayEngineShutdown()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_tempExceeded);
        yield return new WaitForSeconds(5.0f);
    }
    IEnumerator RotateForward(Transform target, Quaternion rot, float dur)      //Future Implementation. method for making player spin out of control when engines overheat.
    {
        //PlayClip(_tempNormal); Future Implementation. adding sound effects for engine temp stages.

        float t = 0f;
        Quaternion start = target.rotation;
        while (t < dur)
        {
            target.rotation = Quaternion.Slerp(start, rot, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        target.rotation = rot;
        _nextFire = Time.time;
        _uiManager.EngineStable(false);
    }

    void ThrustersOff()
    {
        _speed = 5.0f;
        if (_isSpeedBoostActive)
        {
            _speed *= _thrustSpeed;
        }
    }
    private void AmmoCount()
    {
        --ammoCount;
        _uiManager.UpdateAmmoCount(ammoCount);
    }
    void FireLaser()
    {
        if (isAlive)
        {
            AmmoCount();

            _nextFire = Time.time + _fireRate;

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _audioSource.Play();

            }
            else
            {
                Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
                _audioSource.Play();
            }
        }
    }
    void FireMissile()
    {
        if (isAlive)
        {

            AmmoCount();
            _nextFire = Time.time + _fireRate;
            Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_missileAudio);
        }
    }
    public void Health()
    {
        if (_lives == 3)
        {
            return;
        }
        else
        {
            _lives++;
            _uiManager.UpdateLives(_lives);

            if (_lives == 2)
            {
                _leftEngineFire.gameObject.SetActive(false);
            }
            else if (_lives == 3)
            {
                _rightEngineFire.gameObject.SetActive(false);
            }
        }
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
            StartCoroutine(shakeCamera.StartShake());
            _audioSource.PlayOneShot(_explosion);
        }
        else if (_lives == 1)
        {
            _leftEngineFire.gameObject.SetActive(true);
            StartCoroutine(shakeCamera.StartShake());
            _audioSource.PlayOneShot(_explosion);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives == 0)
        {
            isAlive = false;
            _anim.SetTrigger("OnPlayerDeath");
            _audioSource.PlayOneShot(_explosion);
            StartCoroutine(shakeCamera.StartShake());
            _thruster.SetActive(false);
            _leftEngineFire.SetActive(false);
            _rightEngineFire.SetActive(false);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject, _delay);


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
        if (isAlive)
        { 
            _shield = true;
            _shieldLives = 3;
            _shieldColor.color = Color.white;
            StartCoroutine(ShieldPowerOffRoutine());
        }
    }
    IEnumerator ShieldPowerOffRoutine()
    {
        _timeStamp = Time.time;
        _duration = 10f;

        if (_shield)
        {
            _isShieldActive = true;
            _playerShield.SetActive(true);

            while (Time.time < _timeStamp + _duration)
            {
                yield return null;
            }

            _isShieldActive = false;
            _playerShield.SetActive(false);
        }
        _shield = false;

    }
   
    public void AmmoUp()
    {if (isAlive)
        {
            ammoCount += _ammoUp;
            if (ammoCount >= 99)
            {
                ammoCount = 99;
                _uiManager.UpdateAmmoCount(ammoCount);
            }
            else
            {
                _uiManager.UpdateAmmoCount(ammoCount);
            }
        }
    }
    public void Nuke()
    {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var enemy in enemies)
            {
                Enemy target = enemy.GetComponent<Enemy>();
                target.Destroy();
            }
        
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }


}