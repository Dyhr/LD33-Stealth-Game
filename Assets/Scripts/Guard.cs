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
    public float AlertTime;
    public float AlertLevel;
    public AudioClip AlarmSound;

    public bool CanSeePlayer;

    private RaycastHit _hit;
    private Human human;
    internal Seeker seeker;
    internal Path path;
    private int currentWaypoint = 0;
    public static AudioClip AlarmClip;
    public static GameObject[] _switches;
    public static readonly Dictionary<Transform, float> _patrols = new Dictionary<Transform, float>();
    public static Transform _player;
    public static Rigidbody _playerr;
    public static GameObject[] _guards;
    internal bool _awaitingPath;
    internal int alert;

    private void Start()
    {
        AlarmClip = AlarmSound;
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;
        if (_switches == null)
            _switches = GameObject.FindGameObjectsWithTag("Switch");
        if (_guards == null)
            _guards = GameObject.FindGameObjectsWithTag("Guard");
        if (_patrols.Count == 0)
            foreach (var go in GameObject.FindGameObjectsWithTag("Patrol"))
                _patrols.Add(go.transform, 0);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Reinforce();
    }

    private void Reinforce()
    {
        if (human.Level < AlertLevel || FindObjectOfType<AstarPath>() == null || _guards == null) return;
        if(_player!= null)
            _player.GetComponent<AudioSource>().PlayOneShot(AlarmSound);
        for (int i = 0; i < _guards.Length; ++i)
        {
            if (_guards[i] == null) continue;

            var g = _guards[i].GetComponent<Guard>();
            g.path = null;
            g.targetPosition = transform.position;
            g._awaitingPath = true;
            g.seeker.StartPath(g.transform.position, g.targetPosition);
            g.alert = 2;
        }
    }

    private bool Alarming;
    public void Alarm()
    {
        if(!Alarming)
            StartCoroutine(DoAlarm());
    }

    IEnumerator DoAlarm()
    {
        Alarming = true;
        yield return new WaitForSeconds(AlertTime);
        Alarming = false;
        if (!CanSeePlayer) yield break;

        if (FindObjectOfType<AstarPath>() == null || _guards == null) yield break;
        for (int i = 0; i < _guards.Length; ++i)
        {
            if (_guards[i] == null) continue;

            var g = _guards[i].GetComponent<Guard>();
            g.path = null;
            g.targetPosition = _player.position;
            g._awaitingPath = true;
            g.seeker.StartPath(g.transform.position, g.targetPosition);
            g.alert = 2;
        }
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
                if (int.Parse(patrol.name.Split('-')[0]) != human.Level) continue;
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
                targetPosition = max[Random.Range(0, max.Count)].position;
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
                    _hit.transform.parent.SendMessage("Activate", human.Creds);
                }
                if (_hit.transform.parent.GetComponent<Guard>() != null)
                {
                    Interrupt();
                    //human.InputControl = (transform.right - transform.forward).normalized;
                }
            }
        }
        CanSeePlayer = false;
        var dir = _player != null ? (_player.position - transform.position).normalized : Vector3.zero;
        if (_player != null && Physics.Raycast(transform.position, dir, out _hit))
        {
            if (_hit.transform == _player && Vector3.Dot(dir, transform.forward) > Mathf.Cos(DetectionAngle * Mathf.Deg2Rad))
            {
                CanSeePlayer = true;
                if (!_awaitingPath && Vector3.Distance(_player.position, targetPosition) > 1)
                {
                    path = null;
                    targetPosition = _player.position;
                    _awaitingPath = true;
                    seeker.StartPath(transform.position, targetPosition);
                }
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation((_player.position + transform.right * 0.25f + _playerr.velocity * 0.2f) - transform.position), 0.5f);
                human.InputFire = true;
                human.LockRot = true;
                if (alert == 0)
                    Alarm();
                alert = 2;
            }
        }

        human.InputAim = alert > 0;
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
