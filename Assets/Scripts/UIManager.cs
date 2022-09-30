using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //create a handle to Text
  [SerializeField] private TMP_Text _scoreText;
  [SerializeField] private Image _LivesImg;
  [SerializeField] private Sprite[] _liveSprites;
  
   
 

    // Start is called before the first frame update
    void Start()
    {
        //assign text component to handle
         _scoreText.text ="Score: " + 0;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        
        _LivesImg.sprite = _liveSprites[currentLives];
    }
}
