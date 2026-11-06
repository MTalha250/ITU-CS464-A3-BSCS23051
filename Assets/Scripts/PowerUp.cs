using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float multiplier = 1.6f, duration = 5f, spinSpeed = 120f;

    void Update() => transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // the timer runs on the player, so destroying the pickup can't stop it
        other.GetComponent<PlayerController>().BoostSpeed(multiplier, duration);
        Destroy(gameObject);
    }
}
