using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text finalScoreText;
     [SerializeField] private Text totalPenaltiesText;
     [SerializeField] private Text penaltyText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Game Settings")]
    [SerializeField] private float initialTime = 30f;
    [SerializeField] private float timePenalty = 1f;
    [SerializeField] private float gameOverDelay = 0.5f;

    private float timeRemaining;
    private bool timerIsRunning = false;
    private bool gameEnded = false;
    private int totalPenalties = 0;

    private void Start()
    {
        InitializeGame();
        SetupButtonListeners();
    }

    private void Update()
    {
        if (!timerIsRunning || gameEnded) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerDisplay();

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            StartCoroutine(EndGameWithDelay());
        }
    }

    private void InitializeGame()
    {
        timeRemaining = initialTime;
        timerIsRunning = true;
        gameEnded = false;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        UpdateTimerDisplay();
    }

    private void SetupButtonListeners()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        else
            Debug.LogError("Restart Button not assigned!");

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        else
            Debug.LogError("Quit Button not assigned!");
    }

    public void AddTimePenalty()
    {
        if (!gameEnded)
        {
            timeRemaining -= timePenalty;
            totalPenalties++;
            UpdateTimerDisplay();
            Debug.Log($"Penalty added! Total now: {totalPenalties}", this);
            totalPenaltiesText.text = $"Total Penalties: {totalPenalties}";
        }
    } 
    
            

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int seconds = Mathf.FloorToInt(timeRemaining);
            int milliseconds = Mathf.FloorToInt((timeRemaining * 100) % 100);
            timerText.text = string.Format("Time : {0:00}:{1:00}", seconds, milliseconds);
        }
    }

    private IEnumerator EndGameWithDelay()
    {
        timerIsRunning = false;
        yield return new WaitForSeconds(gameOverDelay);
        GameOver();
    }

    private void GameOver()
    {
        gameEnded = true;
        
        if (gameOverPanel != null)
        {
            // Update texts BEFORE activating panel
            if (finalScoreText != null && ScoreManager.Instance != null)
            {
                finalScoreText.text = $"Final Score: {ScoreManager.Instance.CurrentScore}";
            }
            gameOverPanel.SetActive(true); // Activate last
        }

        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        ScoreManager.Instance?.ResetScore();
        FindObjectOfType<LetterShuffler>()?.SetGameActive(true);
        InitializeGame();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}