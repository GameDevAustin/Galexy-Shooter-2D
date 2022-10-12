using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
     
    [SerializeField] private float _rotateSpeed = 3.0f;
    [SerializeField] private GameObject _explosionPrefab;
  

   
    private SpawnManager _spawnManager;
    
  

    // Start is called before the first frame update
    void Start()
    {
       
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate on Zed axis

        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    //check for LASER collision (trigger)
    //Instantiate explosion at position of the asteroid.

    private void OnTriggerEnter2D(Collider2D other)
    {
       
      if (other.tag == "Laser")
      {
              
       Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
       Destroy(other.gameObject);
       Destroy(this.gameObject, 0.25f);
       
       _spawnManager.StartSpawining();
      }

    }
}
