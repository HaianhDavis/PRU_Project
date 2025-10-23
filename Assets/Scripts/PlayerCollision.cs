 using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            _audioManager.PlayCoinSound();
            _gameManager.AddScore(100);
        }
        else if (collision.CompareTag("Trap"))
        {
            _gameManager.GameOver();
        }
        else if (collision.CompareTag("Enemy"))
        {
            _gameManager.GameOver();
        }
        else if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            _gameManager.GameWin();
        }
    }
}