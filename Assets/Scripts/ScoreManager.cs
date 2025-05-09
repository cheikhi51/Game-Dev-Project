using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [SerializeField] private Text scoreText;
    private int currentScore = 0;

    // Public property to access the score
    public int CurrentScore => currentScore;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
        Debug.Log($"Added {points} points. Total: {currentScore}"); // Debug log
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
        else
        {
            Debug.LogError("Score Text reference not assigned in ScoreManager!");
        }
    }

    // New method to get current score
    public int GetCurrentScore()
    {
        return currentScore;
    }
}