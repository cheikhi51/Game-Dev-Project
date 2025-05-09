using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Image))]
public class LetterShuffler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button validButton;
    [SerializeField] private Button invalidButton;
    [SerializeField] private Image letterDisplay;

    [Header("Game Settings")]
    [SerializeField] private bool allowEmptyLists = false;
    [SerializeField] private float answerDelay = 0.5f; // Delay after answer before next letter

    private List<Sprite> consonantSprites = new List<Sprite>();
    private List<Sprite> vowelSprites = new List<Sprite>();
    private bool isCurrentConsonant;
    private bool isGameActive = true;
    private Timer gameTimer;

    private void Awake()
    {
        InitializeReferences();
        LoadSpritesFromSpritesFolder();
        ValidateResources();
        gameTimer = FindObjectOfType<Timer>();
    }

    private void Start()
    {
        SetupButtonListeners();
        ShuffleLetter();
    }

    private void InitializeReferences()
    {
        if (letterDisplay == null)
            letterDisplay = GetComponent<Image>();
    }

    private void LoadSpritesFromSpritesFolder()
    {
#if UNITY_EDITOR
        consonantSprites = LoadSpritesAtPath("Assets/Sprites/Letters/Consonants");
        vowelSprites = LoadSpritesAtPath("Assets/Sprites/Letters/Vowels");
#else
        consonantSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Letters/Consonants"));
        vowelSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Letters/Vowels"));
#endif
    }

#if UNITY_EDITOR
    private List<Sprite> LoadSpritesAtPath(string path)
    {
        List<Sprite> sprites = new List<Sprite>();
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { path });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null)
            {
                sprites.Add(sprite);
            }
        }
        return sprites;
    }
#endif

    private void ValidateResources()
    {
        if (!allowEmptyLists)
        {
            if (consonantSprites.Count == 0)
                Debug.LogError($"No consonant sprites found at: {Application.dataPath}/Sprites/Letters/Consonants");
            if (vowelSprites.Count == 0)
                Debug.LogError($"No vowel sprites found at: {Application.dataPath}/Sprites/Letters/Vowels");
            if (consonantSprites.Count == 0 && vowelSprites.Count == 0)
                Debug.LogError("No sprites found in either folder!");
        }
    }

    private void SetupButtonListeners()
    {
        if (validButton != null)
            validButton.onClick.AddListener(() => OnAnswerGiven(true));
        else
            Debug.LogError("Valid Button reference not set!");

        if (invalidButton != null)
            invalidButton.onClick.AddListener(() => OnAnswerGiven(false));
        else
            Debug.LogError("Invalid Button reference not set!");
    }

    private void OnAnswerGiven(bool playerSaysValid)
    {
        if (!isGameActive) return;
        
        // Disable buttons temporarily
        SetButtonsInteractable(false);
        
        // Process answer
        CheckAnswer(playerSaysValid);
        
        // Delay before next letter
        Invoke(nameof(PrepareNextLetter), answerDelay);
    }

    private void PrepareNextLetter()
    {
        if (isGameActive)
        {
            SetButtonsInteractable(true);
            ShuffleLetter();
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        validButton.interactable = state;
        invalidButton.interactable = state;
    }

    public void ShuffleLetter()
    {
        if (!HasValidSprites() || !isGameActive)
        {
            Debug.LogWarning("Cannot shuffle letter - game not active or no sprites available");
            return;
        }

        isCurrentConsonant = Random.Range(0, 2) == 0;
        List<Sprite> targetSprites = isCurrentConsonant ? consonantSprites : vowelSprites;

        if (targetSprites.Count == 0 && allowEmptyLists)
        {
            isCurrentConsonant = !isCurrentConsonant;
            targetSprites = isCurrentConsonant ? consonantSprites : vowelSprites;
        }

        if (targetSprites.Count > 0)
        {
            letterDisplay.sprite = targetSprites[Random.Range(0, targetSprites.Count)];
        }
    }

    private bool HasValidSprites()
    {
        return allowEmptyLists 
            ? (consonantSprites.Count > 0 || vowelSprites.Count > 0)
            : (consonantSprites.Count > 0 && vowelSprites.Count > 0);
    }

    private void CheckAnswer(bool playerSaysValid)
    {
        bool isCorrect = playerSaysValid == isCurrentConsonant;
        
        if (ScoreManager.Instance != null)
        {
            if (isCorrect)
            {
                ScoreManager.Instance.AddScore(1);
                Debug.Log("Correct! +1 point");
            }
            else
            {
                ScoreManager.Instance.AddScore(-2);
                gameTimer?.AddTimePenalty();
                Debug.Log("Incorrect! -2 points and +1 second");
            }
        }
    }

    public void SetGameActive(bool active)
    {
        isGameActive = active;
        SetButtonsInteractable(active);
        
        if (!active)
        {
            CancelInvoke(nameof(PrepareNextLetter));
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (letterDisplay == null)
            letterDisplay = GetComponent<Image>();
    }
#endif
}