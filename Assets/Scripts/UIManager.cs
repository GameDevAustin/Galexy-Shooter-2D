using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle 
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _LivesImg;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartLevelText;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private TMP_Text _ammoText;
    private GameManager _gameManager;
 
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _ammoText.text = Player.ammoCount.ToString();

        if (_gameManager == null)
        {
            Debug.LogError("Game_Manager is NULL");
        }
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }
    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }
    public void UpdateAmmoCount(int playerAmmo)
    {
        if (playerAmmo > 0)
        {
           _ammoText.text = playerAmmo.ToString();
        }
        else if (playerAmmo == 0)
        {
            _ammoText.text = "00";
        }
    }
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
