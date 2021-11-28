using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] waypoints;
    public int wpIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[wpIndex].transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, waypoints[wpIndex].transform.position) < 2)
        {
            wpIndex++;
            if (wpIndex == waypoints.Length) wpIndex = 0;
            agent.SetDestination(waypoints[wpIndex].transform.position);
        }

    }
}
