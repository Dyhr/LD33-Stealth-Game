using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float Speed = 40;
    public float Damage = 10;
    public float Piercing = 1;

    [HideInInspector]
    public Transform Owner;
    [HideInInspector]
    public Vector3 Forward;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine("Init");
    }

    IEnumerator Init()
    {
        while (Owner == null || Forward.magnitude == 0)
        {
            if (Owner != null) Forward = Owner.transform.forward;
            yield return null;
        }

        Destroy(gameObject, 10);
        GetComponent<Rigidbody>().velocity = Forward * Speed;
        GetComponent<Collider>().enabled = true;
        Physics.IgnoreCollision(GetComponent<Collider>(), Owner.GetComponent<Collider>());
    }

    private void OnCollisionEnter(Collision c)
    {
        var human = c.transform.GetComponent<Human>();
        if (human != null)
        {
            //human.HP -= Mathf.Max(Damage - Mathf.Max(human.Armor - Piercing, 0), 0);
        }
        Destroy(gameObject);
    }
}
