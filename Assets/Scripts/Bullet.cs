using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float Speed = 100;
    public float Damage = 10;
    public float Piercing = 1;

    [HideInInspector]
    public Transform Owner;

    private RaycastHit _hit;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject,5);
    }

    private void Update()
    {
        if (!_rigidbody.SweepTest(transform.forward, out _hit, Speed*Time.deltaTime*2))
        {
            transform.position += transform.forward*Speed*Time.deltaTime;
        }
        else
        {
            if (_hit.transform == Owner) return;

            var human = _hit.transform.GetComponent<Human>();
            if (human != null)
            {
                human.HP -= Mathf.Max(Damage - Mathf.Max(human.Armor - Piercing, 0), 0);
            }
            Destroy(gameObject);
        }
    }
}
