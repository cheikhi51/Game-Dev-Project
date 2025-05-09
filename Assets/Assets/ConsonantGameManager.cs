using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsonantGameManager : MonoBehaviour
{
    public GameObject letterPrefab;
    public Transform lettersContainer;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI scoreText;
    
    private List<string> consonants = new List<string> { "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Y", "Z" };
    private List<GameObject> letterButtons = new List<GameObject>();
    private string currentTarget;
    private int score = 0;
    
    void Start()
    {
        SetupGame();
    }
    
    public void SetupGame()
    {
        // Clear any existing letters
        foreach (GameObject button in letterButtons)
        {
            Destroy(button);
        }
        letterButtons.Clear();
        
        // Create letter buttons for each consonant
        foreach (string letter in consonants)
        {
            GameObject newButton = CreateLetterButton(letter);
            letterButtons.Add(newButton);
        }
        
        // Initialize game variables
        score = 0;
        UpdateScoreText();
        SetNewTargetLetter();
    }
    
    private GameObject CreateLetterButton(string letter)
    {
        // Instantiate the button from prefab
        GameObject letterObj = new GameObject(letter);
        letterObj.transform.SetParent(lettersContainer, false);
        
        // Add UI components
        RectTransform rectTransform = letterObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 100);
        
        Image image = letterObj.AddComponent<Image>();
        image.color = Color.white;
        
        Button button = letterObj.AddComponent<Button>();
        button.targetGraphic = image;
        
        // Add text component for the letter
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(letterObj.transform, false);
        
        TextMeshProUGUI textComp = textObj.AddComponent<TextMeshProUGUI>();
        textComp.text = letter;
        textComp.fontSize = 48;
        textComp.color = Color.black;
        textComp.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = textComp.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        // Add click handler
        string capturedLetter = letter;
        button.onClick.AddListener(() => OnLetterClicked(capturedLetter, letterObj));
        
        return letterObj;
    }
    
    private void OnLetterClicked(string letter, GameObject letterObj)
    {
        if (letter == currentTarget)
        {
            // Correct letter
            score += 10;
            UpdateScoreText();
            StartCoroutine(CorrectLetterFeedback(letterObj));
            SetNewTargetLetter();
        }
        else
        {
            // Wrong letter
            score = Mathf.Max(0, score - 5);
            UpdateScoreText();
            StartCoroutine(WrongLetterFeedback(letterObj));
        }
    }
    
    private IEnumerator CorrectLetterFeedback(GameObject letterObj)
    {
        Image img = letterObj.GetComponent<Image>();
        if (img != null)
        {
            Color originalColor = img.color;
            img.color = Color.green;
            yield return new WaitForSeconds(0.5f);
            img.color = originalColor;
        }
    }
    
    private IEnumerator WrongLetterFeedback(GameObject letterObj)
    {
        Image img = letterObj.GetComponent<Image>();
        if (img != null)
        {
            Color originalColor = img.color;
            img.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            img.color = originalColor;
        }
    }
    
    private void SetNewTargetLetter()
    {
        // Choose a random consonant
        currentTarget = consonants[Random.Range(0, consonants.Count)];
        promptText.text = "Find the letter: " + currentTarget;
    }
    
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
