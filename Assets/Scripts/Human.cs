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

        var move = (Forward * InputControl.z + Right * InputControl.x).normalized;
        if (InputControl.magnitude == 0)
        {
            if(IdleLook != null)IdleLook();
        }
        else
        {
            transform.LookAt(transform.position + move);
        }
        _rigidbody.velocity = move * Speed;

        if (InputFire)
        {
            // TODO pool this
            var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.AddComponent<Rigidbody>().useGravity = false;
            bullet.AddComponent<Bullet>().Owner = transform;
            bullet.AddComponent<Bullet>().Forward = transform.forward;
            bullet.transform.position = transform.position;
            bullet.transform.localScale = Vector3.one*0.1f;
        }
        InputControl = Vector3.zero;
        InputFire = false;
    }
}
