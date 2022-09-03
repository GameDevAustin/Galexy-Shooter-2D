using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    //Variables
     private IEnumerator _spawn;
     private IEnumerator _tripleShot;
     [SerializeField]
     private GameObject _enemyPrefab;
     [SerializeField]
     private GameObject _tripleShotPrefab;
     [SerializeField]
     private GameObject _enemyContainer;
     [SerializeField]
     private GameObject _powerUpContainer;

     private bool _stopSpawn = false;
     private bool _stopPowerUp = false;
    // Start is called before the first frame update
    void Start()
    {
        _spawn = SpawnRoutine(5.0f);
        StartCoroutine(_spawn);

        _tripleShot = TripleShotRoutine(5.0f);
        StartCoroutine(_tripleShot);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    //spawn game objects every 5 seconds
    //create coroutine IEnumerater-- Yield Events
    //while loop

    IEnumerator SpawnRoutine(float delay)
    {
      // yield return null; will wait one frame before executing next line
      while (_stopSpawn == false)
      {
       Vector3 spawnPos = new Vector3(Random.Range(-9.45f, 9.45f), 7.4f, 0);
       GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
       newEnemy.transform.parent = _enemyContainer.transform;
       yield return new WaitForSeconds(delay);
      }
    }
    public void OnPlayerDeath()
    {
       _stopSpawn = true;
    }
    IEnumerator TripleShotRoutine(float delay)
    {
     while(_stopPowerUp == false)
      {
        Vector3 powerUpPosition = new Vector3(Random.Range(-9.45f, 9.45f), 7.4f, 0);
        GameObject newPowerUp = Instantiate(_tripleShotPrefab, powerUpPosition, Quaternion.identity);
        newPowerUp.transform.parent = _powerUpContainer.transform;
        yield return new WaitForSeconds(delay);
      }
    }
}
