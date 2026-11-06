using UnityEngine;

// One kick straight up. VelocityChange ignores mass, so the number is the launch speed in m/s.
public class BouncePad : MonoBehaviour
{
    [SerializeField] float launchSpeed = 8.5f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Rigidbody body = other.attachedRigidbody;
        Vector3 v = body.linearVelocity;
        body.linearVelocity = new Vector3(v.x, 0f, v.z);   // same bounce whether you walk or fall onto it
        body.AddForce(Vector3.up * launchSpeed, ForceMode.VelocityChange);
    }
}
