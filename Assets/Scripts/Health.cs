using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int max = 3;
    [SerializeField] float iFrames = 1f;
    [SerializeField] Renderer body;        // blinks while invulnerable
    int current;
    float invulnerableUntil;

    public int Current => current;
    public int Max => max;

    void Awake() => current = max;

    void Update()
    {
        bool invulnerable = Time.time < invulnerableUntil;
        body.enabled = !invulnerable || Mathf.Repeat(Time.time, 0.2f) < 0.1f;
    }

    // A hazard can ask every physics step. It only lands once per iFrames seconds.
    public bool TakeDamage(int amount)
    {
        if (current <= 0 || Time.time < invulnerableUntil) return false;
        invulnerableUntil = Time.time + iFrames;
        current -= amount;
        GameManager.Instance.UpdateHealth();
        if (current <= 0) GameManager.Instance.PlayerDied();   // <= 0: a crusher's 2 can skip past 0
        return true;
    }

    public void Restore(float protection)
    {
        current = max;
        invulnerableUntil = Time.time + protection;
        GameManager.Instance.UpdateHealth();
    }
}
