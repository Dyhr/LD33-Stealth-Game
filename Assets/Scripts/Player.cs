using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Human))]
public class Player : MonoBehaviour
{
    public Camera Camera;

    private RaycastHit _hit;
    private Human _human;

    private void Start()
    {
        _human = GetComponent<Human>();
        _human.IdleLook = () =>
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                var m = _hit.point;
                m.y = transform.position.y;
                transform.LookAt(m); // TODO lerp
            }
        };
        _human.Forward = Camera.transform.forward;
        _human.Forward.y = 0;
        _human.Right = Camera.transform.right;
        _human.Right.y = 0;
    }

    private void Update()
    {
        _human.InputControl = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _human.InputFire = Input.GetAxisRaw("Fire1") > 0;

        Camera.transform.position = transform.position - Camera.transform.forward * 100;
        if (Input.GetAxisRaw("Fire2") > 0) _human.IdleLook();
    }
}
