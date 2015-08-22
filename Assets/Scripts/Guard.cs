using System.Linq;
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

    private RaycastHit _hit;
    private Human human;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private static GameObject[] _switches;

    private void Start()
    {
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;
        seeker.StartPath(transform.position, targetPosition);
        if (_switches == null)
            _switches = GameObject.FindGameObjectsWithTag("Switch");
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

        if (Physics.Raycast(transform.position, transform.forward, out _hit, 2))
        {
            if (_hit.transform.CompareTag("Switch"))
            {
                if (_hit.transform.GetComponent<Door>() != null)
                {
                    _hit.transform.SendMessage("Activate", "GUARD");
                }
            }
        }
    }
    public void OnPathComplete(Path p)
    {
        if (p.error) return;
        path = p;
        currentWaypoint = 0;
    }
}
