using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Human : MonoBehaviour
{
    public float Height;
    public float Speed;

    public float HP;
    public float Armor;

    [HideInInspector]
    public Vector3 InputControl;
    [HideInInspector]
    public bool InputFire;

    [HideInInspector]
    public Vector3 Forward = Vector3.forward;
    [HideInInspector]
    public Vector3 Right = Vector3.right;

    [HideInInspector]
    public Action IdleLook; 

    private RaycastHit _hit;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out _hit))
        {
            transform.position = _hit.point + Vector3.up * Height;
        }
        if (InputControl.magnitude == 0)
        {
            if(IdleLook != null)IdleLook();
        }
        else
        {
            var move = (Forward * InputControl.z + Right * InputControl.x).normalized;

            transform.LookAt(transform.position + move);
            if (!_rigidbody.SweepTest(move, out _hit, Speed * Time.deltaTime*2))
            {
                transform.Translate(move * Speed * Time.deltaTime, Space.World);
            }
            else
            {
                // TODO ignore certain collisions
            }
        }

        if (InputFire)
        {
            // TODO pool this
            var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.AddComponent<Rigidbody>().isKinematic = true;
            bullet.AddComponent<Bullet>();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.localScale = Vector3.one*0.1f;
        }
    }
}
