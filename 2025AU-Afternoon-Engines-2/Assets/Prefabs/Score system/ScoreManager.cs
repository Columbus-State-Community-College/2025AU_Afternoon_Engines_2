using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public int currentScore = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddPoints(int amount)
    {
        currentScore += amount;
        Debug.Log($"[ScoreManager] Added {amount} pts | Total: {currentScore}");
        UpdateUI();
    }

    public bool SpendPoints(int amount)
    {
        if (currentScore >= amount)
        {
            currentScore -= amount;
            UpdateUI();
            return true;
        }
        Debug.Log("[ScoreManager] Not enough points!");
        return false;
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"placeholder score: {currentScore}";
            Debug.Log($"[ScoreManager] UI updated: {scoreText.text}");
        }
        else
        {
            Debug.LogWarning("[ScoreManager] Missing scoreText reference!");
        }
    }
}
