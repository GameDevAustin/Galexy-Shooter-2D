using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float _time;
    [SerializeField] private int _duration;
    private bool _stopSpawn = false;
    private bool _missileSpawn = false;
    
   
    void Start()
    {
        
    }
    public void StartSpawining()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
       // StartCoroutine(SpawnMissileRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawn == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-9.45f, 9.45f), 7.4f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, postToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
           
            yield return new WaitForSeconds(5.0f);
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawn == false)
        {
            int missilePowerup = 5;
            _time = Time.time;
            _duration = 10;
            Vector3 postToSpawn = new Vector3(Random.Range(-9.45f, 9.45f), 7.4f, 0);
            int randomPowerUp = Random.Range(0, 5);
           
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
            _missileSpawn = true;
           
            if (_missileSpawn)
            {              
                while (Time.time < _time + _duration)
                {
                    yield return null;
                }

                Instantiate(powerups[missilePowerup], postToSpawn, Quaternion.identity);
            }
        }
           
    }
    IEnumerator SpawnMissileRoutine()
    {
        if (_missileSpawn)
        {
            int missilePowerup = 5;
            _time = Time.time;
            _duration = 10;
            Vector3 postToSpawn = new Vector3(Random.Range(-9.45f, 9.45f), 7.4f, 0);

            while (Time.time < _time + _duration)
            {
                yield return null;
            }

            Instantiate(powerups[missilePowerup], postToSpawn, Quaternion.identity);
        }
    }
}
