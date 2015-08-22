using Pathfinding;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Human))]
[RequireComponent(typeof(Seeker))]
public class Guard : MonoBehaviour
{
    public Player Player;
    public float nextWaypointDistance = 3;
    public Vector3 targetPosition;

    private Human human;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;

    private void Start()
    {
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;
        seeker.StartPath(transform.position, targetPosition);
    }

    public void Update()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            Debug.Log("End Of Path Reached");
            path = null;
            return;
        }

        human.InputControl = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            currentWaypoint++;
    }
    public void OnPathComplete(Path p)
    {
        if (p.error) return;
        path = p;
        currentWaypoint = 0;
    }
}
