using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
    private float _zAngle;
 
    [SerializeField] private float _rotateSpeed = 3.0f;
    [SerializeField] private GameObject _explosionPrefab;
   // [SerializeField] private float _timeDelay = 2.38f;
    
  

    // Start is called before the first frame update
    void Start()
    {
        //_explosion = GameObject.Find("Explosion").GetComponent<Explosion>();
       
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate on Zed axis

        transform.Rotate(Vector3.forward* _rotateSpeed * Time.deltaTime);
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
               
      }
        
       



    }
}
