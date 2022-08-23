using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Public or private reference
    //data type (int, float, bool, string)
    //every variable has a name
    //optional value assigned
        
    [SerializeField]                    //using SerializeField attribute with private variables
    private float _speed = 10.0f;       //allows them to be seen in the Inspector
    
    [SerializeField]
    private GameObject _laserPrefab;    // use underscore to denote private variables
    



    // Start is called before the first frame update
    void Start()
    {
        // take the current position and assign it a start position =new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update() 
    {
      CalculateMovement();

      //if space bar pressed 
      //then spawn game object

      if(Input.GetKeyDown(KeyCode.Space))
      {
        Instantiate(_laserPrefab, transform.position, Quaternion.identity);                                                                //Debug.Log("Space Key Pressed");
      }
    }   

    void CalculateMovement()
    {
    
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontalInput, verticalInput, 0);
    
        transform.Translate(move *_speed * Time.deltaTime);  // combines Horizontaland Vertical translation to one line

        // Vertical position limits
      
         // use clamping function in place of if statements to set boundries where possible
       
         transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        // Horizontal postion wrapping
        
         if(transform.position.x > 11.3f)
         {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
         }
         else if(transform.position.x < -11.3f)
         {
         transform.position = new Vector3(11.3f, transform.position.y, 0);
    }
}
    
        
}
