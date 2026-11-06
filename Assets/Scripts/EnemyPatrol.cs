using UnityEngine;

public enum PatrolMode { Loop, PingPong }

[RequireComponent(typeof(Rigidbody))]
public class EnemyPatrol : MonoBehaviour
{
    enum State { Patrol, Chase, Return }

    [SerializeField] Transform[] points;
    [SerializeField] PatrolMode mode = PatrolMode.Loop;
    [SerializeField] float patrolSpeed = 2f, chaseSpeed = 3.5f, turnSpeed = 360f;
    [SerializeField] float chaseRange = 4f, giveUpRange = 6f, arriveDistance = 0.1f;
    [SerializeField] Renderer body;
    [SerializeField] Material patrolMaterial, chaseMaterial;

    State state = State.Patrol;
    int index, step = 1;
    Transform player;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<PlayerController>().transform;   // one prefab fits every level
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Patrol:
                MoveTo(points[index].position, patrolSpeed);
                if (Arrived(points[index].position)) index = NextIndex();
                if (InRange(chaseRange)) SetState(State.Chase);
                break;
            case State.Chase:
                Vector3 target = player.position;
                target.y = rb.position.y;                  // stays on its own floor
                MoveTo(target, chaseSpeed);
                if (!InRange(giveUpRange))                 // lost outside the longer range
                {
                    index = Nearest();
                    SetState(State.Return);
                }
                break;
            case State.Return:
                MoveTo(points[index].position, patrolSpeed);
                if (InRange(chaseRange)) SetState(State.Chase);
                else if (Arrived(points[index].position)) SetState(State.Patrol);
                break;
        }
    }

    void MoveTo(Vector3 target, float speed)
    {
        Vector3 toTarget = target - rb.position;
        toTarget.y = 0f;
        rb.MovePosition(Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));
        if (toTarget != Vector3.zero)
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(toTarget),
                                                     turnSpeed * Time.fixedDeltaTime));
    }

    // arrival by distance, not ==
    bool Arrived(Vector3 target) => Vector3.Distance(rb.position, target) < arriveDistance;

    // seen inside one range, lost outside a longer one
    bool InRange(float range) => Vector3.Distance(rb.position, player.position) < range;

    int NextIndex()
    {
        if (mode == PatrolMode.Loop) return (index + 1) % points.Length;
        if (index + step < 0 || index + step >= points.Length) step = -step;
        return index + step;
    }

    int Nearest()
    {
        int best = 0;
        for (int i = 1; i < points.Length; i++)
            if (Vector3.Distance(rb.position, points[i].position) < Vector3.Distance(rb.position, points[best].position))
                best = i;
        return best;
    }

    void SetState(State next)
    {
        state = next;
        body.sharedMaterial = next == State.Chase ? chaseMaterial : patrolMaterial;
    }
}
