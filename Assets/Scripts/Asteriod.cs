using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.0f;
    [SerializeField] private GameObject _explosionPrefab;
    public ShakeCamera shakeCamera;

    private SpawnManager _spawnManager;

    void Start()
    {
        shakeCamera = GameObject.Find("Main_Camera").GetComponent<ShakeCamera>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
    }
    void Update()
    {
        // Rotate on Zed axis
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            StartCoroutine(shakeCamera.StartShake());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);

            _spawnManager.StartSpawining();
        }
        else if(other.tag == "Missile")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            StartCoroutine(shakeCamera.StartShake());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);

            _spawnManager.StartSpawining();
        }
    }
}