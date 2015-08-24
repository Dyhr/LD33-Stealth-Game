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
    public int Level;

    public Creds Creds
    {
        get { return new Creds(name, Level, this); }
    }

    public AnimationCurve AimCurve;
    public float AimTimeMul;

    [HideInInspector]
    public Vector3 InputControl;
    [HideInInspector]
    public bool InputAim;
    [HideInInspector]
    public bool InputFire;

    [HideInInspector]
    public Vector3 Forward = Vector3.forward;
    [HideInInspector]
    public Vector3 Right = Vector3.right;

    [HideInInspector]
    public Action IdleLook;
    [HideInInspector]
    public bool LockRot;

    private Rigidbody _rigidbody;
    private float aimTime;

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

        var move = (Forward*InputControl.z + Right*InputControl.x).normalized;
        if (InputControl.magnitude == 0)
        {
            if (IdleLook != null) IdleLook();
        }
        else if (!LockRot)
        {
            if(move.magnitude > 0)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(move), 0.4f);
        }
        _rigidbody.velocity = move*Speed;

        var gun = GetComponentInChildren<Gun>();
        if (gun != null) {
            aimTime += (InputAim ? -Time.deltaTime : Time.deltaTime) * AimTimeMul;
            aimTime = Mathf.Clamp01(aimTime);
            gun.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(90, 0, 0), Quaternion.Euler(0, 0, 0), AimCurve.Evaluate(aimTime));
            if (InputFire && aimTime == 0)
            {
                gun.Fire();
            }
        }

        InputControl = Vector3.zero;
        InputFire = false;
        InputAim = false;
        LockRot = false;
    }
}
