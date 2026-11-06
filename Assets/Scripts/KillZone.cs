using UnityEngine;

// One wide trigger under the level. A fall skips the hearts and costs a life.
public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.Instance.PlayerDied();
    }
}
