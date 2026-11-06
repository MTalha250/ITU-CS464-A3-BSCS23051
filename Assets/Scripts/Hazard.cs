using System.Collections;
using UnityEngine;

// One script for every danger: spikes, the guard and the crusher.
public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 8f, knockTime = 0.3f;
    [SerializeField] float restTime = 0f, warnTime = 0.7f, strikeTime = 0.4f;   // restTime 0 = always on
    [SerializeField] GameObject warning, slab;

    IEnumerator Start()
    {
        if (restTime <= 0f) yield break;
        Collider zone = GetComponent<Collider>();
        while (true)
        {
            zone.enabled = false;
            slab.SetActive(false);
            yield return new WaitForSeconds(restTime);
            warning.SetActive(true);
            yield return new WaitForSeconds(warnTime);
            warning.SetActive(false);
            zone.enabled = true;
            slab.SetActive(true);                  // the slab shows the strike
            yield return new WaitForSeconds(strikeTime);
        }
    }

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
