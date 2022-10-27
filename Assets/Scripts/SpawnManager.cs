using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Variables
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] powerups;

    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawn = false;

    void Start()
    {

    }
    public void StartSpawining()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
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
            Vector3 postToSpawn = new Vector3(Random.Range(-9.45f, 9.45f), 7.4f, 0);
            int randomPowerUp = Random.Range(0, 4);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }
}
