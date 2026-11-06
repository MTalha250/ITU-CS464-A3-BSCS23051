using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int targetScore = 5, lives = 3;
    [SerializeField] float respawnDelay = 0.6f, spawnProtection = 1.5f;
    [SerializeField] string title = "Level01", tip = "";
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;
    [SerializeField] TMP_Text titleText, scoreText, heartsText, livesText, messageText, winText;
    [SerializeField] GameObject winPanel, losePanel, nextButton;

    int score;
    float messageUntil;
    bool over, respawning;

    void Awake() => Instance = this;

    void Start()
    {
        titleText.text = title;
        UpdateScore();
        UpdateHealth();
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

    public void UpdateHealth()
    {
        heartsText.text = $"Hearts: {Mathf.Max(player.Current, 0)} / {player.Max}";
        livesText.text = $"Lives: {lives}";
    }

    public void ReachGoal()
    {
        if (score >= targetScore) Win();
        else Show($"You need {targetScore - score} more coins", 2f);
    }

    public void PlayerDied()
    {
        if (over || respawning) return;   // a fall during the respawn beat doesn't cost a second life
        lives--;
        UpdateHealth();
        if (lives <= 0) Lose();
        else StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        respawning = true;
        yield return new WaitForSeconds(respawnDelay);     // a beat, so the hit's knockback is over
        Rigidbody body = player.GetComponent<Rigidbody>();
        body.position = respawnPoint.position;
        player.transform.position = respawnPoint.position;
        body.linearVelocity = Vector3.zero;                // back standing still
        player.Restore(spawnProtection);                   // full hearts and a moment of protection
        respawning = false;
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

    void Lose()
    {
        over = true;
        losePanel.SetActive(true);
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
