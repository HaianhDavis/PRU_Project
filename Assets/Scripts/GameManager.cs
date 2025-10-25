using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    private bool _gameOver = false;
    private bool _gameWin = false;
    void Start()
    {
        UpdateScore();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    public void AddScore(int point)
    {
        if (!_gameOver&& !_gameWin)
        {
            _score += point;
            UpdateScore();  
        }
    }

    private void UpdateScore()
    {
        scoreText.text = _score.ToString();
    }

    public void GameOver()
    {
        _gameOver = true;
        _score = 0;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void GameWin()
    {
        _gameWin = true;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);
    }
    public void RestartGame()
    {
        _gameOver = false;
        _score = 0;
        UpdateScore();
        Time.timeScale = 1;
        SceneManager.LoadScene("MapVang");
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public bool IsGameOver()
    {
        return _gameOver;
    }

    public bool IsGameWin()
    {
        return _gameWin;
    }
}