using System.Linq;
using Pathfinding;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Human))]
[RequireComponent(typeof(Seeker))]
public class Guard : MonoBehaviour
{
    public float nextWaypointDistance = 3;
    public Vector3 targetPosition;

    public float SlowSpeed;
    public float NormalSpeed;
    public float FastSpeed;
    public float DetectionAngle;

    private RaycastHit _hit;
    private Human human;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private static GameObject[] _switches;
    private static Transform _player;
    private static Rigidbody _playerr;
    private bool _awaitingPath;
    private int alert;

    private void Start()
    {
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;
        seeker.StartPath(transform.position, targetPosition);
        if (_switches == null)
            _switches = GameObject.FindGameObjectsWithTag("Switch");
        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (_playerr == null)
            _playerr = _player.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (path != null)
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                Debug.Log("End Of Path Reached");
                path = null;
                alert = 0;
                return;
            }

            human.InputControl = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                currentWaypoint++;
        }

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
        var dir = (_player.position - transform.position).normalized;
        if (_player != null && Physics.Raycast(transform.position, dir, out _hit))
        {
            if (_hit.transform == _player && Vector3.Dot(dir, transform.forward) > Mathf.Cos(DetectionAngle * Mathf.Deg2Rad))
            {
                if (!_awaitingPath && Vector3.Distance(_player.position, targetPosition) > 1)
                {
                    path = null;
                    targetPosition = _player.position;
                    _awaitingPath = true;
                    seeker.StartPath(transform.position, targetPosition);
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((_player.position + _playerr.velocity * 0.2f) - transform.position), 0.5f);
                human.InputFire = true;
                human.LockRot = true;
                alert = 2;
            }
        }

        human.Speed = alert == 0 ? NormalSpeed : (alert == 2 ? SlowSpeed : FastSpeed);

        if (alert == 2)
            alert = 1;
    }
    public void OnPathComplete(Path p)
    {
        if (p.error) return;
        path = p;
        currentWaypoint = 0;
        _awaitingPath = false;
    }
}
