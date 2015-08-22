using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Bullet : MonoBehaviour
{
    public float Damage = 10;
    public float Piercing = 1;
    public Material Material;

    [HideInInspector]
    public Transform Owner;
    [HideInInspector]
    public Vector3 Forward;

    private void Start()
    {
        GetComponent<LineRenderer>().enabled = false;
        StartCoroutine("Init");
    }

    IEnumerator Init()
    {
        while (Owner == null || Forward.magnitude == 0)
        {
            if (Owner != null) Forward = Owner.transform.forward;
            yield return null;
        }

        var line = GetComponent<LineRenderer>();
        line.enabled = true;
        line.SetWidth(0.1f,0.1f);
        line.sharedMaterial = Material;
        line.SetPosition(0,transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Forward, out hit))
        {
            var human = hit.transform.GetComponent<Human>();
            if (human != null)
            {
                human.HP -= Mathf.Max(Damage - Mathf.Max(human.Armor - Piercing, 0), 0);
            }
            line.SetPosition(1, hit.point);
        }
        else
        {
            line.SetPosition(1, transform.position+Forward*1000);
        }

        Destroy(gameObject, 0.2f);
    }
}
