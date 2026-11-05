using UnityEngine;

// One script for every danger. Spikes for now.
public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 8f, knockTime = 0.3f;

    void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Health health)) return;
        if (!health.TakeDamage(damage)) return;    // only a hit that lands knocks back
        Rigidbody body = other.attachedRigidbody;
        if (body == null) return;
        Vector3 away = body.position - transform.position;
        away.y = 0f;
        Vector3 push = away.normalized * knockback;
        if (other.TryGetComponent(out PlayerController p))
            p.Knockback(push, knockTime);          // the controller stops steering for knockTime
        else
            body.AddForce(push, ForceMode.Impulse);
    }
}
