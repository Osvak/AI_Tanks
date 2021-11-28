using UnityEngine;
using UnityEngine.AI;

public class WanderScript : MonoBehaviour
{
    NavMeshAgent agent;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Wander();
    }

    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float radius = 2f;
        float offset = 3f;
        Vector3 localTarget = new Vector3(
            Random.Range(-1.0f, 1.0f), 0,
            Random.Range(-1.0f, 1.0f));
        localTarget.Normalize();
        localTarget *= radius;
        localTarget += new Vector3(0, 0, offset);
        Vector3 worldTarget =
            transform.TransformPoint(localTarget);
        worldTarget.y = 0f;
    }
}
