using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    //Variables
     private IEnumerator _spawn;
     [SerializeField]
     private GameObject _enemyPrefab;
     [SerializeField]
     private GameObject _enemyContainer;
     private bool _stopSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawn = SpawnRoutine(5.0f);
        StartCoroutine(_spawn);
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
}
