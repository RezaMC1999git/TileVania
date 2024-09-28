using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] int score = 0;
    [SerializeField] Text ScoreText;
    [SerializeField] Text HealthText;
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        HealthText.text = health.ToString();
        ScoreText.text = score.ToString();
    }

    public void AddToScore(int amount) 
    {
        score += amount;
        ScoreText.text = score.ToString();
    }
    public void ProcessPlayerDeath() 
    {
        if (health > 1)
            TakeLife();
        else
            ResetGameSession();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        health --;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        HealthText.text = health.ToString();
    }
}
