using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int targetScore = 5;
    [SerializeField] string title = "Level01", tip = "";
    [SerializeField] TMP_Text titleText, scoreText, messageText, winText;
    [SerializeField] GameObject winPanel, nextButton;

    int score;
    float messageUntil;
    bool over;

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

    public void ReachGoal()
    {
        if (score >= targetScore) Win();
        else Show($"You need {targetScore - score} more coins", 2f);
    }

    void Show(string text, float seconds)
    {
        messageText.text = text;
        messageUntil = Time.time + seconds;
    }

    void Win()
    {
        if (over) return;
        over = true;
        bool last = SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
        winText.text = last ? "You beat all five levels!" : "Level complete!";
        nextButton.SetActive(!last);
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;                               // first: it survives the scene load
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
