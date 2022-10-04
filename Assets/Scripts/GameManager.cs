using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;

    private void Update()
    {
      
            if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
            {
                SceneManager.LoadScene(0); // current game scene
            }
        
    }
    public void GameOver()
    {
        Debug.Log("GameManager::GameOVer() Called");
        _isGameOver = true;
    }
}