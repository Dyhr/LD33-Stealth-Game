using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Human))]
public class Player : MonoBehaviour
{
    public Camera Camera;
    public float ActivateDistance;

    public Material NodeMaterial;
    public Material PlaneMaterial;
    public Transform NodeLabel;
    public AudioClip TeleClip;

    private HackMode hacker;
    private Networkable _hack;
    public Networkable Hack
    {
        set
        {
            _hack = value;
            if (_hack != null && hacker == null) { 
                hacker = new GameObject("Hacker").AddComponent<HackMode>();
                hacker.NodeMaterial = NodeMaterial;
                hacker.PlaneMaterial = PlaneMaterial;
                hacker.NodeLabel = NodeLabel;
                hacker.Origin = value;
            }
            if(_hack == null && hacker != null)
                Destroy(hacker.gameObject);
        }
        get { return _hack; }
    }

    private RaycastHit _hit;
    private Human _human;
    private GameObject[] _switches;

    private void Start()
    {
        if (Guard._player == null)
            Guard._player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Guard._playerr == null)
            Guard._playerr = Guard._player.GetComponent<Rigidbody>();

        if (Camera == null) Camera = Camera.main;
        _human = GetComponent<Human>();
        _human.IdleLook = () =>
        {
            if (Hack == null)
            {
                if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out _hit))
                {
                    var m = _hit.point + transform.right * 0.25f;
                    m.y = transform.position.y;
                    transform.LookAt(m); // TODO lerp
                }
            }
        };
        _human.Forward = Camera.transform.forward;
        _human.Forward.y = 0;
        _human.Right = Camera.transform.right;
        _human.Right.y = 0;
        _switches = GameObject.FindGameObjectsWithTag("Switch");

        GetComponent<AudioSource>().PlayOneShot(TeleClip, 0.8f);
    }

    private void Update()
    {
        if (Hack == null)
        {
            _human.InputControl = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            _human.InputFire = Input.GetButtonDown("Fire1") && Input.GetButton("Fire2");
            _human.InputAim = Input.GetButton("Fire2");

            Camera.transform.position = transform.position - Camera.transform.forward*40;
            if (Input.GetAxisRaw("Fire2") > 0) _human.IdleLook();

            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Camera.transform.position += _human.Right*Input.GetAxisRaw("Mouse X") +
                                         _human.Forward*Input.GetAxisRaw("Mouse Y");

            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Label.INSTANCE != null)
            Label.INSTANCE.Target = null;
        var s = _switches.OrderBy(h => Vector3.Distance(transform.position, h.transform.position)).FirstOrDefault();
        if (s != null && Vector3.Distance(transform.position, s.transform.position) <= ActivateDistance)
        {
            var item = s.GetComponent<Networkable>();
            if (item != null)
            {
                if (Label.INSTANCE != null && Hack == null)
                {
                    Label.INSTANCE.Target = s.transform;
                    Label.INSTANCE.Text.text = item.Name + "\nLevel " + NetNode.Roman(item.Level) + "\n" + item.Action;
                }
                if (Input.GetButtonDown("Action"))
                {
                    s.transform.SendMessage("Activate", _human.Creds);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (Label.INSTANCE != null)
            Label.INSTANCE.Target = null;
        Hack = null;
        Destroy(FindObjectOfType<HUD>());
    }
}
