using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    //Variables
     private IEnumerator _spawn;
     [SerializeField]
     private GameObject _enemyPrefab;

     
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
      //while loop (must be infinite loop)
      // Insttantiate enemy prefab
      //yield wait for five seconds

      // wait one frame before executing next line
      yield return null;

      

      while (true)
      {
       Vector3 spawnPos = new Vector3(Random.Range(-9.45f, 9.45f), 7.4f, 0);

       Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
       yield return new WaitForSeconds(delay);
      }
    }
}
