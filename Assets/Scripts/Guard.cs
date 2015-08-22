using System.Collections.Generic;
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
    public static GameObject[] _switches;
    public static readonly Dictionary<Transform, float> _patrols = new Dictionary<Transform, float>();
    public static Transform _player;
    public static Rigidbody _playerr;
    private bool _awaitingPath;
    private int alert;

    private void Start()
    {
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;
        if (_switches == null)
            _switches = GameObject.FindGameObjectsWithTag("Switch");
        if (_patrols.Count == 0)
            foreach (var go in GameObject.FindGameObjectsWithTag("Patrol"))
                _patrols.Add(go.transform, 0);
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
                path = null;
                alert = 0;
                return;
            }

            human.InputControl = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                currentWaypoint++;
        }
        else if (!_awaitingPath)
        {
            var maxt = 0f;
            var max = new List<Transform>();
            foreach (var patrol in _patrols.Keys.ToArray())
            {
                var value = _patrols[patrol];
                _patrols[patrol] = value + Time.deltaTime;
                if (maxt < value)
                {
                    maxt = value;
                    max.Clear();
                }
                if (maxt == value)
                {
                    max.Add(patrol);
                    _patrols[patrol] = 0;
                }
            }
            if (max.Count > 0)
            {
                targetPosition = max[Random.Range(0,max.Count)].position;
                _awaitingPath = true;
                seeker.StartPath(transform.position, targetPosition);
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out _hit, 2))
        {
            if (_hit.transform.parent != null && _hit.transform.parent.CompareTag("Switch"))
            {
                if (_hit.transform.parent.GetComponent<Door>() != null)
                {
                    _hit.transform.parent.SendMessage("Activate", human);
                }
            }
        }
        var dir = _player != null ? (_player.position - transform.position).normalized : Vector3.zero;
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

    public void Interrupt()
    {
        path = null;
    }

    public void OnPathComplete(Path p)
    {
        if (p.error) return;
        path = p;
        currentWaypoint = 0;
        _awaitingPath = false;
    }
}
