using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int targetScore = 5;
    [SerializeField] string title = "Level01", tip = "";
    [SerializeField] TMP_Text titleText, scoreText, messageText;

    int score;
    float messageUntil;

    void Awake() => Instance = this;

    void Start()
    {
        titleText.text = title;
        UpdateScore();
        Show(tip, 6f);
    }

    void Update()
    {
        if (messageText.text != "" && Time.time >= messageUntil) messageText.text = "";
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();
    }

    void UpdateScore() => scoreText.text = $"Coins: {score} / {targetScore}";

    void Show(string text, float seconds)
    {
        messageText.text = text;
        messageUntil = Time.time + seconds;
    }
}
